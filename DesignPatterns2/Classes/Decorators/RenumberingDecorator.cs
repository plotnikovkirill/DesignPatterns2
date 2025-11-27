using DesignPatterns2.Interfaces;
using System;

namespace DesignPatterns2.Classes.Decorators
{
    /// <summary>
    /// Декоратор для перенумерации строк и столбцов матрицы
    /// Реализует паттерн Decorator для интерфейса IMatrix
    /// </summary>
    internal class RenumberingDecorator : IMatrix
    {
        private readonly IMatrix _decoratedMatrix;

        // Таблицы перенумерации: новый индекс -> старый индекс
        private int[] _rowMapping;    // Перенумерация строк
        private int[] _columnMapping; // Перенумерация столбцов

        public RenumberingDecorator(IMatrix matrix)
        {
            _decoratedMatrix = matrix ?? throw new ArgumentNullException(nameof(matrix));

            // Инициализируем таблицы перенумерации как идентичные (без изменений)
            _rowMapping = new int[matrix.RowNum];
            _columnMapping = new int[matrix.ColumnNum];

            // Изначально каждый индекс соответствует самому себе
            for (int i = 0; i < _rowMapping.Length; i++)
                _rowMapping[i] = i;

            for (int i = 0; i < _columnMapping.Length; i++)
                _columnMapping[i] = i;
        }
        /// Количество строк (не меняется при перенумерации)
        public int RowNum => _decoratedMatrix.RowNum;

        /// Количество столбцов (не меняется при перенумерации)
        public int ColumnNum => _decoratedMatrix.ColumnNum;

        public float GetElement(int indexX, int indexY)
        {
            // Преобразуем новые индексы в старые с помощью таблиц перенумерации
            int originalRow = _rowMapping[indexX];
            int originalCol = _columnMapping[indexY];

            // Делегируем вызов декорируемой матрице с преобразованными индексами
            return _decoratedMatrix.GetElement(originalRow, originalCol);
        }

        public void SetElement(int indexX, int indexY, float newValue)
        {
            // Преобразуем новые индексы в старые
            int originalRow = _rowMapping[indexX];
            int originalCol = _columnMapping[indexY];

            // Делегируем вызов декорируемой матрице
            _decoratedMatrix.SetElement(originalRow, originalCol, newValue);
        }

        public void Renumber()
        {
            Random random = new Random();

            // Обмен двух случайных строк
            if (RowNum >= 2)
            {
                int row1 = random.Next(RowNum);
                int row2 = random.Next(RowNum);

                // Убедимся, что выбраны разные строки
                while (row1 == row2)
                    row2 = random.Next(RowNum);

                SwapRows(row1, row2);
            }

            // Обмен двух случайных столбцов
            if (ColumnNum >= 2)
            {
                int col1 = random.Next(ColumnNum);
                int col2 = random.Next(ColumnNum);

                // Убедимся, что выбраны разные столбцы
                while (col1 == col2)
                    col2 = random.Next(ColumnNum);

                SwapColumns(col1, col2);
            }
        }
        /// Восстановить исходную нумерацию строк и столбцов
        public void Restore()
        {
            // Сбрасываем таблицы перенумерации к исходному состоянию
            for (int i = 0; i < _rowMapping.Length; i++)
                _rowMapping[i] = i;

            for (int i = 0; i < _columnMapping.Length; i++)
                _columnMapping[i] = i;
        }
        /// Обменять местами две строки в таблице перенумерации
        private void SwapRows(int row1, int row2)
        {
            int temp = _rowMapping[row1];
            _rowMapping[row1] = _rowMapping[row2];
            _rowMapping[row2] = temp;
        }
        /// Обменять местами два столбца в таблице перенумерации
        private void SwapColumns(int col1, int col2)
        {
            int temp = _columnMapping[col1];
            _columnMapping[col1] = _columnMapping[col2];
            _columnMapping[col2] = temp;
        }
    }
}