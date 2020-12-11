using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class RotationModerator : MonoBehaviour
    {
        public RotatingMechanicalPart bucketPivot;

        [SerializeField] private Vector3 bucketPivotMinBound;
        [SerializeField] private Vector3 bucketPivotMaxBound;
        private Vector3 bucketPivotOrigin;
        private Vector3 newbucketPivotOrigin;
        [Range(0,1)]
        [SerializeField] private float bucketPivotScale;
        
        public RotatingMechanicalPart loaderFrame;

        [SerializeField] private Vector3 loaderFrameMinBound;
        [SerializeField] private Vector3 loaderFrameMaxBound;
        private Vector3 loaderFrameOrigin;
        private Vector3 newloaderFrameOrigin;
        [Range(0,1)]
        [SerializeField] private float loaderFrameScale;
        
        void Start()
        {
            // Angles from origin
            bucketPivotMinBound = bucketPivot.Min - bucketPivot.Default;
            bucketPivotMaxBound = bucketPivot.Max - bucketPivot.Default;
            loaderFrameMinBound = loaderFrame.Min - loaderFrame.Default;
            loaderFrameMaxBound = loaderFrame.Max - loaderFrame.Default;

            // Load origin value from origin.json and update RotatingMechanicalPart.cs
            if (System.IO.File.Exists(Application.dataPath + "/Saved/origin.json"))
            {
                string saveString = System.IO.File.ReadAllText(Application.dataPath + "/Saved/origin.json");
                SaveAngle loadedData = JsonUtility.FromJson<SaveAngle>(saveString);
                bucketPivot.Default = loadedData.savedBucketPivotOrigin;
                loaderFrame.Default = loadedData.savedLoaderFrameOrigin;
            }
            loaderFrame.ResetToDefault();
            bucketPivot.ResetToDefault();
        }
        
        void Update()
        {
            // Take origin and scale from RotatingMechanicalPart.cs every frame
            bucketPivotOrigin = bucketPivot.Default;
            bucketPivotScale = bucketPivot.MovementInput;
            if (newbucketPivotOrigin != bucketPivotOrigin) {
                newbucketPivotOrigin = bucketPivotOrigin;
                Debug.Log("Bucket Pivot Origin:  " + bucketPivotOrigin);

                // Min and max angles change with origin
                bucketPivot.Min = bucketPivotOrigin + bucketPivotMinBound;
                bucketPivot.Max = bucketPivotOrigin + bucketPivotMaxBound;
                bucketPivot.RecalculateMovementInput(bucketPivotOrigin);
            
                // Save origin angle to origin.json
                SaveAngle saveAngle = new SaveAngle { savedBucketPivotOrigin = bucketPivotOrigin };
                string json = JsonUtility.ToJson(saveAngle);
                System.IO.File.WriteAllText(Application.dataPath + "/Saved/origin.json", json);
            }
            
            loaderFrameOrigin = loaderFrame.Default;
            loaderFrameScale = loaderFrame.MovementInput;
            if (newloaderFrameOrigin != loaderFrameOrigin) {
                newloaderFrameOrigin = loaderFrameOrigin;
                Debug.Log("Loader Frame Origin:  " + loaderFrameOrigin);
            
                // Min and max angles change with origin
                loaderFrame.Min = loaderFrameOrigin + loaderFrameMinBound;
                loaderFrame.Max = loaderFrameOrigin + loaderFrameMaxBound;
                loaderFrame.RecalculateMovementInput(loaderFrameOrigin);
            
                // Save origin angle to origin.json
                SaveAngle saveAngle = new SaveAngle { savedLoaderFrameOrigin = loaderFrameOrigin };
                string json = JsonUtility.ToJson(saveAngle);
                System.IO.File.WriteAllText(Application.dataPath + "/Saved/origin.json", json);
            }
        }

        private class SaveAngle {
            public Vector3 savedBucketPivotOrigin;
            public Vector3 savedLoaderFrameOrigin;
        }

        //Link buttons to buttons on RotatingMechanicalPart.cs
        public void BucketPivotSetCurrentAsOrigin() {
            bucketPivot.SetCurrentAsDefault();
        }

        public void BucketPivotBackToOrigin() {
            bucketPivot.ResetToDefault();
        }

        public void LoaderFrameSetCurrentAsOrigin() {
            loaderFrame.SetCurrentAsDefault();
        }

        public void LoaderFrameBackToOrigin() {
            loaderFrame.ResetToDefault();
        }
    }
}
