using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Drawing
{
    internal class ConsoleDrawing : IDrawingImplementor
    {
        private StringBuilder _output;
        private int _columns;
        private int _rows;
        private bool[,] _drawnCells; // Отслеживаем, какие ячейки были нарисованы
        private int _currentRow;
        private int _currentCol;

        public bool ShowBorder { get; set; }

        public ConsoleDrawing()
        {
            _output = new StringBuilder();
            ShowBorder = true;
            _currentRow = 0;
            _currentCol = 0;
        }

        public void DrawBorder(IMatrix matrix)
        {
            _output.Clear();
            _columns = matrix.ColumnNum;
            _rows = matrix.RowNum;
            _drawnCells = new bool[_rows, _columns];
            _currentRow = 0;
            _currentCol = 0;

            if (ShowBorder)
            {
                _output.AppendLine("┌" + new string('─', _columns * 6 - 1) + "┐");
            }
        }

        public void DrawCell(int row, int col, float value)
        {
            // Если перешли на новую строку, завершаем предыдущую
            while (_currentRow < row)
            {
                FinishCurrentRow();
                _currentRow++;
                _currentCol = 0;
            }

            // Если нужно пропустить ячейки в текущей строке
            while (_currentCol < col)
            {
                DrawEmptyCell();
                _currentCol++;
            }

            // Рисуем текущую ячейку
            if (_currentCol == 0)
            {
                if (ShowBorder)
                    _output.Append("│");
                else
                    _output.Append(" ");
            }

            _output.AppendFormat("{0,5}", value.ToString("0.##"));
            _drawnCells[row, col] = true;

            if (_currentCol < _columns - 1)
            {
                _output.Append(" ");
            }

            _currentCol++;

            // Если это последняя ячейка в строке
            if (_currentCol >= _columns)
            {
                if (ShowBorder)
                    _output.AppendLine("│");
                else
                    _output.AppendLine();

                _currentRow++;
                _currentCol = 0;
            }
        }

        private void DrawEmptyCell()
        {
            if (_currentCol == 0)
            {
                if (ShowBorder)
                    _output.Append("│");
                else
                    _output.Append(" ");
            }

            _output.Append("     "); // 5 пробелов для пустой ячейки

            if (_currentCol < _columns - 1)
            {
                _output.Append(" ");
            }
        }

        private void FinishCurrentRow()
        {
            // Заполняем оставшиеся ячейки пустатой
            while (_currentCol < _columns)
            {
                DrawEmptyCell();
                _currentCol++;
            }

            if (ShowBorder)
                _output.AppendLine("│");
            else
                _output.AppendLine();
        }

        public void FinishDrawing()
        {
            // Завершаем текущую строку, если она не завершена
            if (_currentCol > 0 && _currentCol < _columns)
            {
                FinishCurrentRow();
                _currentRow++;
            }

            // Добавляем пустые строки, если они не были нарисованы
            while (_currentRow < _rows)
            {
                _currentCol = 0;
                FinishCurrentRow();
                _currentRow++;
            }

            if (ShowBorder)
            {
                _output.AppendLine("└" + new string('─', _columns * 6 - 1) + "┘");
            }
            Console.Write(_output.ToString());
        }

        public string GetOutput()
        {
            return _output.ToString();
        }
    }
}
