using System.Reflection;
using u040.prespective.prepair.inspector;
using UnityEngine;

namespace u040.prespective.referenceobjects.sensors.colorsensor
{
    public class ContrastSensor : ColorDetector, IControlPanel
    {
        public float MatchFactorBase { get; private set; } = 0f;
        public float MatchFactorBackground { get; private set; } = 0f;

        public Color BaseColor
        {
            get { return this.ReferenceColor; }
            set { this.ReferenceColor = value; }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private Color backgroundColor = Color.black;
        public Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set
            {
                if (this.backgroundColor != value)
                {
                    this.backgroundColor = value;
                    compareColor(DetectedColor);
                }
            }
        }

        protected override void compareColor(Color _color)
        {
            float _differenceR, _differenceG, _differenceB;
            float _contributionR, _contributionG, _contributionB;

            //Calculate Base match
            //Calculate the differences between the R, G, and B values
            _differenceR = Mathf.Abs(BaseColor.r - DetectedColor.r);
            _differenceG = Mathf.Abs(BaseColor.g - DetectedColor.g);
            _differenceB = Mathf.Abs(BaseColor.b - DetectedColor.b);

            //Each value has a potential of contributing a third of the maximum match factor
            _contributionR = (1f / 3f) * (1f - _differenceR);
            _contributionG = (1f / 3f) * (1f - _differenceG);
            _contributionB = (1f / 3f) * (1f - _differenceB);

            //Add all contributions together for the resulting match factor base
            MatchFactorBase = _contributionR + _contributionG + _contributionB;

            //Calculate Background match
            //Calculate the differences between the R, G, and B values
            _differenceR = Mathf.Abs(BackgroundColor.r - DetectedColor.r);
            _differenceG = Mathf.Abs(BackgroundColor.g - DetectedColor.g);
            _differenceB = Mathf.Abs(BackgroundColor.b - DetectedColor.b);

            //Each value has a potential of contributing a third of the maximum match factor
            _contributionR = (1f / 3f) * (1f - _differenceR);
            _contributionG = (1f / 3f) * (1f - _differenceG);
            _contributionB = (1f / 3f) * (1f - _differenceB);

            //Add all contributions together for the resulting match factor background
            MatchFactorBackground = _contributionR + _contributionG + _contributionB;

            //Flagged depends on which color has the most match
            Flagged = MatchFactorBase >= MatchFactorBackground;
        }
    }
}