using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    [System.Serializable]
    public class BlockEvent : UnityEvent<Block> { }
    
    public class BlockEventListener : MonoBehaviour
    {
        [SerializeField] 
        private BlockEventChannel _channel = default;

        public BlockEvent OnEventRaised;

        public void Response(Block b) => OnEventRaised?.Invoke(b);
        
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
