using System;
using System.Collections.Generic;
using ColorFill.Events;
using ColorFill.States;
using UnityEngine;

namespace ColorFill
{
    [Serializable]
    public enum BlockStateType { Player, Enemy, Wall, Trail, Completed, Obstacle, Crystal, Empty }
    
    [Serializable]
    public enum MovementDirection {  Up, Down, Right, Left, Stop }

    [Serializable]
    public struct BlockTypeMaterial
    {
        public BlockStateType StateType;
        public Material Material;
    }

    [Serializable]
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Block : MonoBehaviour
    {
        public static readonly Dictionary<MovementDirection, Vector3> MovementMap = new Dictionary<MovementDirection, Vector3>()
        {
            {MovementDirection.Up, Vector3.forward},
            {MovementDirection.Down, Vector3.back},
            {MovementDirection.Right, Vector3.right},
            {MovementDirection.Left, Vector3.left},
            {MovementDirection.Stop, Vector3.zero}
        };

        public BlockEventChannel OnBlockDestroy;
        public BlockEventChannel OnPlayerBlockChanged;
        public BlockEventChannel OnPlayerStop;
        public EmptyEventChannel OnPlayerDestroyed;
        public EmptyEventChannel OnPlayerEnterCompleted;
        public BlockStateChangeEventChannel OnBlockStateChanged;
        public EmptyEventChannel OnCrystalCollected;

        public BlockStateMachine BlockStateMachine;

        [SerializeField]
        public Map Map;
        
        [SerializeField]
        public Vector3 Position;

        [SerializeField]
        public MovementDirection MovementDirection = MovementDirection.Stop;
        
        [SerializeField]
        public BlockStateType StateType;

        public bool IsMarked = false;

        public bool IsCompleted;
        
        public bool ShouldMove;
        
        public int MovementCount;
        
        public int MovementCounter = 0;
        
        public bool IsMoving = false;

        public Vector3 TargetPosition;

        public Rigidbody Rigidbody
        {
            get
            {
                if (_rigidbody == null)
                    _rigidbody = GetComponent<Rigidbody>();

                return _rigidbody;
            }
        }

        public Renderer Renderer
        {
            get
            {
                if (_renderer == null)
                    _renderer = GetComponent<Renderer>();

                return _renderer;
            }
        }

        public Collider Collider
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<Collider>();

                return _collider;
            }
        }

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private Renderer _renderer;

        private Collider _collider;
        
        void Start()
        {
            _collider  = GetComponent<Collider>();
            _renderer  = GetComponent<Renderer>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void UpdateTargetPosition()
        {
            TargetPosition = Position + MovementMap[MovementDirection];
        }

        public void ChangeBlockState(BlockState newState)
        {
            BlockStateType oldStateType = StateType;
            BlockStateMachine.SetState(newState);
            OnBlockStateChanged.Raise(this, oldStateType, StateType);
        }

        public Material GetBlockMaterial()
        {
            var blockTypeMaterials = Map.Settings.BlockMaterials;

            for (int i = 0; i < blockTypeMaterials.Count; i++)
                if (blockTypeMaterials[i].StateType == StateType)
                    return blockTypeMaterials[i].Material;

            return blockTypeMaterials[0].Material;
        }

        private bool IsMoveable()
        {
            return BlockStateMachine.CurrentState is IBlockMovableState;
            //BlockStateType.Player || StateType == BlockStateType.Enemy
        }

        public void ResetBlockPosition()
        {
            transform.localPosition = Position;
        }

        public void DestroyBlock()
        {
            OnBlockDestroy.Raise(this);

            ChangeBlockState(new BlockEmptyState(this));
        }
        
        public void SetupRigidbody()
        {
            Rigidbody.drag = 0;
            Rigidbody.angularDrag = 0;
            Rigidbody.useGravity = false;
        }

        void HandleCollision(Block b1)
        {
            BlockStateMachine.CurrentState.OnCollision(b1);
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (IsMoveable())
            {
                Block b = other.gameObject.GetComponent<Block>();

                if (b != null)
                    HandleCollision(b);
            }
        }
    }
}