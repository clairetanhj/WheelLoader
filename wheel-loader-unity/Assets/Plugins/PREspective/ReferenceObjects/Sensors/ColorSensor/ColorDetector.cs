using System.Reflection;
using UnityEngine;
using u040.prespective.prepair.components.sensors;
using u040.prespective.utility;
using u040.prespective.prepair.inspector;

namespace u040.prespective.referenceobjects.sensors.colorsensor
{
    public class ColorDetector : QuantitativeSensor, IControlPanel
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public ColorSensor ColorSensor;

        [SerializeField] [Obfuscation(Exclude = true)] private Color referenceColor = Color.red;
        /// <summary>
        /// The color which the ColorDetector is referencing colors to
        /// </summary>
        public Color ReferenceColor
        {
            get { return this.referenceColor; }
            set
            {
                if (this.referenceColor != value)
                {
                    this.referenceColor = value;
                }
            }
        }

        /// <summary>
        /// The Color value of the ColorSensor
        /// </summary>
        public Color DetectedColor
        {
            get
            {
                if (ColorSensor)
                {
                    return ColorSensor.OutputSignal;
                }
                else
                {
                    return Color.black;
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private float threshold = 0.75f;
        /// <summary>
        /// The minimum MatchFactor to detect a match
        /// </summary>
        public float Threshold
        {
            get { return this.threshold; }
            set
            {
                if (this.threshold != value)
                {
                    this.threshold = Mathf.Clamp(value, 0f, 1f);
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private float matchFactor = 0f;
        /// <summary>
        /// The factor of similarities between the reference color and the detected color
        /// </summary>
        public float MatchFactor
        {
            get { return this.matchFactor; }
            set
            {
                if (this.matchFactor != value)
                {
                    this.matchFactor = Mathf.Clamp(value, 0f, 1f);
                    this.Flagged = this.matchFactor >= Threshold;
                }
            }
        }

        private void Reset()
        {
            this.ColorSensor = this.RequireComponent<ColorSensor>(true);
        }

        private void Awake()
        {
            //If a color sensor has been assigned
            if (ColorSensor)
            {
                //Add a listener to its OnValueChanged event
                ColorSensor.OnValueChanged.AddListener(() => 
                { 
                    compareColor(ColorSensor.OutputSignal);
                });
            }

            //If no color sensor has been assigned
            else
            {
                Debug.LogWarning("No Color sensor has been assigned.");
            }
        }

        new private void Start()
        {
            base.Start();

            //If a color sensor has been assigned
            if (ColorSensor)
            {
                //Update the ColorDetector
                compareColor(ColorSensor.OutputSignal);
            }
        }

        /// <summary>
        /// Compare a color with the reference color for similarities
        /// </summary>
        /// <param name="_color"></param>
        protected virtual void compareColor(Color _color)
        {
            //Calculate the differences between the R, G, and B values
            float _differenceR = Mathf.Abs(ReferenceColor.r - DetectedColor.r);
            float _differenceG = Mathf.Abs(ReferenceColor.g - DetectedColor.g);
            float _differenceB = Mathf.Abs(ReferenceColor.b - DetectedColor.b);

            //Each value has a potential of contributing a third of the maximum match factor
            float _contributionR = (1f / 3f) * (1f - _differenceR);
            float _contributionG = (1f / 3f) * (1f - _differenceG);
            float _contributionB = (1f / 3f) * (1f - _differenceB);

            //Add all contributions together for the resulting match factor
            MatchFactor = _contributionR + _contributionG + _contributionB;
        }

        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            this.ShowQuantitativeSensorValuesInspector(false);
#endif
        }
    }
}