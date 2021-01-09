using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    public class EmptyEventListener : MonoBehaviour
    {
        [SerializeField] 
        private EmptyEventChannel _channel = default;
        
        public UnityEvent OnEventRaised;

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

        public void Response() => OnEventRaised?.Invoke();
    }
}
