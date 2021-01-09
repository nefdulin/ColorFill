using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace ColorFill
{
    [Serializable]
    public class Array3D<T> : ISerializationCallbackReceiver
    {
        public T this[int x, int y, int z]
        {
            get { return _array[GetIndex(x, y, z)]; }
            set { _array[GetIndex(x, y, z)] = value; }
        }
        
        [SerializeField] 
        protected int _x;
        
        [SerializeField] 
        protected int _y;
        
        [SerializeField] 
        protected int _z;
        
        [SerializeField]
        protected T[] _array;

        public Array3D(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;

            _array = new T[x * y * z];
        }

        private int GetIndex(int x, int y, int z)
        {
            return x + (_x * y) + (_x * _y * z);
        }
        
        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}