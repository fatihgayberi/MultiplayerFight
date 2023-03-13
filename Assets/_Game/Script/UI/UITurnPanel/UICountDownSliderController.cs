using UnityEngine;
using UnityEngine.UI;

namespace Wonnasmith
{
    public class UICountDownSliderController : UIWonnaSliderBase
    {
        private void OnEnable()
        {
            TourController.TourCountDownChange += OnTourCountDownChange;
        }
        private void OnDisable()
        {
            TourController.TourCountDownChange -= OnTourCountDownChange;
        }


        private void OnTourCountDownChange(float currentCountDownSecond, float targetCountDownSecond)
        {
            SliderChangeValue(currentCountDownSecond, 0, targetCountDownSecond);
        }
    }
}