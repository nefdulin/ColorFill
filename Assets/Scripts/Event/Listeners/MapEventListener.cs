using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    [System.Serializable]
    public class MapEvent : UnityEvent<Map> { }

    [ExecuteAlways]
    public class MapEventListener : MonoBehaviour
    {
        [SerializeField]
        private MapEventChannel _channel = default;

        public MapEvent OnEventRaised;

        public void Response(Map m) => OnEventRaised?.Invoke(m);

        void OnEnable()
        {
            if (_channel == null) return;

            _channel.OnEventRaised += Response;
        }

        void OnDisable()
        {
            if (_channel == null) return;

            _channel.OnEventRaised -= Response;
        }
    } 
}
