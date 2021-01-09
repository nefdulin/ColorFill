using System;
using System.Collections;
using System.Collections.Generic;
using ColorFill.States;
using UnityEngine;

namespace ColorFill.Events  
{
    [CreateAssetMenu(menuName = "Events/Block State Change Event Channel")]
    public class BlockStateChangeEventChannel : BaseEventChannel
    {
        public Action<Block, BlockStateType, BlockStateType> OnEventRaised;

        public void Raise(Block b, BlockStateType oldState, BlockStateType newState) => OnEventRaised?.Invoke(b, oldState, newState);
    }

}