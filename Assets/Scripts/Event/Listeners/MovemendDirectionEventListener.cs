using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    [System.Serializable]
    public class MovementDirectionEvent : UnityEvent<MovementDirection> { }

    
    public class MovemendDirectionEventListener : MonoBehaviour
    {
        public MovementDirectionEvent OnEventRaised;

        [SerializeField] 
        private MovementDirectionEventChannel _channel = default;

        public void Response(MovementDirection md) => OnEventRaised?.Invoke(md);

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
