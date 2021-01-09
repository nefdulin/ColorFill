using System.Collections;
using System.Collections.Generic;
using ColorFill;
using ColorFill.Events;
using ColorFill.States;
using UnityEngine;
using UnityEngine.Events;

namespace ColorFill.Events
{
    [System.Serializable]
    public class BlockStateChangeEvent : UnityEvent<Block, BlockStateType, BlockStateType> { }

    public class BlockStateChangeEventListener : MonoBehaviour
    {
        [SerializeField]
        private BlockStateChangeEventChannel _channel = default;

        public BlockStateChangeEvent OnEventRaised;

        public void Response(Block b,  BlockStateType oldStateType, BlockStateType newStateType) 
            => OnEventRaised?.Invoke(b, oldStateType, newStateType);

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
