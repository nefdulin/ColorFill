using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.Events
{
    [CreateAssetMenu(menuName = "Events/Block Event Channel")]
    public class BlockEventChannel : BaseEventChannel
    {
        public event Action<Block> OnEventRaised;

        public void Raise(Block b) => OnEventRaised?.Invoke(b);
    } 
}
