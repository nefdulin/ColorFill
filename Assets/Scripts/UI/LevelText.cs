using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorFill
{
    public class LevelText : MonoBehaviour
    {
        private Text _text;

        void Awake() => _text = GetComponent<Text>();

        public void UpdateLevelNoText(int level) => _text.text = level.ToString();
    } 
}
