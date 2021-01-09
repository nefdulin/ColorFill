using System.Collections;
using System.Collections.Generic;
using ColorFill.Events;
using UnityEngine;
using UnityEditor;

namespace ColorFill
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        private MapGenerator _mapGenerator;
        private int _mapWidth = 4;
        private int _mapHeight = 4;
        private int _mapDepth = 4;
        private int _levelNumber;

        public void OnEnable()
        {
            _mapGenerator = target as MapGenerator;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _mapWidth = EditorGUILayout.IntField("Width", _mapWidth);
            _mapHeight = EditorGUILayout.IntField("Height", _mapHeight);
            _mapDepth = EditorGUILayout.IntField("Depth", _mapDepth);
            _levelNumber = EditorGUILayout.IntField("Map Number: ", _levelNumber);

            if (GUILayout.Button(_mapGenerator.Initialized ? "Update" : "Initialize"))
            {
                _mapGenerator.CreateMap(_mapWidth, _mapHeight, _mapDepth, _levelNumber);
            }
        }
    } 
}
