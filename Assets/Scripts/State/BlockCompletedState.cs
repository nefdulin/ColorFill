using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    [System.Serializable]
    public class BlockCompletedState : BlockState
    {
        public BlockCompletedState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Completed;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.transform.localScale = Block.Map.Settings.CompletedBlockScale;
            Block.Rigidbody.isKinematic = true;
            Block.IsMarked = true;
            Block.IsCompleted = true;
            // asdasd dsfds fdgdfg
            yield break;; 
        }
    } 
}
