using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Interfaces
{
    internal interface IMatrix
    {
        public float GetElement(int indexX, int indexY);
        public void SetElement(int indexX, int indexY, float newValue);
        public int RowNum { get; }
        public int ColumnNum { get; }

    }
}
