#if UNITY_EDITOR || UNITY_EDITOR_BETA
using System.Reflection;
using UnityEngine;
using UnityEditor;
using u040.prespective.core;
using u040.prespective.utility.editor;
using u040.prespective.prepair.kinematics;

namespace u040.prespective.referenceobjects.materialhandling.beltsystem.editor
{
    /// <summary>
    /// @CLASS : BeltSystemEditor
    /// 
    /// @ABOUT : Editor script for BeltSystem
    /// 
    /// @AUTHOR: Pieter, Tymen, Mathijs (Unit040)
    /// 
    /// @VERSION: 09/07/2019 - V1.00 - Implemented first build which controls the making of loft mesh and implementing which velocity the belt has
    /// @VERSION: 23/01/2020 - V2.00 - Loft Mesh now uses its own inspector. Implemented velocity indicator as gizmo instead of cube.
    /// </summary>
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(BeltSystem))]
    public class BeltSystemEditor : PrespectiveEditor
    {
        private BeltSystem component;

        private const float directionGizmoArcAngle = 60f;
        private const float directionGizmoArcRadius = 1.5f;
        private const float directioGizmoConeSize = 0.25f;

        void OnEnable()
        {
            this.component = (BeltSystem)target;
        }

        public override void OnInspectorGUI()
        {
            component.WheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Wheel Joint", component.WheelJoint, typeof(WheelJoint), true);
            EditorGUILayout.LabelField("Velocity", component.Velocity.ToString());
            if (!Application.isPlaying)
            {
                component.InvertAxis = EditorGUILayout.Toggle("Invert Axis", component.InvertAxis);
            }
            else
            {
                EditorGUILayout.LabelField("Invert Axis", component.InvertAxis.ToString());
            }
            
            //component.ManageRotation = EditorGUILayout.Toggle("Manage Rotation", component.ManageRotation);
            if (component.ManageRotation) { EditorGUILayout.HelpBox("Manage Rotation has been enabled. This could lag the system drastically.", MessageType.Warning); }
            EditorGUILayout.Space();
            
            component.BufferedCircumferenceSplinePoint = EditorGUILayout.IntField("Buffered Circumference Points", component.BufferedCircumferenceSplinePoint);
            component.BufferedSurfaceSplinePoints = EditorGUILayout.IntField("Buffered Surface Points", component.BufferedSurfaceSplinePoints);

            EditorGUILayout.Space();

            component.LoftMesh.ShowInspector(component);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Gizmo Settings", EditorStyles.boldLabel);
            component.EnableVelocityGizmo = EditorGUILayout.Toggle("Enable Velocity Gizmo", component.EnableVelocityGizmo);
            if (component.EnableVelocityGizmo)
            {
                EditorGUI.indentLevel++;
                component.ShowGizmoWhenNotSelected = EditorGUILayout.Toggle("Show Gizmo when not selected", component.ShowGizmoWhenNotSelected);
                component.VelocityGizmoColor = EditorGUILayout.ColorField("Velocity Gizmo Color", component.VelocityGizmoColor);
                EditorGUI.indentLevel--;
            }
            base.OnInspectorGUI();
        }

        /// <summary>
        /// Called every tick in the editor; draws the wheeljoint in the editor - this part manages specific parts when the joint is not selected (only active when 'showInSceneViewWhenNotSelected' is set)
        /// </summary>
        /// <param name="_belt">the wheeljoint this sceneview gizmo is called on</param>
        /// <param name="gizmoType"></param>
        [ObfuscationAttribute(Exclude = true, StripAfterObfuscation = false)]
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
        private static void RenderCustomGizmo(BeltSystem _belt, GizmoType gizmoType)
        {
            Handles.BeginGUI();
            Handles.Label(Vector3.zero, "");

            if ( _belt.EnableVelocityGizmo && (_belt.ShowGizmoWhenNotSelected || Selection.Contains(_belt.gameObject)))
            {
                if (_belt.WheelJoint)
                {
                    Handles.color = _belt.VelocityGizmoColor;

                    //Draw arc
                    Vector3 _center = _belt.WheelJoint.transform.position;
                    Vector3 _normal = _belt.WheelJoint.GlobalAxisDirection;
                    Quaternion _arcRotation = Quaternion.AngleAxis((360f * _belt.WheelJoint.CurrentRevolutionPercentage) + directionGizmoArcAngle * -0.5f, _belt.WheelJoint.GlobalAxisDirection);
                    Vector3 _from = _arcRotation * _belt.WheelJoint.GlobalForwardDirection.normalized;
                    float _angle = directionGizmoArcAngle;
                    float _radius = _belt.WheelJoint.Radius * directionGizmoArcRadius;
                    Handles.DrawWireArc(_center, _normal, _from, _angle, _radius);

                    //Draw handle
                    float _size = _belt.WheelJoint.Radius * directioGizmoConeSize;
                    float _positionAngle = (360f * _belt.WheelJoint.CurrentRevolutionPercentage);
                    if (_belt.Velocity == 0f)
                    {
                        Quaternion _handleRotation = Quaternion.AngleAxis(_positionAngle, _belt.WheelJoint.GlobalAxisDirection);
                        Vector3 _position = _belt.WheelJoint.transform.position + _handleRotation * _belt.WheelJoint.GlobalForwardDirection.normalized * _belt.WheelJoint.Radius * directionGizmoArcRadius;
                        Handles.SphereHandleCap(0, _position, Quaternion.identity, _size, EventType.Repaint);
                    }
                    else
                    { 
                        //Determine rotation direction from velocity
                        bool _directionCW = _belt.Velocity >= 0f ? true : false;

                        //If axis is inverted, intert gizmo
                        if (_belt.InvertAxis) { _directionCW = !_directionCW; }

                        Vector3 _directionAxis = _belt.WheelJoint.GlobalAxisDirection * (_directionCW ? 1f : -1f);

                        float _directionModifier = _directionCW ? 1f : -1f;
                        _positionAngle += directionGizmoArcAngle * 0.5f * _directionModifier;
                        Quaternion _coneRotation = Quaternion.AngleAxis(_positionAngle, _belt.WheelJoint.GlobalAxisDirection);
                        Vector3 _position = _belt.WheelJoint.transform.position + _coneRotation * _belt.WheelJoint.GlobalForwardDirection.normalized * _belt.WheelJoint.Radius * directionGizmoArcRadius;
                        Quaternion _rotation = Quaternion.LookRotation(Quaternion.AngleAxis(90f, _directionAxis) * (_coneRotation * _belt.WheelJoint.GlobalForwardDirection.normalized), _directionAxis);
                        Handles.ConeHandleCap(0, _position, _rotation, _size, EventType.Repaint);
                    }
                }
            }
            Handles.EndGUI();
        }


    }
}
#endif