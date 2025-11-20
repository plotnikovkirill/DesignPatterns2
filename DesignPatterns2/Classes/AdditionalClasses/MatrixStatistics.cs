using DesignPatterns2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Classes.AdditionalClasses
{
    internal class MatrixStatistics
    {
        private IMatrix _matrix;
        public MatrixStatistics(IMatrix matrix)
        {
            _matrix = matrix;
        }
        public float SumOfValues
        {
            get
            {
                float sum = 0;
                for (int i = 0; i < _matrix.RowNum; ++i)
                {
                    for (int j = 0; j < _matrix.ColumnNum; ++j)
                    {
                        sum += _matrix.GetElement(i, j);
                    }
                }
                return sum;
            }
        }

        public float AverageValue
        {
            get
            {
                int totalCount = _matrix.RowNum * _matrix.ColumnNum;
                return SumOfValues / totalCount;
            }
        }

        public float MaxValue
        {
            get
            {
                float max = 0;
                for (int i = 0; i < _matrix.RowNum; ++i)
                {
                    for (int j = 0; j < _matrix.ColumnNum; ++j)
                    {
                        if (max < _matrix.GetElement(i, j))
                        {
                            max = _matrix.GetElement(i, j);
                        }
                    }
                }
                return max;
            }
        }

        public int NotZeroCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _matrix.RowNum; ++i)
                {
                    for (int j = 0; j < _matrix.ColumnNum; ++j)
                    {
                        if (_matrix.GetElement(i, j) != 0)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }
    }
}
