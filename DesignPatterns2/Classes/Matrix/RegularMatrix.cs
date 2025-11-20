using DesignPatterns2.Classes.VectorClasses;
using DesignPatterns2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Classes.MatrixClasses
{
    internal class RegularMatrix: SomeMatrix
    {
        public RegularMatrix(int rows, int columns)
        {
            _components = new List<IVector>(rows);
            for (int i = 0; i < rows; i++)
            {
                _components.Add(new RegularVector(new float[columns]));
            }
        }
    }
}
