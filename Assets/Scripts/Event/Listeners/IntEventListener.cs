using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    public class IntEventListener : MonoBehaviour
    {
        [SerializeField]
        private IntEventChannel _channel = default;

        public IntEvent OnEventRaised;

        public void Response(int i) => OnEventRaised?.Invoke(i);

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