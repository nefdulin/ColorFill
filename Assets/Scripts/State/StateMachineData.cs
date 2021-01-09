using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    [System.Serializable]
    public class StateMachineData : ScriptableObject
    {
        [SerializeReference]
        public IState CurrentState;
    } 
    
    // asdasd
}
