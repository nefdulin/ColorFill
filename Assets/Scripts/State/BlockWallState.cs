using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    public class BlockWallState : BlockState
    {
        public BlockWallState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Wall;
            Block.transform.localScale = Vector3.one;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.Collider.isTrigger = false;
            
            yield break;
        }
    } 
}
