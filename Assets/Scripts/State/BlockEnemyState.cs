using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ColorFill.States
{
    public class BlockEnemyState : BlockState, IBlockMovableState
    {
        public BlockEnemyState(Block b,
            bool shouldMove,
            MovementDirection movementDirection,
            int movementCount,
            int movementCounter
        ) : base(b)
        {
            Block.ShouldMove = shouldMove;
            Block.MovementDirection = movementDirection;
            Block.MovementCount = movementCount;
            Block.MovementCounter = movementCounter;
        }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Enemy;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.SetupRigidbody();
            Block.Collider.isTrigger = true;
            Block.transform.localScale = Block.Map.Settings.EnemyBlockScale;
            
            RegisterForCollision(BlockStateType.Trail, TrailCollision);
            RegisterForCollision(BlockStateType.Completed, CompletedCollision);
            
            yield break;
        }

        private void CompletedCollision(Block completed)
        {
            Block.DestroyBlock();
        }

        private void TrailCollision(Block trail)
        {
            // Since we are triggering the game over state with OnPlayerDestroyed event
            // Let just invoke it here
            // Might wanna change it later
            Block.OnPlayerDestroyed.Raise();
        }

        public override IEnumerator Exit()
        {
            Block.Collider.isTrigger = false;
            Block.transform.localScale = Vector3.one;
            Block.ResetBlockPosition();
            
            yield break;
        }

        public override IEnumerator Tick()
        {
            yield return StartMovement();
        }

        public IEnumerator StartMovement()
        {
            Block.UpdateTargetPosition();
            yield return MoveBlock();
        }
        
        public IEnumerator MoveBlock()
        {
            float distance = 1.0f;

            if (Block.MovementCounter >= Block.MovementCount)
            {
                if (Block.MovementDirection == MovementDirection.Up || Block.MovementDirection == MovementDirection.Down)
                    Block.MovementDirection = Block.MovementDirection == MovementDirection.Up ? MovementDirection.Down : MovementDirection.Up;
                else if (Block.MovementDirection == MovementDirection.Right || Block.MovementDirection == MovementDirection.Left)
                    Block.MovementDirection = Block.MovementDirection == MovementDirection.Right ? MovementDirection.Left : MovementDirection.Right;

                Block.MovementCounter = 0;
                Block.UpdateTargetPosition();
            }

            // TODO: Optimize
            while (distance > 0.1f)
            {
                if (Block.MovementDirection == MovementDirection.Up || Block.MovementDirection == MovementDirection.Down)
                    distance = Mathf.Abs(Block.transform.localPosition.z - Block.TargetPosition.z);
                else if (Block.MovementDirection == MovementDirection.Right || Block.MovementDirection == MovementDirection.Left)
                    distance = Mathf.Abs(Block.transform.localPosition.x - Block.TargetPosition.x);

                Block.transform.localPosition = Block.transform.localPosition + (Block.MovementMap[Block.MovementDirection] * Block.Map.Settings.EnemyMovementSpeed * Time.deltaTime);
                yield return null;
            }

            BlockStateType stateType = Block.StateType;

            Block nextBlock = Block.Map.MapData[(int)Block.TargetPosition.x, (int)Block.TargetPosition.y, (int)Block.TargetPosition.z];
            yield return new WaitWhile(() => nextBlock.StateType == BlockStateType.Enemy);

            Block.MovementCounter++;
            //nextBlock.SetEnemyData(Block.ShouldMove, Block.MovementDirection, Block.MovementCount, Block.MovementCounter);
            nextBlock.ChangeBlockState(new BlockEnemyState(nextBlock, Block.ShouldMove, Block.MovementDirection, Block.MovementCount, Block.MovementCounter));
            
            Block.ChangeBlockState(new BlockEmptyState(Block));

            Block.transform.localPosition = Block.Position;
        }
    } 
}
