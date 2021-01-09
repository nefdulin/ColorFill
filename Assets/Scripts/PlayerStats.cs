using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorFill
{
    // Using a good old static class to store data between scenes
    // Might have used a scriptable object or PlayerPrefs but they seemed like overkill
    public static class PlayerStats
    {
        public static int Level = 1;
        public static int CrystalCount = 0;
    }
}
