using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;
using System.Drawing;

namespace DesignPatterns2.Classes.Drawing
{
    

        /// <summary>
        /// Конкретная реализация графической отрисовки
        /// </summary>
        internal class GraphicsDrawing : IDrawingImplementor
        {
            private Graphics? _graphics;
            private int _cellSize = 50;
            private int _offsetX = 10;
            private int _offsetY = 10;
            private int _rows;
            private int _columns;
            private Pen _borderPen;
            private Font _font;
            private Brush _textBrush;
            private Brush _backgroundBrush;

            public bool ShowBorder { get; set; }

            public GraphicsDrawing(Graphics graphics, int cellSize = 50)
            {
                _graphics = graphics;
                _cellSize = cellSize;
                ShowBorder = true;

                _borderPen = new Pen(Color.Black, 2);
                _font = new Font("Arial", 12);
                _textBrush = new SolidBrush(Color.Black);
                _backgroundBrush = new SolidBrush(Color.White);
            }

            public void DrawBorder(IMatrix matrix)
            {
                _rows = matrix.RowNum;
                _columns = matrix.ColumnNum;

                if (_graphics == null) return;

                // Очистка области рисования
                _graphics.Clear(Color.White);

                if (ShowBorder)
                {
                    // Рисуем внешнюю границу матрицы
                    Rectangle borderRect = new Rectangle(
                        _offsetX,
                        _offsetY,
                        _columns * _cellSize,
                        _rows * _cellSize
                    );
                    _graphics.DrawRectangle(_borderPen, borderRect);
                }
            }

            public void DrawCell(int row, int col, float value)
            {
                if (_graphics == null) return;

                int x = _offsetX + col * _cellSize;
                int y = _offsetY + row * _cellSize;

                // Рисуем границы ячейки
                if (ShowBorder)
                {
                    Rectangle cellRect = new Rectangle(x, y, _cellSize, _cellSize);
                    _graphics.DrawRectangle(Pens.Gray, cellRect);
                }

                // Рисуем значение в центре ячейки
                string valueStr = value.ToString("0.##");
                SizeF textSize = _graphics.MeasureString(valueStr, _font);

                float textX = x + (_cellSize - textSize.Width) / 2;
                float textY = y + (_cellSize - textSize.Height) / 2;

                _graphics.DrawString(valueStr, _font, _textBrush, textX, textY);
            }

            public void FinishDrawing()
            {
                // Метод для завершения отрисовки (если нужна дополнительная логика)
            }

            public void SetGraphics(Graphics graphics)
            {
                _graphics = graphics;
            }
        }
    
}
