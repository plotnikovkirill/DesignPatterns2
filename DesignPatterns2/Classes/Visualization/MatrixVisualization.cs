using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Visualization
{
    internal abstract class MatrixVisualization
    {
        protected IDrawingImplementor _implementor;
        protected IMatrix _matrix;

        protected MatrixVisualization(IMatrix matrix, IDrawingImplementor implementor)
        {
            _matrix = matrix;
            _implementor = implementor;
        }

        /// <summary>
        /// Изменяет схему визуализации в режиме выполнения
        /// </summary>
        public void SetImplementor(IDrawingImplementor implementor)
        {
            _implementor = implementor;
        }

        /// <summary>
        /// Устанавливает опцию отображения границы
        /// </summary>
        public void SetBorderVisibility(bool showBorder)
        {
            _implementor.ShowBorder = showBorder;
        }

        /// <summary>
        /// Визуализирует матрицу
        /// </summary>
        public abstract void Visualize();
    }
}
