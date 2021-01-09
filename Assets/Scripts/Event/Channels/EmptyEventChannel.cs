using System;
using UnityEngine;

namespace ColorFill.Events
{
    [CreateAssetMenu(menuName = "Events/Empty Event Channel")]
    public class EmptyEventChannel : BaseEventChannel
    {
        public event Action OnEventRaised;

        public void Raise() => OnEventRaised?.Invoke();
    } 
}
