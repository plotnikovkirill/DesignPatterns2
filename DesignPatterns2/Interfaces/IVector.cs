using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Interfaces
{
    internal interface IVector
    {
        public int VectorDimensionality { get; }
        public float GetComponent(int index);
        public void SetComponent(int index, float value);
    }
}
