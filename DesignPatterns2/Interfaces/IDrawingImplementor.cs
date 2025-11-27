using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;
namespace DesignPatterns2.Interfaces
{
        /// <summary>
        /// Интерфейс реализатора для паттерна Bridge
        /// Определяет методы для отрисовки визуальных элементов матрицы
        /// </summary>
        internal interface IDrawingImplementor
        {
            /// <summary>
            /// Рисует внешнюю границу матрицы
            /// </summary>
            void DrawBorder(IMatrix matrix);

            /// <summary>
            /// Рисует содержимое ячейки
            /// </summary>
            void DrawCell(int row, int col, float value);

            /// <summary>
            /// Завершает отрисовку (для очистки или финализации)
            /// </summary>
            void FinishDrawing();

            /// <summary>
            /// Устанавливает, нужно ли отображать границу
            /// </summary>
            bool ShowBorder { get; set; }
        }
}
