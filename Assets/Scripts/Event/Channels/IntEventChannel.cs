using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.Events
{
    [CreateAssetMenu(menuName = "Events/Int Event Channel")]
    public class IntEventChannel : BaseEventChannel
    {
        public Action<int> OnEventRaised;

        public void Raise(int i) => OnEventRaised?.Invoke(i);
    } 
}
