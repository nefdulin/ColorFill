using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill.States
{
    public interface IState
    {
        IEnumerator Start();

        IEnumerator Tick();

        IEnumerator Exit();
    }
}