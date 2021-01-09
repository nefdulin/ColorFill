using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States 
{
    public class BlockCrystalState : BlockState
    {
        public BlockCrystalState(Block b) : base(b) { }

        public override IEnumerator Start()
        {
            Block.StateType = BlockStateType.Crystal;
            Block.Renderer.sharedMaterial = Block.GetBlockMaterial();
            Block.transform.localScale = Block.Map.Settings.CrystalBlockScale;
            // Should optimize this
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Block.GetComponent<MeshFilter>().sharedMesh = GameObject.Instantiate(sphere.GetComponent<MeshFilter>().sharedMesh);
            if (Application.isPlaying)
                GameObject.Destroy(sphere);
            else
                GameObject.DestroyImmediate(sphere);

            yield break;
        }

        public override IEnumerator Exit()
        {
            Block.Collider.isTrigger = false;
            Block.transform.localScale = Vector3.one;
            // Optimizeeeeeeeeesdfgjkşsdfgsdşfdasdfa
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Block.GetComponent<MeshFilter>().sharedMesh = GameObject.Instantiate(cube.GetComponent<MeshFilter>().mesh);

            if (Application.isPlaying)
            {
                PlayerStats.CrystalCount++;
                Block.OnCrystalCollected.Raise();
                GameObject.Destroy(cube);
            }
            else
                GameObject.DestroyImmediate(cube);
            
            yield break;
        }
    } 
}
