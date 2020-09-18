using System;
using Godot;

namespace TaisGodot.Scripts
{
    public class LimitSlider : Panel
    {
        public LimitSlider()
        {
        }

        public double Value { get => slider.Value; set => slider.Value = value; }

        public float MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                if (_LimitMinValue != null && _LimitMinValue.Value > value)
                {
                    _LimitMinValue = value;
                }

                _MinValue = value;
                UpdateSlider();
            }
        }

        public float MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                if (_LimitMaxValue != null && _LimitMaxValue.Value < value)
                {
                    _LimitMaxValue = value;
                }

                _MaxValue = value;
                UpdateSlider();
            }
        }

        public float LimitMinValue
        {
            get
            {
                return _LimitMinValue != null? _LimitMinValue.Value : MinValue;
            }
            set
            {
                if(_LimitMinValue != null && _LimitMinValue.Value == value)
                {
                    return;
                }

                if(value < MinValue)
                {
                    throw new Exception($"LimitMinValue:{value} can not less than MinValue:{MinValue} in LimitSlider !");
                }
                if (value > LimitMaxValue)
                {
                    throw new Exception($"LimitMinValue:{value} can not bigger than LimitMaxValue:{LimitMaxValue} in LimitSlider !");
                }

                _LimitMinValue = value;
                UpdateSlider();
            }
        }

        public float LimitMaxValue
        {
            get
            {
                return _LimitMaxValue != null ? _LimitMaxValue.Value : MaxValue;
            }
            set
            {
                if (_LimitMaxValue != null && _LimitMaxValue.Value == value)
                {
                    return;
                }

                if (value > MaxValue)
                {
                    throw new Exception($"LimitMaxValue:{value} can not bigger than MaxValue:{MaxValue} in LimitSlider !");
                }
                if (value < LimitMinValue)
                {
                    throw new Exception($"LimitMaxValue:{value} can not less than LimitMinValue:{LimitMinValue} in LimitSlider !");
                }

                _LimitMaxValue = value;
                UpdateSlider();
            }
        }

        private void UpdateSlider()
        {
            var lenPercent = (LimitMaxValue - LimitMinValue) / (MaxValue - MinValue);
            var startPercent = (LimitMinValue - MinValue) / (MaxValue - MinValue);

            slider.RectSize = new Vector2(this.RectSize.x * lenPercent, slider.RectSize.y);
            slider.RectPosition = new Vector2(slider.RectPosition.x * startPercent, slider.RectPosition.y);

        }

        private float _MinValue;
        private float _MaxValue;

        private float? _LimitMinValue;
        private float? _LimitMaxValue;

        private Slider slider;
    }
}
