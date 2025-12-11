using DesignPatterns2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns2.Classes.Matrix
{
    /// <summary>
    /// Реализация паттерна Composite для горизонтальной группы матриц.
    /// Позволяет объединить несколько матриц в одну составную матрицу,
    /// где матрицы располагаются горизонтально (слева направо).
    /// 
    /// Пример: [Matrix1 | Matrix2 | Matrix3]
    /// 
    /// Логика работы:
    /// - RowNum: возвращает МАКСИМАЛЬНОЕ количество строк среди всех матриц группы
    /// - ColumnNum: возвращает СУММУ столбцов всех матриц группы
    /// - GetElement/SetElement: определяет, к какой матрице относится запрашиваемый элемент
    /// </summary>
    internal class HorizontalMatrixGroup : IMatrix
    {
        // Список матриц, составляющих горизонтальную группу
        private List<IMatrix> _matrices;

        /// <summary>
        /// Конструктор создает пустую группу матриц
        /// </summary>
        public HorizontalMatrixGroup()
        {
            _matrices = new List<IMatrix>();
        }

        /// <summary>
        /// Конструктор с начальным списком матриц
        /// </summary>
        public HorizontalMatrixGroup(IEnumerable<IMatrix> matrices)
        {
            _matrices = new List<IMatrix>(matrices ?? throw new ArgumentNullException(nameof(matrices)));
        }

        /// <summary>
        /// Добавить матрицу в группу
        /// Новая матрица добавляется справа от существующих
        /// </summary>
        public void AddMatrix(IMatrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            _matrices.Add(matrix);
        }

        /// <summary>
        /// Количество матриц в группе
        /// </summary>
        public int MatrixCount => _matrices.Count;

        /// <summary>
        /// ЧИСЛО_СТРОК: возвращает МАКСИМАЛЬНОЕ количество строк
        /// среди всех матриц в группе.
        /// 
        /// Почему максимальное?
        /// Представим горизонтальную группу:
        /// [2x2 | 3x3 | 5x1]
        /// 
        /// Результирующая матрица должна иметь 5 строк (максимум),
        /// чтобы вместить самую высокую матрицу.
        /// Матрицы с меньшим количеством строк "дополняются" нулями снизу.
        /// </summary>
        public int RowNum
        {
            get
            {
                if (_matrices.Count == 0)
                    return 0;

                return _matrices.Max(m => m.RowNum);
            }
        }

        /// <summary>
        /// ЧИСЛО_СТОЛБЦОВ: возвращает СУММАРНОЕ количество столбцов
        /// всех матриц в группе.
        /// 
        /// Почему суммарное?
        /// При горизонтальном объединении матрицы располагаются слева направо:
        /// [2x2 | 3x3 | 5x1] => результат имеет 2+3+1=6 столбцов
        /// </summary>
        public int ColumnNum
        {
            get
            {
                if (_matrices.Count == 0)
                    return 0;

                return _matrices.Sum(m => m.ColumnNum);
            }
        }

        /// <summary>
        /// Получить элемент составной матрицы по индексам
        /// 
        /// Алгоритм:
        /// 1. Определяем, к какой матрице относится столбец indexY
        /// 2. Вычисляем локальный индекс столбца внутри найденной матрицы
        /// 3. Проверяем, существует ли строка indexX в этой матрице
        /// 4. Возвращаем элемент или 0, если строка выходит за границы
        /// </summary>
        public float GetElement(int indexX, int indexY)
        {
            if (_matrices.Count == 0)
                return 0;

            // Проверка строки: не должна превышать максимальную
            if (indexX < 0 || indexX >= RowNum)
                return 0;

            // Проверка столбца: не должна превышать суммарную ширину
            if (indexY < 0 || indexY >= ColumnNum)
                return 0;

            // Находим матрицу, содержащую запрашиваемый столбец
            int currentColumnOffset = 0;
            foreach (var matrix in _matrices)
            {
                int matrixColumnCount = matrix.ColumnNum;

                // Проверяем, попадает ли indexY в диапазон текущей матрицы
                if (indexY < currentColumnOffset + matrixColumnCount)
                {
                    // Вычисляем локальный индекс столбца
                    int localColumnIndex = indexY - currentColumnOffset;

                    // Если строка indexX существует в этой матрице, возвращаем элемент
                    // Иначе возвращаем 0 (матрица "дополняется" нулями)
                    if (indexX < matrix.RowNum)
                    {
                        return matrix.GetElement(indexX, localColumnIndex);
                    }
                    else
                    {
                        return 0; // Строка выходит за границы этой матрицы
                    }
                }

                // Переходим к следующей матрице
                currentColumnOffset += matrixColumnCount;
            }

            return 0;
        }

        /// <summary>
        /// Установить значение элемента составной матрицы
        /// 
        /// Алгоритм аналогичен GetElement:
        /// 1. Находим матрицу, которой принадлежит столбец
        /// 2. Вычисляем локальный индекс
        /// 3. Если строка существует в матрице - записываем значение
        /// 4. Если строка выходит за границы - игнорируем (или можно выбросить исключение)
        /// </summary>
        public void SetElement(int indexX, int indexY, float newValue)
        {
            if (_matrices.Count == 0)
                return;

            // Проверка границ
            if (indexX < 0 || indexX >= RowNum)
                return;

            if (indexY < 0 || indexY >= ColumnNum)
                return;

            // Находим матрицу, содержащую запрашиваемый столбец
            int currentColumnOffset = 0;
            foreach (var matrix in _matrices)
            {
                int matrixColumnCount = matrix.ColumnNum;

                if (indexY < currentColumnOffset + matrixColumnCount)
                {
                    int localColumnIndex = indexY - currentColumnOffset;

                    // Устанавливаем значение только если строка существует
                    if (indexX < matrix.RowNum)
                    {
                        matrix.SetElement(indexX, localColumnIndex, newValue);
                    }
                    // Если строка выходит за границы матрицы, ничего не делаем
                    // (попытка записи в "виртуальный" нулевой элемент)

                    return;
                }

                currentColumnOffset += matrixColumnCount;
            }
        }

        /// <summary>
        /// Получить матрицу по индексу (для отладки и тестирования)
        /// </summary>
        public IMatrix GetMatrix(int index)
        {
            if (index < 0 || index >= _matrices.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _matrices[index];
        }

        /// <summary>
        /// Очистить группу матриц
        /// </summary>
        public void Clear()
        {
            _matrices.Clear();
        }
    }
}