using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.Events
{
    [ExecuteAlways]
    [CreateAssetMenu(menuName = "Events/Map Event Channel")]
    public class MapEventChannel : BaseEventChannel
    {
        public Action<Map> OnEventRaised;

        public void Raise(Map m) => OnEventRaised?.Invoke(m);
    }
}