using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill
{
    [CreateAssetMenu(menuName = "Map Settings")]
    public class MapSettings : ScriptableObject
    {
        public List<BlockTypeMaterial> BlockMaterials;

        public float PlayerMovementSpeed = 8.0f;
        public float EnemyMovementSpeed = 1.0f;
        
        public Vector3 PlayerBlockScale = new Vector3(0.98f, 1.0f, 0.98f);
        public Vector3 EnemyBlockScale = new Vector3(0.95f, 1.0f, 0.95f);
        public Vector3 TrailBlockScale  = new Vector3(1.0f, 0.7f, 1.0f);
        public Vector3 CompletedBlockScale = new Vector3(1.0f, 0.9f, 1.0f);
        public Vector3 CrystalBlockScale = new Vector3(0.9f, 0.9f, 0.9f);
    }       
}
