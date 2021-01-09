using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    [System.Serializable]
    public class BlockEmptyState : BlockState
    {
        public BlockEmptyState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Empty;
            Block.Renderer.enabled = false;
            Block.Collider.isTrigger = false;
            Block.MovementCount = 0;
            Block.MovementCounter = 0;
            Block.MovementDirection = MovementDirection.Stop;
            Block.TargetPosition = Block.Position;

            Block.SetupRigidbody();
            Block.Rigidbody.isKinematic = true;

            yield break;
        }

        public override IEnumerator Exit()
        {
            Block.Renderer.enabled = true;
            
            yield break;
        }
    } 
}
