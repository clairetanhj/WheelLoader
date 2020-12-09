using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;
using u040.prespective.core;
using u040.prespective.math;
using u040.prespective.utility;
using u040.prespective.prepair.kinematics;
using u040.prespective.prepair.physics;

namespace u040.prespective.referenceobjects.materialhandling.beltsystem
{
    /// <summary>
    /// @CLASS : BeltSystem
    /// 
    /// @ABOUT : Creates and controls a convayer belt using loftmesh
    /// 
    /// @AUTHOR: Pieter, Tymen, Mathijs (Unit040)
    /// 
    /// @VERSION: 08/07/2019 - V1.00 - Implemented alpha
    /// @VERSION: 08/07/2019 - V1.10 - Moved the loft mesh creations to its own class which this class inherits from and implemented basic apply belt speed through force
    /// @VERSION: 29/07/2019 - V1.15 - Implemented lookup table to find clossed points
    /// @VERSION: 30/07/2019 - V1.20 - Made fixed update calculations multi threaded
    /// @VERSION: 30/07/2019 - V1.21 - Made stored spline lookup table in local space of belt
    /// @VERSION: 16/09/2019 - V1.22 - Fixed sleeping rigidbodies bug
    /// @Version: 24/01/2020 - V1.30 - Belt surface points are now being buffered  and calculates closest points from the buffer. Belt is no longer a KinematicRelation. 
    /// </summary>

    public class BeltSystem : MonoBehaviour
    {
        #region<public variables>
        public WheelJoint WheelJoint;
        public float Velocity { get; private set; } = 0f;
        public bool InvertAxis = false;
        public int BufferedCircumferenceSplinePoint = 1000;
        public int BufferedSurfaceSplinePoints = 100;
        public bool ManageRotation = false;
        public LoftMesh LoftMesh = new LoftMesh();

        public bool EnableVelocityGizmo = true;
        public bool ShowGizmoWhenNotSelected = true;
        public Color VelocityGizmoColor = Color.red;
        #endregion

        #region<class variables>
        private bool Idle = true;
        private List<Rigidbody> IdleRigidbodies = new List<Rigidbody>();
        private BeltPointsDescription beltPointsDescription;
        private List<BeltObjectContact> BeltObjectContactList = new List<BeltObjectContact>();
        private float storedWheelJointPosition;
        private float surfacePlaneAngleMargin = 1f;
        private bool pointsProperlyBuffered = true;
        #endregion

        #region<collision calculations>
        private void OnCollisionStay(Collision _collision)
        {
            int _numberOfContactPoints = _collision.contactCount;

            //Calculate the average contact point and contact normal
            Vector3 _contactPointsSum = Vector3.zero;
            Vector3 _contactNormalSum = Vector3.zero;

            for (int _i = 0; _i < _numberOfContactPoints; _i++)
            {
                _contactPointsSum += _collision.contacts[_i].point;
                _contactNormalSum += _collision.contacts[_i].normal;
            }
            Vector3 _averageContactPoint = _contactPointsSum / _numberOfContactPoints;
            Vector3 _averageContactNormal = _contactNormalSum / _numberOfContactPoints;

            //Add the contact point to the list
            this.BeltObjectContactList.Add(new BeltObjectContact() { ObjectRigidbody = _collision.rigidbody, ContactPoint = _averageContactPoint, ContactNormal = _averageContactNormal });
        }
        #endregion

        #region<start>
        private void Awake()
        {
            bufferSurfacePoints();
            this.storedWheelJointPosition = this.WheelJoint ? this.WheelJoint.CurrentRevolutionPercentage : 0f;
            
            //Get the collider
            MeshCollider _collider = this.GetComponent<MeshCollider>();

            //If a collider is found
            if (_collider)
            {
                //Check if the physics material is the default
                if (_collider.material.name == "")
                {
                    //Create a new 'icy' physics material and assign it
                    _collider.material = new PhysicMaterial() { dynamicFriction = 0f, staticFriction = 0f, bounciness = 0f, frictionCombine = PhysicMaterialCombine.Minimum, bounceCombine = PhysicMaterialCombine.Average, name = "Generated Belt Physics Material" };
                }
                else
                {
                    //Log that we wont override a non-default physics material
                    Debug.LogWarning("Cannot generate a PhysicsMaterial on " + this.gameObject.name + "(" + this.GetType().Name + ") since a PhysicsMaterial has already been assigned");
                }   
            }

            //If no collider is found
            else
            {
                Debug.LogError(this.gameObject.name + "(" + this.GetType().Name + ") is unable to function since it has no collider");
            }
        }



        /// <summary>
        /// Buffer all world points on the belt surface so they wont have to be calculated in runtime
        /// </summary>
        private void bufferSurfacePoints()
        {
            //Create an array for all surface splines
            BeltSurfacePointsDescription[] _allSurfacePoints = new BeltSurfacePointsDescription[this.LoftMesh.SurfaceGuideSplines.Count];
            for (int _i = 0; _i < this.LoftMesh.SurfaceGuideSplines.Count; _i++)
            {
                //Buffer Spline
                Spline _surfaceSpline = this.LoftMesh.SurfaceGuideSplines[_i];

                //Create an array for all points on a surface spline
                Vector3[] _surfacePoints = new Vector3[this.BufferedSurfaceSplinePoints];
                for (int _j = 0; _j < this.BufferedSurfaceSplinePoints; _j++)
                {
                    //Calculate the point on the spline at a certain percentage in world space and add it to the array
                    _surfacePoints[_j] = _surfaceSpline.GetPointAtEquidistantPerc(_j / (float)this.BufferedSurfaceSplinePoints);
                }

                //Add all points on the current surface spline to the array of all point on the belt
                _allSurfacePoints[_i] = new BeltSurfacePointsDescription() { Points = _surfacePoints };
            }

            //Create an array for all circumference points
            Vector3[] _circumferencePoints = new Vector3[this.LoftMesh.SurfaceGuideSplines.Count];
            for (int _i = 0; _i < this.LoftMesh.SurfaceGuideSplines.Count; _i++)
            {
                //Add point at certain percentage of spline to array
                Vector3 _circumferecePoint;
                float _distance;
                Vector3 _surfacePoint = _allSurfacePoints[_i].Points[0];
                this.LoftMesh.LoftCircumferenceSpline.GetClosestPercentageToWorldPos(_surfacePoint, out _distance, out _circumferecePoint);
                _circumferencePoints[_i] = _circumferecePoint;
            }

            //Create a new BeltPointDescription of the circumference points and all surface points
            this.beltPointsDescription = new BeltPointsDescription() { CircumferencePoints = _circumferencePoints, SurfacePoints = _allSurfacePoints };
            if (this.beltPointsDescription.CircumferencePoints.Length < 2 || this.beltPointsDescription.SurfacePoints.Length < 2)
            {
                pointsProperlyBuffered = false;
                Debug.LogError("BeltSystem on " + this.gameObject.name + " is unable to function since the spline points could not be buffered.");
            }
        }
        #endregion

        #region<fixed update>
        private void FixedUpdate()
        {
            if (!this.pointsProperlyBuffered)
            {
                return;
            }

            //Determine the velocity on the belt by the angle difference on the wheeljoint
            DetermineWheeljointVelocity();

            //force wake up sleeping bodies when applying velocity after idle
            forceWakeupIdleBodies();

            //Only update objects if an actual velocity is set
            if (this.Velocity != 0f)
            {
                int _numberOfBeltObjectContacts = this.BeltObjectContactList.Count;
                int _numberofsurfaceguides = this.LoftMesh.SurfaceGuideSplines.Count;

                BeltForceIntent[] _forcesToBeApplied = new BeltForceIntent[_numberOfBeltObjectContacts];
                ConcurrentDictionary<int, BeltObjectContact> _contactsBag = this.BeltObjectContactList.ToConcurrentDictonairy();

                Parallel.For(0, _numberOfBeltObjectContacts, _i =>
                {
                    BeltObjectContact _contact = _contactsBag[_i];

                    //Write data to seperate memory location
                    BeltPointsDescription _beltPointsDescription = this.beltPointsDescription;

                    //Find the index of the closest surface point to the contact point
                    KeyValuePair<Vector3, Vector3> _closestPointIndexes = findBetweenWhichSurfacePoints(_beltPointsDescription, _contact);

                    //Calculate the force direction that needs to be applied on the rigidbody
                    Vector3 _forceDirectionVector = (_closestPointIndexes.Value - _closestPointIndexes.Key).normalized;

                    _forcesToBeApplied[_i] = new BeltForceIntent() { Rigidbody = _contact.ObjectRigidbody, PointOfForceApplication = _contact.ContactPoint, ForceDirection = _forceDirectionVector };
                });


                //Add the calculated force to the rigidbodies
                for (int _i = 0; _i < _forcesToBeApplied.Length; _i++)
                {
                    //Buffer the intent
                    BeltForceIntent _intent = _forcesToBeApplied[_i];

                    //Buffer rigidbody
                    Rigidbody _rig = _intent.Rigidbody;

                    //Velocity in the belt direction
                    Vector3 _currentDirectionalVelocity = Vector3.Project(_rig.velocity, _intent.ForceDirection);

                    //Target velocity
                    Vector3 _targetDirectionalVelocity = _intent.ForceDirection.normalized * this.Velocity;

                    //Difference in directional velocity
                    Vector3 _differenceDirectionalVelocity = _targetDirectionalVelocity - _currentDirectionalVelocity;

                    //Force needed to accelerate to target velocity
                    Vector3 _accelerationForce = _rig.mass * (_differenceDirectionalVelocity / Time.fixedDeltaTime);

                    //Apply force to rigidbody
                    _rig.AddForce(_accelerationForce);
                    //_rig.AddForceAtPosition(_accelerationForce, _intent.PointOfForceApplication, ForceMode.Force);

                    if (ManageRotation)
                    {
                        float _angle = Vector3.SignedAngle(_rig.velocity, _intent.ForceDirection, Vector3.up);
                        Quaternion _correction = Quaternion.AngleAxis(_angle, Vector3.up);
                        _rig.transform.rotation = _correction * _rig.transform.rotation;
                    }
                }
            }

            //Clear the list of contacts to recalculate each contact points next update
            this.BeltObjectContactList.Clear();
        }

        /// <summary>
        /// Determine the velocity of the belt by calculating how much the WheelJoint has rotated and what the equivalent arclength is
        /// </summary>
        private void DetermineWheeljointVelocity()
        {
            //If there is a WheelJoint and if the WheelJoint has changed rotation
            if (this.WheelJoint && this.WheelJoint.CurrentRevolutionPercentage != this.storedWheelJointPosition)
            {
                //Calculated how much it has changed since last tick
                float _differenceInPercentage = this.WheelJoint.CurrentRevolutionPercentage - this.storedWheelJointPosition;

                //Calculated the distance the belt would move over that rotation
                float _arcLength = _differenceInPercentage * (2f * Mathf.PI * this.WheelJoint.Radius);

                //Set the velocity to match the distance over the time of a tick
                this.Velocity = _arcLength / Time.fixedDeltaTime;

                //Invert the velocity is axis is inverted
                if (InvertAxis) { this.Velocity *= -1f; }

                //Store the current rotation for next change calculation
                this.storedWheelJointPosition = this.WheelJoint.CurrentRevolutionPercentage;
            }

            //If there is no WheelJoint or it has not changed rotation
            else
            {
                this.Velocity = 0f;
            }
        }

        /// <summary>
        /// Makes sure that the rigidbodies of objects on the belt stay awake
        /// </summary>
        private void forceWakeupIdleBodies()
        {
            //finds connected rigidbodies when no velocity
            if (this.Velocity == 0f)
            {
                if (!this.Idle) { this.Idle = true; }

                //this.IdleRigidbodies.AddRange(this.BeltObjectContactList);
                for (int _i = 0; _i < this.BeltObjectContactList.Count; _i++)
                {
                    if (!this.IdleRigidbodies.ContainsCheck<Rigidbody>(this.BeltObjectContactList[_i].ObjectRigidbody))
                    {
                        this.IdleRigidbodies.Add(this.BeltObjectContactList[_i].ObjectRigidbody);
                    }
                }
            }

            //force wake up sleeping bodies when applying velocity after idle
            else if (this.Idle)
            {
                for (int _i = 0; _i < this.IdleRigidbodies.Count; _i++)
                {
                    if (this.IdleRigidbodies[_i] != null)
                    {
                        this.IdleRigidbodies[_i].WakeUp();
                    }
                }
                this.Idle = false;
                this.IdleRigidbodies.Clear();
            }
        }

        /// <summary>
        /// Returns the two surface points between which the contact point is
        /// </summary>
        /// <param name="_beltPointsDescription"></param>
        /// <param name="_objectContactPoint"></param>
        /// <returns></returns>
        private KeyValuePair<Vector3, Vector3> findBetweenWhichSurfacePoints(BeltPointsDescription _beltPointsDescription, BeltObjectContact _objectContact)
        {
            /*
             *  First calculate which point on the circumference is closest to the contact point
             */

            //Points on the circumference spline to check
            Vector3[] _circumferencePoints = _beltPointsDescription.CircumferencePoints;

            //Stored values on closest circumference point
            float _leastDifferenceInDistance = -1f;
            int _closestCircumferencePointIndex = 0;
            Vector3 _closestCircumferencePoint = Vector3.zero;
            //Check for each circumference point the distance to the contact point
            for (int _i = 0; _i < _circumferencePoints.Length; _i++)
            {
                int _currentIndex = _i;
                Vector3 _currentCircumferencePoint = _circumferencePoints[_i];

                //Calculate distance to circumference point
                float _distanceFromContactToCurrentCircumferencePoint = Vector3.Distance(_objectContact.ContactPoint, _currentCircumferencePoint);
                float _angle = Vector3.Angle(_objectContact.ContactNormal, _currentCircumferencePoint - _objectContact.ContactPoint);

                //if the calculated distance is less then previously found and lies in the plane of the collision surface, set the closest distance to the new calculated distance and store index
                if ((_leastDifferenceInDistance > _distanceFromContactToCurrentCircumferencePoint || _leastDifferenceInDistance == -1f) && (Mathf.Abs(_angle - 90f) < this.surfacePlaneAngleMargin))
                {
                    _leastDifferenceInDistance = _distanceFromContactToCurrentCircumferencePoint;

                    _closestCircumferencePointIndex = _currentIndex;
                    _closestCircumferencePoint = _currentCircumferencePoint;
                }
            }
                       
            /*
             *  Next, calculate which surface point in the spline belonging to the closest circumference point, is closest to the contact point.
             */

            //Take the points for the spline that belongs to the closest point on the circumference
            Vector3[] _surfacePoints = beltPointsDescription.SurfacePoints[_closestCircumferencePointIndex].Points;

            //Stored values on closest surface point
            int _closestSurfaceIndex = 0;
            float _closestDistanceFromSurfacePoint = -1f;

            //Check for each surface point the distance to the contact point
            for (int _i = 0; _i < _surfacePoints.Length; _i++)
            {
                float _distanceFromContactToCurrentSurfacePoint = Vector3.Distance(_objectContact.ContactPoint, _surfacePoints[_i]);

                //if the calculated distance is less then previously found, set the closest distance to the new calculated distance and store index
                if (_closestDistanceFromSurfacePoint > _distanceFromContactToCurrentSurfacePoint || _closestDistanceFromSurfacePoint == -1f)
                {
                    _closestSurfaceIndex = _i;
                    _closestDistanceFromSurfacePoint = _distanceFromContactToCurrentSurfacePoint;
                }
            }


            /*
             *  Calculate if the closest point is in front or behind the contact point
             */

            //Calculate the previous circumference point
            Vector3 _previousCircumferencePoint = _beltPointsDescription.CircumferencePoints[PreSpectiveMath.LimitMinMax(_closestCircumferencePointIndex - 1, 0, _beltPointsDescription.CircumferencePoints.Length - 1)];

            //If the distance from previous surface point to contact is less that previous surface point to closest point, closest point is in front of contact point. Otherwise behind contact point. 
            int _indexModifier = Vector3.Distance(_previousCircumferencePoint, _objectContact.ContactPoint) < Vector3.Distance(_previousCircumferencePoint, _closestCircumferencePoint) ? -1 : 1;

            //Take the lower index value to check the point behind the contact point
            int _index = Mathf.Min(_closestCircumferencePointIndex, _closestCircumferencePointIndex + _indexModifier);
            Vector3 _surfacePointInBack = _beltPointsDescription.SurfacePoints[PreSpectiveMath.LimitMinMax(_index, 0, _beltPointsDescription.CircumferencePoints.Length - 1)].Points[_closestSurfaceIndex];

            //Take the higher index value to check the point in front of the contact point
            _index = Mathf.Max(_closestCircumferencePointIndex, _closestCircumferencePointIndex + _indexModifier);
            Vector3 _surfacePointInFront = _beltPointsDescription.SurfacePoints[PreSpectiveMath.LimitMinMax(_index, 0, _beltPointsDescription.CircumferencePoints.Length - 1)].Points[_closestSurfaceIndex];

            //Return the closest surface point behind and in front of the contact point
            return new KeyValuePair<Vector3, Vector3>(_surfacePointInBack, _surfacePointInFront);
        }
        #endregion

        /// <summary>
        /// A buffer for all circumference and surface points in the belt
        /// </summary>
        private struct BeltPointsDescription
        {
            public Vector3[] CircumferencePoints;
            public BeltSurfacePointsDescription[] SurfacePoints;            
        }

        /// <summary>
        /// A buffer for all points on a surface spline
        /// </summary>
        private struct BeltSurfacePointsDescription
        {
            public Vector3[] Points;
        }

        /// <summary>
        /// Describes an object contact on the belt
        /// </summary>
        private struct BeltObjectContact
        {
            public Rigidbody ObjectRigidbody;
            public Vector3 ContactPoint;
            public Vector3 ContactNormal;
        }

        /// <summary>
        /// Describes an intended force to be applied to a specific rigidbody
        /// </summary>
        private struct BeltForceIntent
        {
            public Rigidbody Rigidbody;
            public Vector3 ForceDirection;
            public Vector3 PointOfForceApplication;
        }
    }
}
