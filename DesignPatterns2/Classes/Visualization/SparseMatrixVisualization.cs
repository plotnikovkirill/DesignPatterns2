using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Visualization
{
    internal class SparseMatrixVisualization : MatrixVisualization
    {
        public SparseMatrixVisualization(IMatrix matrix, IDrawingImplementor implementor)
            : base(matrix, implementor)
        {
        }

        public override void Visualize()
        {
            // 1. Рисуем внешнюю границу
            _implementor.DrawBorder(_matrix);

            // 2. Рисуем ТОЛЬКО ненулевые ячейки
            for (int i = 0; i < _matrix.RowNum; i++)
            {
                for (int j = 0; j < _matrix.ColumnNum; j++)
                {
                    float value = _matrix.GetElement(i, j);

                    // Визуализируем только ненулевые элементы
                    if (value != 0)
                    {
                        _implementor.DrawCell(i, j, value);
                    }
                    //else
                    //{
                    //    _implementor.DrawCell(i, j, -2);
                    //}
                }
            }

            // 3. Завершаем отрисовку
            _implementor.FinishDrawing();
        }
    }
}
