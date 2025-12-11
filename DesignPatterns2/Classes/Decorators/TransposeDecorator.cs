using DesignPatterns2.Interfaces;
using System;
using DesignPatterns2.Classes.Matrix;

namespace DesignPatterns2.Classes.Decorators
{
    /// <summary>
    /// Декоратор транспонирования матрицы.
    /// Реализует паттерн Decorator для создания транспонированного представления матрицы.
    /// 
    /// ЧТО ТАКОЕ ТРАНСПОНИРОВАНИЕ?
    /// Транспонирование - это операция, при которой строки и столбцы меняются местами:
    /// 
    /// Исходная матрица 3x2:     Транспонированная 2x3:
    /// [1 2]                     [1 3 5]
    /// [3 4]          =>         [2 4 6]
    /// [5 6]
    /// 
    /// КАК ЭТО ПОМОГАЕТ СОЗДАТЬ ВЕРТИКАЛЬНЫЕ ГРУППЫ?
    /// 
    /// Вертикальная группа - это матрицы, расположенные одна под другой:
    /// [Matrix1]
    /// [Matrix2]
    /// [Matrix3]
    /// 
    /// Идея: если мы умеем делать ГОРИЗОНТАЛЬНЫЕ группы [M1 | M2 | M3],
    /// то можем создать ВЕРТИКАЛЬНУЮ группу, используя транспонирование:
    /// 
    /// 1. Транспонируем каждую матрицу M1, M2, M3
    /// 2. Объединяем их горизонтально: [M1^T | M2^T | M3^T]
    /// 3. Транспонируем результат обратно
    /// 
    /// Результат: вертикальная группа!
    /// 
    /// ПРИМЕР:
    /// Хотим создать вертикальную группу из матриц 2x3 и 2x3:
    /// 
    /// [1 2 3]        M1 (2x3)
    /// [4 5 6]
    /// 
    /// [7 8 9]        M2 (2x3)
    /// [0 1 2]
    /// 
    /// Результат (вертикальная группа 4x3):
    /// [1 2 3]
    /// [4 5 6]
    /// [7 8 9]
    /// [0 1 2]
    /// 
    /// Как достичь через транспонирование:
    /// 1. M1^T = 3x2, M2^T = 3x2
    /// 2. Горизонтальная группа [M1^T | M2^T] = 3x4
    /// 3. Транспонируем обратно: (3x4)^T = 4x3 ✓
    /// </summary>
    internal class TransposeDecorator : IMatrix
    {
        private readonly IMatrix _decoratedMatrix;

        /// <summary>
        /// Создает транспонированное представление матрицы
        /// </summary>
        /// <param name="matrix">Декорируемая матрица</param>
        public TransposeDecorator(IMatrix matrix)
        {
            _decoratedMatrix = matrix ?? throw new ArgumentNullException(nameof(matrix));
        }

        /// <summary>
        /// Количество строк транспонированной матрицы = 
        /// количеству столбцов исходной матрицы
        /// </summary>
        public int RowNum => _decoratedMatrix.ColumnNum;

        /// <summary>
        /// Количество столбцов транспонированной матрицы = 
        /// количеству строк исходной матрицы
        /// </summary>
        public int ColumnNum => _decoratedMatrix.RowNum;

        /// <summary>
        /// Получить элемент транспонированной матрицы.
        /// 
        /// При транспонировании строки и столбцы меняются местами:
        /// Element[i,j] транспонированной = Element[j,i] исходной
        /// 
        /// Пример:
        /// Исходная:           Транспонированная:
        /// [1 2 3]             [1 4]
        /// [4 5 6]             [2 5]
        ///                     [3 6]
        /// 
        /// GetElement(0,1) транспонированной = GetElement(1,0) исходной = 4
        /// </summary>
        public float GetElement(int indexX, int indexY)
        {
            // Меняем местами индексы: строка становится столбцом, столбец - строкой
            return _decoratedMatrix.GetElement(indexY, indexX);
        }

        /// <summary>
        /// Установить элемент транспонированной матрицы.
        /// Аналогично GetElement, меняем индексы местами.
        /// </summary>
        public void SetElement(int indexX, int indexY, float newValue)
        {
            // Меняем местами индексы
            _decoratedMatrix.SetElement(indexY, indexX, newValue);
        }

        /// <summary>
        /// Получить исходную (нетранспонированную) матрицу
        /// </summary>
        public IMatrix GetDecoratedMatrix()
        {
            return _decoratedMatrix;
        }
    }

    /// <summary>
    /// Вспомогательный класс для создания вертикальных групп матриц
    /// 
    /// ИСПОЛЬЗОВАНИЕ:
    /// 
    /// // Создаем вертикальную группу из трех матриц
    /// var verticalGroup = VerticalMatrixGroupHelper.CreateVerticalGroup(
    ///     matrix1,  // будет сверху
    ///     matrix2,  // будет посередине
    ///     matrix3   // будет снизу
    /// );
    /// 
    /// ПРИНЦИП РАБОТЫ:
    /// 1. Транспонируем каждую входную матрицу
    /// 2. Создаем горизонтальную группу из транспонированных матриц
    /// 3. Транспонируем результат обратно
    /// 4. Получаем вертикальную группу!
    /// </summary>
    public static class VerticalMatrixGroupHelper
    {
        /// <summary>
        /// Создать вертикальную группу из массива матриц.
        /// Матрицы располагаются сверху вниз в порядке перечисления.
        /// 
        /// Результат:
        /// - RowNum = сумма высот всех матриц
        /// - ColumnNum = максимальная ширина среди всех матриц
        /// </summary>
        public static IMatrix CreateVerticalGroup(params IMatrix[] matrices)
        {
            if (matrices == null || matrices.Length == 0)
                throw new ArgumentException("Необходимо указать хотя бы одну матрицу", nameof(matrices));

            // Шаг 1: Транспонируем каждую матрицу
            var transposedMatrices = new IMatrix[matrices.Length];
            for (int i = 0; i < matrices.Length; i++)
            {
                transposedMatrices[i] = new TransposeDecorator(matrices[i]);
            }

            // Шаг 2: Создаем горизонтальную группу из транспонированных матриц
            var horizontalGroup = new Matrix.HorizontalMatrixGroup(transposedMatrices);

            // Шаг 3: Транспонируем результат обратно
            var verticalGroup = new TransposeDecorator(horizontalGroup);

            return verticalGroup;
        }

        /// <summary>
        /// Альтернативный способ: добавление матриц по одной
        /// </summary>
        public static IMatrix CreateVerticalGroupBuilder()
        {
            var horizontalGroup = new Matrix.HorizontalMatrixGroup();
            return new VerticalMatrixGroupBuilder(horizontalGroup);
        }
    }

    /// <summary>
    /// Паттерн Builder для пошагового создания вертикальной группы
    /// </summary>
    internal class VerticalMatrixGroupBuilder : IMatrix
    {
        private readonly Matrix.HorizontalMatrixGroup _horizontalGroup;
        private TransposeDecorator? _result;

        public VerticalMatrixGroupBuilder(Matrix.HorizontalMatrixGroup horizontalGroup)
        {
            _horizontalGroup = horizontalGroup;
        }

        /// <summary>
        /// Добавить матрицу в вертикальную группу (она будет добавлена снизу)
        /// </summary>
        public void AddMatrix(IMatrix matrix)
        {
            // Добавляем транспонированную матрицу в горизонтальную группу
            _horizontalGroup.AddMatrix(new TransposeDecorator(matrix));
            _result = null; // Сбрасываем кэшированный результат
        }

        private TransposeDecorator GetResult()
        {
            if (_result == null)
            {
                _result = new TransposeDecorator(_horizontalGroup);
            }
            return _result;
        }

        public int RowNum => GetResult().RowNum;
        public int ColumnNum => GetResult().ColumnNum;
        public float GetElement(int indexX, int indexY) => GetResult().GetElement(indexX, indexY);
        public void SetElement(int indexX, int indexY, float newValue) => GetResult().SetElement(indexX, indexY, newValue);
    }
}