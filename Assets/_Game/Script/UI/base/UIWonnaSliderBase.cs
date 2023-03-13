using UnityEngine;
using UnityEngine.UI;

namespace Wonnasmith
{
    public abstract class UIWonnaSliderBase : MonoBehaviour
    {
        [SerializeField] protected Slider currentSlider;

        [SerializeField] protected Color32 sliderMaxValueColor;
        [SerializeField] protected Color32 sliderMinValueColor;

        protected bool _isTimeWithValueChange = false;

        protected float _timeWithTargetValue;
        protected float _timeWithSpeed;


        private void Update()
        {
            if (_isTimeWithValueChange)
            {
                SliderChangeValueWithTimeEditor();
            }
        }


        /// <summary>
        /// verilen degeri direkt atama yapar
        /// </summary>
        /// <param name="currentValue">simdiki slider degeri</param>
        /// <param name="maxValue">max slider degeri</param>
        /// <param name="minValue">min slider degeri</param>
        public void SliderChangeValue(float currentValue, float minValue, float maxValue)
        {
            if (currentSlider == null)
            {
                return;
            }

            _isTimeWithValueChange = false;

            if (currentSlider.maxValue != maxValue)
            {
                currentSlider.maxValue = maxValue;
            }

            if (currentSlider.minValue != minValue)
            {
                currentSlider.minValue = minValue;
            }

            currentValue = Mathf.Clamp(currentValue, minValue, maxValue);

            currentSlider.value = currentValue;
        }


        /// <summary>
        /// zamana bağlı olup degere göre çalışan slider
        /// </summary>
        /// <param name="targetValue"> sliderin ilerleyeceği değer </param>
        /// <param name="duration"> sliderın kaç saniyede gitmesi gerektigini belirler  </param>
        /// <param name="minValue"> sliderin min degeri </param>
        /// <param name="maxValue"> sliderin max degeri </param>
        public void SliderChangeValueWithTime(float targetValue, float duration, float minValue, float maxValue)
        {
            if (currentSlider == null)
            {
                return;
            }

            if (currentSlider.maxValue != maxValue)
            {
                currentSlider.maxValue = maxValue;
            }

            if (currentSlider.minValue != minValue)
            {
                currentSlider.minValue = minValue;
            }

            _timeWithTargetValue = targetValue;

            _timeWithSpeed = Mathf.Abs(currentSlider.value - _timeWithTargetValue) / duration;

            _isTimeWithValueChange = true;
        }


        /// <summary>
        /// zamana bağlı olup yüzdelik degere göre çalışan slider
        /// </summary>
        /// <param name="percent"> sliderın yüzde kaçına gitmesini belirler </param>
        /// <param name="duration"> sliderın kaç saniyede gitmesi gerektigini belirler  </param>
        public void SliderChangeValueWithTime(float percent, float duration)
        {
            if (currentSlider == null)
            {
                return;
            }

            _isTimeWithValueChange = true;

            _timeWithTargetValue = (currentSlider.maxValue - currentSlider.minValue) * percent;

            _timeWithSpeed = Mathf.Abs(currentSlider.value - _timeWithTargetValue) / duration;
        }


        private void SliderChangeValueWithTimeEditor()
        {
            if (currentSlider == null)
            {
                return;
            }

            float newValue = Mathf.MoveTowards(currentSlider.value, _timeWithTargetValue, _timeWithSpeed * Time.deltaTime);

            bool isGrowing;

            if (currentSlider.value < _timeWithTargetValue)
            {
                isGrowing = true;
            }
            else
            {
                isGrowing = false;
            }

            currentSlider.value = newValue;

            if (isGrowing)
            {
                if (currentSlider.value >= _timeWithTargetValue)
                {
                    currentSlider.value = _timeWithTargetValue;
                    _isTimeWithValueChange = false;
                }
            }
            else
            {
                if (currentSlider.value <= _timeWithTargetValue)
                {
                    currentSlider.value = _timeWithTargetValue;
                    _isTimeWithValueChange = false;
                }
            }
        }


        public void _SLIDER_TextChange_INT(TMPro.TMP_Text TMPtext)
        {
            if (currentSlider == null)
            {
                return;
            }

            if (TMPtext == null)
            {
                return;
            }

            TMPtext.text = ((int)currentSlider.value).ToString();
        }


        public void _SLIDER_FillImageColorChange(Image fillImage)
        {
            if (currentSlider == null)
            {
                return;
            }

            if (fillImage == null)
            {
                return;
            }

            float percent = currentSlider.value.FloatRemap(currentSlider.minValue, currentSlider.maxValue, 0f, 100f);

            fillImage.color = ColorExtensionMethods.ColorPercentRate(percent, sliderMinValueColor, sliderMaxValueColor);
        }
    }
}