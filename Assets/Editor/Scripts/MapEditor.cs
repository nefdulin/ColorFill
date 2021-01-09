using System;
using System.Collections;
using System.Collections.Generic;
using ColorFill.States;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;

namespace ColorFill
{
    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {
        private Map _map;

        private int _x;
        private int _y;
        private int _z;

        private BlockStateType _placementStateType;

        private bool _shouldMove;
        private MovementDirection _movementDirection;
        private int _movementCount = 1;
        
        public void OnEnable()
        {
            _map = target as Map;
        }

        void OnSceneGUI()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 1)
                {
                    RaycastHit hit;

                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    
                    if (Physics.Raycast(ray, out hit, 1000.0f))
                        if (hit.collider.transform.tag == "Block")
                        {
                            Block b = hit.collider.transform.GetComponent<Block>();
                            if (_placementStateType == BlockStateType.Wall) 
                                b.ChangeBlockState(new BlockWallState(b));
                            else if (_placementStateType == BlockStateType.Trail)
                                b.ChangeBlockState(new BlockTrailState(b));
                            else if (_placementStateType == BlockStateType.Completed)
                                b.ChangeBlockState((new BlockCompletedState(b)));
                            else if (_placementStateType == BlockStateType.Enemy)
                            {
                                b.ChangeBlockState(new BlockEnemyState(b, _shouldMove, _movementDirection, _movementCount, 0));
                                b.UpdateTargetPosition();
                            }
                            else if (_placementStateType == BlockStateType.Player)
                            {
                                b.ChangeBlockState(new BlockPlayerState(b));
                                _map.UpdatePlayerBlock(b);
                            }
                            else if (_placementStateType == BlockStateType.Obstacle)
                                b.ChangeBlockState(new BlockObstacleState(b));
                            else if (_placementStateType == BlockStateType.Crystal)
                                b.ChangeBlockState(new BlockCrystalState(b));
                            else if (_placementStateType == BlockStateType.Empty)
                                b.ChangeBlockState(new BlockEmptyState(b));
                        }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _placementStateType = (BlockStateType) EditorGUILayout.EnumPopup("Placement StateType", _placementStateType);
            
            if (_placementStateType == BlockStateType.Enemy)
            {
                _shouldMove = EditorGUILayout.ToggleLeft("Move", _shouldMove);
                _movementDirection = (MovementDirection) EditorGUILayout.EnumPopup("Placement StateType", _movementDirection);
                _movementCount = EditorGUILayout.IntField("Movement Count", _movementCount);
            }
        }
    } 
}
