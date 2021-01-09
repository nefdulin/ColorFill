using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ColorFill.Events;
using TMPro;
using UnityEngine;

namespace ColorFill
{
    [ExecuteAlways]
    public class MapGenerator : MonoBehaviour
    {
        public delegate void MapInitializationNotify(Map m);

        public event MapInitializationNotify MapInit;

        private static MapGenerator _mapGenerator;

        public static MapGenerator Instance => _mapGenerator;
        
        public bool Initialized { get; private set; } = false;

        public MapEventChannel OnNewMapCreated;

        public GameObject MapPrefab;

        public Material BaseMaterial;

        private Map _lastCreatedMap;
        
        void Awake()
        {
            if (Instance == null)
                _mapGenerator = this;
        }

        void Update()
        {
            if (Instance == null)
                _mapGenerator = this;
        }

        public void CreateMap(int width, int height, int depth, int _levelNumber)
        {
            GameObject mapGameObject = Instantiate(MapPrefab, Vector3.zero, Quaternion.identity, transform);
            mapGameObject.name = $"Map {_levelNumber}";
            mapGameObject.transform.localPosition = Vector3.zero;
            _lastCreatedMap = mapGameObject.GetComponent<Map>();
            
            if (_lastCreatedMap.InitializeMap(width, height, depth, BaseMaterial, _levelNumber))
                OnMapInitialization(_lastCreatedMap);
            
            OnNewMapCreated.Raise(_lastCreatedMap);
        }

        public void OnMapInitialization(Map map)
        {
            MapInit?.Invoke(map);
        }
    }

}