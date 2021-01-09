using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    [System.Serializable]
    public class BlockState : IState
    {
        [System.Serializable]
        public struct CollisionAction
        {
            public BlockStateType type;
            public Action<Block> action;

            public CollisionAction(BlockStateType type, Action<Block> action)
            {
                this.type = type;
                this.action = action;   
            }
        }

        [SerializeField] 
        public Block Block;

        public List<CollisionAction> CollisionRegistry = new List<CollisionAction>();

        public BlockState(Block b) => Block = b;
        
        public virtual void RegisterForCollision(BlockStateType type, Action<Block> action)
            => CollisionRegistry.Add(new CollisionAction(type, action));
        
        public virtual void OnCollision(Block other)
        {
            for (int i = 0; i < CollisionRegistry.Count; i++)
            {
                if (CollisionRegistry[i].type == other.StateType)
                {
                    CollisionRegistry[i].action(other);
                }
            }
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator Tick()
        {
            yield break;
        }

        public virtual IEnumerator Exit()
        {
            yield break;
        }
    } 
}
