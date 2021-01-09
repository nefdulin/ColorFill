using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    public interface IBlockMovableState
    {
        IEnumerator StartMovement();
        IEnumerator MoveBlock();
    } 
}
