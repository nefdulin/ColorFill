using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.Events
{
    [CreateAssetMenu(menuName = "Events/Float Event Channel")]
    public class FloatEventChannel : BaseEventChannel
    {
        public Action<float> OnEventRaised;

        public void Raise(float f) => OnEventRaised?.Invoke(f);
    } 
}
