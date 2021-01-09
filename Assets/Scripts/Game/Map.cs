using System;
using System.Collections.Generic;
using ColorFill.Events;
using ColorFill.States;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace ColorFill
{
    [Serializable]
    public class Map : MonoBehaviour
    {
        public MapEventChannel OnMapCompleted;
        public MapEventChannel OnMapStart;

        public GameObject BlockPrefab;
        public MapSettings Settings;

        // Could have found a better way
        public Dictionary<BlockStateType, int> BlockCountMap = new Dictionary<BlockStateType, int>
        {
            {BlockStateType.Enemy, 0},
            {BlockStateType.Empty, 0},
            {BlockStateType.Trail, 0},
            {BlockStateType.Completed, 0},
            {BlockStateType.Wall, 0},
            {BlockStateType.Player, 0},
            {BlockStateType.Obstacle, 0},
            {BlockStateType.Crystal, 0}
        };

        
        [SerializeField] 
        public Array3D<Block> MapData;
        
        public float MapProgress
        {
            get
            {
                return (float) BlockCountMap[BlockStateType.Completed] / (float) _totalEmptyAndEnemyBlock;
            }
        }

        public List<Block> TrailBlocks = new List<Block>(32);

        // Could have found a better way I guess
        public ProgressBar ProgressBar;

        public Vector3 CameraPosition;

        public int MapNumber;
        
        public bool StartInitialization => _startInitialization;

        [SerializeField]
        public Block PlayerBlock => _playerBlock;

        public bool IsCompleted => _isCompleted;

        public bool Initialized => _initialized;

        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;
        [SerializeField]
        private int _depth;

        [SerializeField]
        private bool _initialized = false;
        
        [SerializeField]
        private Block _playerBlock;

        private bool _activeMap = false;

        private bool _isCompleted;

        private int _collectedCrystalCount = 0;

        private int _totalEmptyAndEnemyBlock;

        private bool _startInitialization;
        
        private GameObject _fakeBlock;
        
        void Start()
        {
            EnumerateOnBlocks((block) => {
                BlockCountMap[block.StateType]++;
            });;

            _totalEmptyAndEnemyBlock += BlockCountMap[BlockStateType.Enemy];
            _totalEmptyAndEnemyBlock += BlockCountMap[BlockStateType.Empty];

            _fakeBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);

            _fakeBlock.transform.parent = transform;
            _fakeBlock.name = "Fake Block";
            _fakeBlock.transform.localScale = new Vector3(1, 0.9f, 1.0f);
            _fakeBlock.transform.localPosition = Vector3.zero;
            _fakeBlock.GetComponent<BoxCollider>().enabled = false;

            foreach (var blockMaterial in Settings.BlockMaterials)
            {
                if (blockMaterial.StateType == BlockStateType.Completed)
                {
                    _fakeBlock.GetComponent<MeshRenderer>().sharedMaterial = blockMaterial.Material;
                    break;
                }
            }
            
            _startInitialization = true;
        }

        public bool InitializeMap(int width, int height, int depth, Material material, int levelNumber)
        {
            MapData = new Array3D<Block>(width, height, depth);

            _width = width;
            _height = height;
            _depth = depth;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        GameObject newBlock = Instantiate(BlockPrefab, transform.localPosition, Quaternion.identity,
                            transform);

                        MapData[x, y, z] = newBlock.GetComponent<Block>();
                        MapData[x, y, z].BlockStateMachine = newBlock.GetComponent<BlockStateMachine>();
                        Vector3 localPosition = new Vector3(x, y, z);
                        MapData[x, y, z].ChangeBlockState(new BlockWallState(MapData[x, y, z]));
                        MapData[x, y, z].Position = localPosition;
                        MapData[x, y, z].Map = this;

                        MapNumber = levelNumber;

                        newBlock.name = $"Block: ({x}, {y}, {z})";
                        newBlock.transform.position = transform.position + localPosition;
                        newBlock.tag = "Block";
                        //asdsa 
                        newBlock.transform.parent = transform;
                    }
                }
            }
            
            _initialized = true;

            return true;
        }

        public void StartMap()
        {
            OnMapStart.Raise(this);
        }
        
        public void UpdatePlayerDirection(MovementDirection direction)
        {
            if (PlayerBlock != null && !_isCompleted)
            {
                if (PlayerBlock.MovementDirection == direction)
                    return;

                // sdjkfjasdfasdf
                if ((PlayerBlock.MovementDirection == MovementDirection.Up && direction == MovementDirection.Down) ||
                    (PlayerBlock.MovementDirection == MovementDirection.Down && direction == MovementDirection.Up) ||
                    (PlayerBlock.MovementDirection == MovementDirection.Right && direction == MovementDirection.Left) ||
                    (PlayerBlock.MovementDirection == MovementDirection.Left && direction == MovementDirection.Right))
                {
                    if (!PlayerBlock.IsCompleted)
                        return;
                }
                    

                PlayerBlock.MovementDirection = direction;
                
                if (PlayerBlock.MovementDirection != MovementDirection.Stop)
                    PlayerBlock.BlockStateMachine.Tick();    
            }
        }

        public void SetActiveMap(bool value) => _activeMap = value;

        void SetFakeBlockPosition(Vector3 position) => _fakeBlock.transform.localPosition = position;

        public void IncreaseCollectedCrystalCount() => _collectedCrystalCount++;

        public void ResetCollectedCrystal()
        {
            Debug.Log("Reset collected crystals");
            PlayerStats.CrystalCount -= _collectedCrystalCount;
            _collectedCrystalCount = 0;
        }

        public void OnPlayerStop(Block b)
        {
            UpdatePlayerDirection(MovementDirection.Stop);
            CompleteTrailBlocks();
            FillMap(b);
            SetFakeBlockPosition(b.Position);
        }

        public void UpdateBlockCountMap(Block block, BlockStateType oldStateType, BlockStateType newStateType)
        {
            BlockCountMap[newStateType]++;
            
            if (BlockCountMap[oldStateType] > 0 )
                BlockCountMap[oldStateType]--;
        }

        public void UpdatePlayerBlock(Block b)
        {
#if false
            if (PlayerBlock != null)
            { 
                Debug.LogWarning("You can't have more than one player inside a map!");
                PlayerBlock.ChangeBlockState(new BlockEmptyState(PlayerBlock));
            }
#endif
            
            _playerBlock = b;
            
            if (_playerBlock.IsCompleted)
                SetFakeBlockPosition(_playerBlock.Position);
        }

        private bool CheckIfCompleted() 
        {
            return BlockCountMap[BlockStateType.Empty] <= 0 && BlockCountMap[BlockStateType.Enemy] <= 0;
        }

        private void EnumerateOnBlocks(Action<Block> action)
        {
            if (!_initialized)
            {
                Debug.Log("Map is not initialized yet!");
                return;
            }

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int z = 0; z < _depth; z++)
                    {
                        action(MapData[x, y, z]);
                    }
                }
            }
        }

        public void FillArea(int xPosition, int zPosition, BlockStateType newStateType)
        {
            Block block = MapData[xPosition, 0, zPosition];

            if (block == null || block.StateType == newStateType)
                return;

            if (block.StateType == BlockStateType.Player || block.StateType == BlockStateType.Wall)
                return;

            if (block.StateType == BlockStateType.Obstacle)
                return;

            if (block.StateType == BlockStateType.Trail)
                return;

            block.ChangeBlockState(new BlockCompletedState(block));

            FillArea(xPosition + 1, zPosition, BlockStateType.Completed);
            FillArea(xPosition - 1, zPosition, BlockStateType.Completed);
            FillArea(xPosition, zPosition + 1, BlockStateType.Completed);
            FillArea(xPosition, zPosition - 1, BlockStateType.Completed);
        }

        public void CompleteTrailBlocks()
        {
            TrailBlocks.ForEach((block) => block.ChangeBlockState(new BlockCompletedState(block)));
            TrailBlocks.Clear();
        }
        
        public void CalculateArea(int xPosition, int zPosition, ref int area)
        {
            Block block = MapData[xPosition, 0, zPosition];

            if (block == null || block.IsMarked)
                return;

            if (block.StateType == BlockStateType.Player || block.StateType == BlockStateType.Wall || block.StateType == BlockStateType.Obstacle)
                return;

            if (block.StateType == BlockStateType.Trail)
            {
                area++;
                block.IsMarked = true;
                return;
            }

            block.IsMarked = true;
            area++;

            CalculateArea(xPosition + 1, zPosition, ref area);
            CalculateArea(xPosition - 1, zPosition, ref area);
            CalculateArea(xPosition, zPosition + 1, ref area);
            CalculateArea(xPosition, zPosition - 1, ref area);
        }

        public Block CheckIfBlocksClosedArea(Block b1, Block b2, ref bool flag)
        {
            if (b1 != null && b1.StateType != BlockStateType.Completed)
            {
                CheckIfBlockInClosedArea((int)b1.Position.x, (int)b1.Position.z, ref flag);
                EnumerateOnBlocks((block) =>
                {
                    if (block.StateType != BlockStateType.Completed)
                        block.IsMarked = false;
                });

                if (flag)
                    return b1;

                flag = true;
            }

            if (b2 != null && b2.StateType != BlockStateType.Completed)
            {
                CheckIfBlockInClosedArea((int)b2.Position.x, (int)b2.Position.z, ref flag);
                EnumerateOnBlocks((block) =>
                {
                    if (block.StateType != BlockStateType.Completed)
                        block.IsMarked = false;
                });

                if (flag)
                    return b2;
            }
            
            return null;
        }

        private void CheckIfBlockInClosedArea(int xPosition, int zPosition, ref bool flag)
        {
            Block block = MapData[xPosition, 0, zPosition];
            
            // Don't forget to add BlockStateType.Completed check
            if (block == null || block.IsMarked || block.StateType == BlockStateType.Player || block.StateType == BlockStateType.Completed || block.StateType == BlockStateType.Trail)
                return;

            if (BlockStateType.Wall == block.StateType)
            {
                flag = false;
                return;
            }

            block.IsMarked = true;
            
            CheckIfBlockInClosedArea(xPosition + 1, zPosition, ref flag);
            CheckIfBlockInClosedArea(xPosition - 1, zPosition, ref flag);
            CheckIfBlockInClosedArea(xPosition, zPosition + 1, ref flag);
            CheckIfBlockInClosedArea(xPosition, zPosition - 1, ref flag);
        }

        public void FillMap(Block b)
        {            Block block1 = null, block2 = null;
            
            int bXPosition = (int) b.Position.x;
            int bZPosition = (int) b.Position.z;
            
            if (PlayerBlock.MovementDirection == MovementDirection.Right ||
                PlayerBlock.MovementDirection == MovementDirection.Left)
            {
                block1 = MapData[bXPosition, 0, bZPosition + 1];
                block2 = MapData[bXPosition, 0, bZPosition - 1];
            }
            else if (PlayerBlock.MovementDirection == MovementDirection.Down ||
                     PlayerBlock.MovementDirection == MovementDirection.Up)
            {
                block1 = MapData[bXPosition + 1, 0, bZPosition];
                block2 = MapData[bXPosition - 1, 0, bZPosition];
            }

            bool flag = true;
            Block blockInsideClosedArea = CheckIfBlocksClosedArea(block1, block2, ref flag);
            
            if (blockInsideClosedArea != null)
                Debug.Log($"{blockInsideClosedArea.name} StateType: {blockInsideClosedArea.StateType}");
            
            // If there is an area that is surrounded by completed and trail blocks, fill it
            if (blockInsideClosedArea != null && blockInsideClosedArea.StateType != BlockStateType.Completed && blockInsideClosedArea.StateType != BlockStateType.Wall)
            {
                if (blockInsideClosedArea != null)
                    Debug.Log("Found An area to fill");

                FillArea((int) blockInsideClosedArea.Position.x, (int) blockInsideClosedArea.Position.z, BlockStateType.Completed);
            }
            // If we can't find an area that is covered with completed or trail blocks then
            // Then find the smallest area that is covered with both wall and completed block and fill that area
            else
            {
                int area1 = 0, area2 = 0;

                if (block1 != null)
                {
                    CalculateArea((int)block1.Position.x, (int)block1.Position.z, ref area1);
                    EnumerateOnBlocks((block) =>
                    {
                        if (block.StateType != BlockStateType.Completed)
                            block.IsMarked = false;
                    });
                }

                if (block2 != null)
                {
                    CalculateArea((int)block2.Position.x, (int)block2.Position.z, ref area2);
                    EnumerateOnBlocks((block) =>
                    {
                        if (block.StateType != BlockStateType.Completed)
                            block.IsMarked = false;
                    });
                }
                
                if (area1 != area2)
                {
                    int xPosition = area1 > area2 ? (int)(block2 == null ? 0 : block2.Position.x) : (int)(block1 == null ? 0 : block1.Position.x);
                    int zPosition = area1 > area2 ? (int)(block2 == null ? 0 : block2.Position.z) : (int)(block1 == null ? 0 : block1.Position.z);
                    FillArea(xPosition, zPosition, BlockStateType.Completed);
                }
            }

            EnumerateOnBlocks((block) =>
            {
                if (block.StateType != BlockStateType.Completed)
                    block.IsMarked = false;
            });
            
            ProgressBar.UpdateProgressBar(MapProgress);

            if (CheckIfCompleted())
                CompleteMap();
        }

        private void CompleteMap()
        {
            if (!_isCompleted)
            {
                UpdatePlayerDirection(MovementDirection.Stop);

                _isCompleted = true;
                OnMapCompleted.Raise(this);
            }
        }
    } 
}
