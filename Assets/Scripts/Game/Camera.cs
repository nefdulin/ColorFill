using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill
{
    public class Camera : MonoBehaviour
    {
        public float CameraMovementSpeed = 4.0f;
        
        public void ChangePositionBasedOnNewMap(Map map)
        {
            StartCoroutine(Move(map.CameraPosition));
        }
        
        private IEnumerator Move(Vector3 position)
        {
            float distance = Vector3.Distance(transform.position, position);
            while (distance > 1.0f)
            {
                transform.position = Vector3.Lerp(transform.position, position, 0.6f * Time.deltaTime * CameraMovementSpeed);
                yield return null;
            }
            
            Debug.Log("Camera at new position");
        }
    }

}