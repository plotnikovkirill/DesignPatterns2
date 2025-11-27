using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Visualization
{
    internal class RegularMatrixVisualization : MatrixVisualization
    {
        public RegularMatrixVisualization(IMatrix matrix, IDrawingImplementor implementor)
            : base(matrix, implementor)
        {
        }

        public override void Visualize()
        {
            // 1. Рисуем внешнюю границу
            _implementor.DrawBorder(_matrix);

            // 2. Рисуем ВСЕ ячейки последовательно
            for (int i = 0; i < _matrix.RowNum; i++)
            {
                for (int j = 0; j < _matrix.ColumnNum; j++)
                {
                    float value = _matrix.GetElement(i, j);
                    _implementor.DrawCell(i, j, value);
                }
            }

            // 3. Завершаем отрисовку
            _implementor.FinishDrawing();
        }
    }
}
