using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    public class BlockTrailState : BlockState
    {
        public BlockTrailState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Trail;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.Collider.isTrigger = false;
            Block.MovementCounter = 0;
            Block.MovementCount = 0;
            Block.MovementDirection = MovementDirection.Stop;
            Block.TargetPosition = Block.Position;
            
            Block.Map.TrailBlocks.Add(Block);
            
            Block.transform.localScale = Block.Map.Settings.TrailBlockScale; 

            yield break;
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    } 
}
