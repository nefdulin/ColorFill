using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorFill
{
    public class CrystalText : MonoBehaviour
    {
        private Text _text;
        
        void Start()
        {
            _text = GetComponent<Text>();
            UpdateCrystalText();
        }

        public void UpdateCrystalText() => _text.text = $"Crystal: {PlayerStats.CrystalCount}";
    } 
}
