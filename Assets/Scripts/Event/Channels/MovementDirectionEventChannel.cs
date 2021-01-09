using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.Events
{
    [CreateAssetMenu(menuName = "Events/Movement Direction Event Channel")]
    public class MovementDirectionEventChannel : BaseEventChannel
    {
        public Action<MovementDirection> OnEventRaised;

        public void Raise(MovementDirection md) => OnEventRaised?.Invoke(md);
    } 
}
