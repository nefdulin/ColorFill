using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ColorFill
{
    public class ProgressBar : MonoBehaviour
    {
        private Slider _slider;
     
        void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.value = 0;
        }

        void ResetProgressBar(Map newMap) => _slider.value = 0;

        public void UpdateProgressBar(float newValue) => _slider.value = newValue;
    }
}