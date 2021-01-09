using System.Collections;
using ColorFill.Events;
using UnityEngine;

namespace ColorFill 
{
    public class PlayerController : MonoBehaviour
    {
        public MovementDirection MovementDirection;
        
        public MovementDirectionEventChannel OnPlayerMovementInput;

        private float _lastMouseXPosition;
        private float _lastMouseYPosition;

        private int _movementThreshold = 600;

        private bool _canTakeInput = true;
        private bool _canProcessInput = false;

        private void NewInput(MovementDirection input)
        {
            MovementDirection = input;

            _lastMouseXPosition = Input.mousePosition.x;
            _lastMouseYPosition = Input.mousePosition.y;

            OnPlayerMovementInput.Raise(MovementDirection);

            _canTakeInput = false;
            StartCoroutine(InputDelay());
        }

        public void SetCanProcessInput(bool value) => _canProcessInput = value;
        
        private IEnumerator InputDelay()
        {
            yield return new WaitForSeconds(0.1f);
            _canTakeInput = true;
        }

        private bool CanTakeInput()
        {
            return _canTakeInput && _canProcessInput;
        }
        
        void Update()
        {
            // Not the most prettiest stuff
            if (CanTakeInput())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _lastMouseXPosition = Input.mousePosition.x;
                    _lastMouseYPosition = Input.mousePosition.y;
                }

                if (Input.GetMouseButton(0))
                {
                    float currentMouseXPosition = Input.mousePosition.x;
                    float currentMouseYPosition = Input.mousePosition.y;

                    float xDifference = currentMouseXPosition - _lastMouseXPosition;
                    float yDifference = currentMouseYPosition - _lastMouseYPosition;

                    if (Mathf.Abs(xDifference) > Mathf.Abs(yDifference))
                    {
                        if (Mathf.Abs(xDifference) > _movementThreshold)
                        {
                            if (xDifference > 0)
                                NewInput(MovementDirection.Right);
                            else
                                NewInput(MovementDirection.Left);

                        }
                    }
                    else
                    {
                        if (Mathf.Abs(yDifference) > _movementThreshold)
                        {
                            if (yDifference > 0)
                                NewInput(MovementDirection.Up);
                            else
                                NewInput(MovementDirection.Down);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    MovementDirection = MovementDirection.Up;
                    
                    OnPlayerMovementInput.Raise(MovementDirection);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    MovementDirection = MovementDirection.Down;
                    
                    OnPlayerMovementInput.Raise(MovementDirection);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    MovementDirection = MovementDirection.Right;

                    OnPlayerMovementInput.Raise(MovementDirection);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    MovementDirection = MovementDirection.Left;
                    
                    OnPlayerMovementInput.Raise(MovementDirection);
                }
            }
        }
    } 
}
