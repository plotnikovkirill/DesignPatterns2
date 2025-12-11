using DesignPatterns2.Classes.Matrix;
using DesignPatterns2.Classes.Decorators;
using DesignPatterns2.Interfaces;
using System;
using DesignPatterns2.Classes.Matrix;

namespace DesignPatterns2.Tests
{
    /// <summary>
    /// Тесты для проверки работы паттерна Composite с матрицами
    /// Эти тесты можно запускать для проверки корректности реализации
    /// </summary>
    internal static class CompositeMatrixTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=== ЗАПУСК ТЕСТОВ COMPOSITE PATTERN ===\n");

            TestHorizontalGroupBasic();
            TestHorizontalGroupDifferentSizes();
            TestHorizontalGroupGetSetElements();
            TestVerticalGroupBasic();
            TestComplexComposite();

            Console.WriteLine("\n=== ВСЕ ТЕСТЫ ПРОЙДЕНЫ УСПЕШНО ===");
        }

        /// <summary>
        /// ТЕСТ 1: Базовый тест горизонтальной группы
        /// Проверяем правильность вычисления размеров
        /// </summary>
        private static void TestHorizontalGroupBasic()
        {
            Console.WriteLine("ТЕСТ 1: Базовая горизонтальная группа");
            Console.WriteLine("-------------------------------------");

            // Создаем две матрицы 3x2
            var m1 = new RegularMatrix(3, 2);
            var m2 = new RegularMatrix(3, 2);

            // Создаем горизонтальную группу
            var hGroup = new HorizontalMatrixGroup();
            hGroup.AddMatrix(m1);
            hGroup.AddMatrix(m2);

            // Проверяем размеры
            // RowNum должен быть max(3, 3) = 3
            // ColumnNum должен быть 2 + 2 = 4
            Assert(hGroup.RowNum == 3, "RowNum должен быть 3");
            Assert(hGroup.ColumnNum == 4, "ColumnNum должен быть 4");
            Assert(hGroup.MatrixCount == 2, "Количество матриц должно быть 2");

            Console.WriteLine($"✓ RowNum = {hGroup.RowNum} (ожидалось 3)");
            Console.WriteLine($"✓ ColumnNum = {hGroup.ColumnNum} (ожидалось 4)");
            Console.WriteLine($"✓ MatrixCount = {hGroup.MatrixCount}\n");
        }

        /// <summary>
        /// ТЕСТ 2: Горизонтальная группа с матрицами разных размеров
        /// Проверяем правильность вычисления максимальной высоты
        /// </summary>
        private static void TestHorizontalGroupDifferentSizes()
        {
            Console.WriteLine("ТЕСТ 2: Матрицы разных размеров");
            Console.WriteLine("-------------------------------");

            // Создаем матрицы как в задании: 2x2, 3x3, 5x1, 1x1
            var m1 = new RegularMatrix(2, 2);
            var m2 = new RegularMatrix(3, 3);
            var m3 = new RegularMatrix(5, 1);
            var m4 = new RegularMatrix(1, 1);

            var hGroup = new HorizontalMatrixGroup();
            hGroup.AddMatrix(m1);
            hGroup.AddMatrix(m2);
            hGroup.AddMatrix(m3);
            hGroup.AddMatrix(m4);

            // RowNum = max(2, 3, 5, 1) = 5
            // ColumnNum = 2 + 3 + 1 + 1 = 7
            Assert(hGroup.RowNum == 5, "RowNum должен быть 5 (максимум)");
            Assert(hGroup.ColumnNum == 7, "ColumnNum должен быть 7 (сумма)");

            Console.WriteLine($"✓ Группа [2x2 | 3x3 | 5x1 | 1x1]");
            Console.WriteLine($"✓ RowNum = {hGroup.RowNum} (max = 5)");
            Console.WriteLine($"✓ ColumnNum = {hGroup.ColumnNum} (sum = 7)\n");
        }

        /// <summary>
        /// ТЕСТ 3: Чтение и запись элементов в горизонтальной группе
        /// Проверяем правильность маршрутизации к нужной матрице
        /// </summary>
        private static void TestHorizontalGroupGetSetElements()
        {
            Console.WriteLine("ТЕСТ 3: Чтение и запись элементов");
            Console.WriteLine("---------------------------------");

            // Создаем группу [2x2 | 2x2]
            var m1 = new RegularMatrix(2, 2);
            var m2 = new RegularMatrix(2, 2);

            // Заполняем m1 единицами, m2 - двойками
            FillMatrix(m1, 1.0f);
            FillMatrix(m2, 2.0f);

            var hGroup = new HorizontalMatrixGroup();
            hGroup.AddMatrix(m1);
            hGroup.AddMatrix(m2);

            // Результирующая матрица 2x4:
            // [1 1 | 2 2]
            // [1 1 | 2 2]

            // Проверяем чтение из первой матрицы
            Assert(hGroup.GetElement(0, 0) == 1.0f, "Элемент [0,0] должен быть 1");
            Assert(hGroup.GetElement(0, 1) == 1.0f, "Элемент [0,1] должен быть 1");

            // Проверяем чтение из второй матрицы (столбцы 2-3)
            Assert(hGroup.GetElement(0, 2) == 2.0f, "Элемент [0,2] должен быть 2");
            Assert(hGroup.GetElement(0, 3) == 2.0f, "Элемент [0,3] должен быть 2");

            // Изменяем элемент в первой матрице
            hGroup.SetElement(0, 0, 5.0f);
            Assert(hGroup.GetElement(0, 0) == 5.0f, "Элемент [0,0] должен быть изменен на 5");

            // Изменяем элемент во второй матрице
            hGroup.SetElement(1, 3, 7.0f);
            Assert(hGroup.GetElement(1, 3) == 7.0f, "Элемент [1,3] должен быть изменен на 7");

            Console.WriteLine($"✓ Чтение элементов работает корректно");
            Console.WriteLine($"✓ Запись элементов работает корректно");
            Console.WriteLine($"✓ Маршрутизация по матрицам работает правильно\n");
        }

        /// <summary>
        /// ТЕСТ 4: Вертикальная группа через транспонирование
        /// </summary>
        private static void TestVerticalGroupBasic()
        {
            Console.WriteLine("ТЕСТ 4: Вертикальная группа");
            Console.WriteLine("---------------------------");

            // Создаем три матрицы 2x3
            var m1 = new RegularMatrix(2, 3);
            var m2 = new RegularMatrix(2, 3);
            var m3 = new RegularMatrix(2, 3);

            FillMatrix(m1, 1.0f);
            FillMatrix(m2, 2.0f);
            FillMatrix(m3, 3.0f);

            // Создаем вертикальную группу
            var vGroup = VerticalMatrixGroupHelper.CreateVerticalGroup(m1, m2, m3);

            // Результирующая матрица должна быть (2+2+2) x max(3,3,3) = 6x3
            Assert(vGroup.RowNum == 6, "RowNum должен быть 6 (сумма высот)");
            Assert(vGroup.ColumnNum == 3, "ColumnNum должен быть 3 (максимум ширин)");

            // Проверяем значения
            Assert(vGroup.GetElement(0, 0) == 1.0f, "Первые две строки - из m1");
            Assert(vGroup.GetElement(2, 0) == 2.0f, "Следующие две строки - из m2");
            Assert(vGroup.GetElement(4, 0) == 3.0f, "Последние две строки - из m3");

            Console.WriteLine($"✓ Вертикальная группа из трех матриц 2x3");
            Console.WriteLine($"✓ Результат: {vGroup.RowNum}x{vGroup.ColumnNum}");
            Console.WriteLine($"✓ RowNum = 6 (сумма), ColumnNum = 3 (максимум)\n");
        }

        /// <summary>
        /// ТЕСТ 5: Сложная композиция (вертикальная группа горизонтальных групп)
        /// </summary>
        private static void TestComplexComposite()
        {
            Console.WriteLine("ТЕСТ 5: Сложная композиция");
            Console.WriteLine("--------------------------");

            // Первая горизонтальная группа [2x2 | 2x2] = 2x4
            var h1m1 = new RegularMatrix(2, 2);
            var h1m2 = new RegularMatrix(2, 2);
            FillMatrix(h1m1, 1.0f);
            FillMatrix(h1m2, 2.0f);

            var hGroup1 = new HorizontalMatrixGroup();
            hGroup1.AddMatrix(h1m1);
            hGroup1.AddMatrix(h1m2);

            // Вторая горизонтальная группа [3x2 | 3x2] = 3x4
            var h2m1 = new RegularMatrix(3, 2);
            var h2m2 = new RegularMatrix(3, 2);
            FillMatrix(h2m1, 3.0f);
            FillMatrix(h2m2, 4.0f);

            var hGroup2 = new HorizontalMatrixGroup();
            hGroup2.AddMatrix(h2m1);
            hGroup2.AddMatrix(h2m2);

            // Вертикальная группа из двух горизонтальных
            var vGroup = VerticalMatrixGroupHelper.CreateVerticalGroup(hGroup1, hGroup2);

            // Результат: (2+3) x max(4,4) = 5x4
            Assert(vGroup.RowNum == 5, "RowNum должен быть 5");
            Assert(vGroup.ColumnNum == 4, "ColumnNum должен быть 4");

            // Проверяем значения
            Assert(vGroup.GetElement(0, 0) == 1.0f, "Верхняя левая часть - из h1m1");
            Assert(vGroup.GetElement(0, 2) == 2.0f, "Верхняя правая часть - из h1m2");
            Assert(vGroup.GetElement(2, 0) == 3.0f, "Нижняя левая часть - из h2m1");
            Assert(vGroup.GetElement(2, 2) == 4.0f, "Нижняя правая часть - из h2m2");

            Console.WriteLine($"✓ Композиция: вертикальная группа из 2 горизонтальных групп");
            Console.WriteLine($"✓ hGroup1 (2x4) = [2x2 | 2x2]");
            Console.WriteLine($"✓ hGroup2 (3x4) = [3x2 | 3x2]");
            Console.WriteLine($"✓ Результат: {vGroup.RowNum}x{vGroup.ColumnNum}\n");
        }

        // Вспомогательные методы

        private static void FillMatrix(IMatrix matrix, float value)
        {
            for (int i = 0; i < matrix.RowNum; i++)
            {
                for (int j = 0; j < matrix.ColumnNum; j++)
                {
                    matrix.SetElement(i, j, value);
                }
            }
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"❌ ТЕСТ НЕ ПРОЙДЕН: {message}");
            }
        }
    }
}