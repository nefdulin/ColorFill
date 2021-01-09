using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    public class BlockObstacleState : BlockState
    {
        public BlockObstacleState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Obstacle;
            Block.transform.localScale = Vector3.one;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.Collider.isTrigger = false;

            yield break;
        }
    } 
}
