using DesignPatterns2.Classes.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Classes.AdditionalClasses
{
    internal static class MatrixInitiator
    {
        private static readonly Random random = new Random();
        public static void FillMatrix(SomeMatrix matrix, int notZeroNumbers, int maxValue)
        {
            if (matrix == null)
            {
                return;
            }
            if (notZeroNumbers < 0)
            {
                return;
            } 
            if (maxValue < 0)
            {
                return;
            }
            if (matrix is RegularMatrix)
            {
                // Для RegularMatrix заполняем всю матрицу случайными числами
                for (int i = 0; i < matrix.RowNum; ++i)
                {
                    for (int j = 0; j < matrix.ColumnNum; ++j)
                    {
                        float value = random.Next(0, maxValue + 1);
                        matrix.SetElement(i, j, value);                                                                                                        
                    }
                }
            }
            else if (matrix is RAZMatrix)
            {
                int totalCells = matrix.ColumnNum * matrix.RowNum;
                notZeroNumbers = Math.Min(notZeroNumbers, totalCells);

                // Обнуляем матрицу
                for (int i = 0; i < matrix.RowNum; ++i)
                {
                    for (int j = 0; j < matrix.ColumnNum; ++j)
                    {
                        matrix.SetElement(i, j, 0);
                    }
                }

                var filled = new HashSet<(int, int)>();

                while (filled.Count < notZeroNumbers)
                {
                    int row = random.Next(matrix.RowNum);
                    int col = random.Next(matrix.ColumnNum);

                    if (!filled.Contains((row, col)))
                    {
                        float value = random.Next(1, maxValue + 1);
                        matrix.SetElement(row, col, value);
                        filled.Add((row, col));
                    }
                }
            }
        }
    }
}
