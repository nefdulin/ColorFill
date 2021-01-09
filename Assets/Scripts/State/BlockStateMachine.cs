using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    [System.Serializable]
    public class BlockStateMachine : MonoBehaviour
    {
        [SerializeField]
        public BlockState CurrentState => _currentState;

        [SerializeReference]
        private BlockState _currentState;

        private Coroutine _updateCoroutine;

        public void Tick()
        {
            if (_currentState != null)
            {
                if (_updateCoroutine != null) StopCoroutine(_updateCoroutine);
                
                _updateCoroutine = StartCoroutine(_currentState.Tick());
            }
        }
        
        void OnEnable()
        {
            Tick();
        }
        
        void OnDisable()
        {
            if (_updateCoroutine != null) StopCoroutine(_updateCoroutine);
        }
        
        public void SetState(BlockState newState)
        {
            if (_updateCoroutine != null) StopCoroutine(_updateCoroutine);

            if (CurrentState != null)
                StartCoroutine(CurrentState.Exit());
            
            _currentState = newState;
            
            StartCoroutine(CurrentState.Start());
          
            if (Application.isPlaying)
                Tick();
        }
    } 
}
