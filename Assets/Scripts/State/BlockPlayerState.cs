using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    public class BlockPlayerState : BlockState, IBlockMovableState
    {
        public BlockPlayerState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Player;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.SetupRigidbody();
            Block.Collider.isTrigger = true;
            Block.transform.localScale = Block.Map.Settings.PlayerBlockScale;
            
            RegisterForCollision(BlockStateType.Wall, WallCollision);
            RegisterForCollision(BlockStateType.Enemy, EnemyCollision);
            RegisterForCollision(BlockStateType.Completed, CompletedCollision);
            RegisterForCollision(BlockStateType.Trail, TrailCollision);
            RegisterForCollision(BlockStateType.Obstacle, WallCollision);
            RegisterForCollision(BlockStateType.Crystal, CrystalCollision);

            Block.OnPlayerBlockChanged.Raise(Block);
            
            yield break;
        }

        private void CrystalCollision(Block crystal)
        {
            crystal.DestroyBlock();
        }

        private void TrailCollision(Block trail)
        {
            Block.DestroyBlock();
            Block.OnPlayerDestroyed.Raise();
        }

        private void CompletedCollision(Block completed)
        {
            Block.OnPlayerEnterCompleted.Raise();
        }

        private void EnemyCollision(Block enemy)
        {
            if (Block.IsCompleted)
                enemy.DestroyBlock();
            else
            {
                Block.DestroyBlock();
                Block.OnPlayerDestroyed.Raise();
            }
        }

        private void WallCollision(Block wall)
        {
            Block.ResetBlockPosition();
            Block.IsCompleted = true;
            
            Block.OnPlayerStop.Raise(Block);
        }

        public override IEnumerator Exit()
        {
            Block.transform.localScale = Vector3.one;
            Block.Collider.isTrigger = false;
            
            return base.Exit();
        }

        public override IEnumerator Tick()
        {
            if (Block.MovementDirection == MovementDirection.Stop) yield break;
            yield return StartMovement();
        }

        public IEnumerator StartMovement()
        {
            Block.UpdateTargetPosition();

            if (Block.IsMoving)
            {
                float distance = Vector3.Distance(Block.transform.position, Block.TargetPosition);

                if (distance > 0.5f)
                {
                    Block.transform.localPosition = Block.Position;
                }
                else
                {
                    BlockStateType stateType = Block.StateType;
                    Block.IsMoving = false;

                    Block nextBlock = Block.Map.MapData[(int)Block.TargetPosition.x, (int)Block.TargetPosition.y, (int)Block.TargetPosition.z];

                    nextBlock.ChangeBlockState(new BlockPlayerState(nextBlock));
                    Block.ChangeBlockState(new BlockEmptyState(Block));
                    
                    Block.transform.localPosition = Block.Position;
                    yield break;
                }
            }

            Block.IsMoving = true;
            yield return MoveBlock();
        }

        public IEnumerator MoveBlock()
        {
            float distance = 1.0f;

            // TODO: Optimize
            while (distance > 0.01f * 25.0f)
            {
                if (Block.MovementDirection == MovementDirection.Up || Block.MovementDirection == MovementDirection.Down)
                    distance = Mathf.Abs(Block.transform.localPosition.z - Block.TargetPosition.z);
                else if (Block.MovementDirection == MovementDirection.Right || Block.MovementDirection == MovementDirection.Left)
                    distance = Mathf.Abs(Block.transform.localPosition.x - Block.TargetPosition.x);

                Block.transform.localPosition = Block.transform.localPosition + (Block.MovementMap[Block.MovementDirection] * Block.Map.Settings.PlayerMovementSpeed * Time.deltaTime);
                yield return null;
            }

            BlockStateType stateType = Block.StateType;

            Block nextBlock = Block.Map.MapData[(int) Block.TargetPosition.x, (int) Block.TargetPosition.y, (int) Block.TargetPosition.z];

            nextBlock.MovementDirection = Block.MovementDirection;
            nextBlock.ChangeBlockState(new BlockPlayerState(nextBlock));
            nextBlock.UpdateTargetPosition();
            Block.ChangeBlockState(Block.IsCompleted ? (BlockState) new BlockCompletedState(Block) : (BlockState) new BlockTrailState(Block));
            
            Block.Map.FillMap(Block);

            Block.transform.localPosition = Block.Position;
        }
    } 
}
