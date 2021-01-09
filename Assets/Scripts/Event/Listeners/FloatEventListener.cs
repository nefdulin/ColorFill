using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    public class FloatEventListener : MonoBehaviour
    {
        [SerializeField]
        private FloatEventChannel _channel = default;

        public FloatEvent OnEventRaised;

        public void Response(float f) => OnEventRaised?.Invoke(f);

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