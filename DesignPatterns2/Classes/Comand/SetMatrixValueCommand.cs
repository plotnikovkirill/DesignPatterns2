using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Comand
{
    public class SetMatrixValueCommand : Comand
    {
        // Матрица, с которой работаем
        private readonly IMatrix _matrix;

        // Координаты ячейки
        private readonly int _row;
        private readonly int _col;

        // Новое значение для записи
        private readonly float _newValue;

        // Старое значение (для отмены)
        private float _oldValue;

        /// <summary>
        /// Конструктор команды записи значения в матрицу
        /// </summary>
        /// <param name="matrix">Матрица для изменения</param>
        /// <param name="row">Номер строки (0-based)</param>
        /// <param name="col">Номер столбца (0-based)</param>
        /// <param name="newValue">Новое значение</param>
        public SetMatrixValueCommand(IMatrix matrix, int row, int col, float newValue)
        {
            _matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));

            // Валидация координат
            if (row < 0 || row >= matrix.RowNum)
            {
                throw new ArgumentOutOfRangeException(nameof(row),
                    $"Строка должна быть в диапазоне [0, {matrix.RowNum - 1}]");
            }

            if (col < 0 || col >= matrix.ColumnNum)
            {
                throw new ArgumentOutOfRangeException(nameof(col),
                    $"Столбец должен быть в диапазоне [0, {matrix.ColumnNum - 1}]");
            }

            _row = row;
            _col = col;
            _newValue = newValue;
        }

        /// <summary>
        /// Команда ПОДДЕРЖИВАЕТ отмену
        /// </summary>
        public override bool CanUndo => true;

        /// <summary>
        /// Описание команды с деталями
        /// </summary>
        public override string Description =>
            $"Установить значение [{_row},{_col}] = {_newValue:F2} (было: {_oldValue:F2})";

        /// <summary>
        /// Выполнить запись значения в матрицу.
        /// 
        /// АЛГОРИТМ:
        /// 1. Сохранить текущее значение (для отмены)
        /// 2. Записать новое значение
        /// 3. Логирование
        /// </summary>
        protected override void DoExecute()
        {
            // Сохраняем старое значение ДО изменения
            _oldValue = _matrix.GetElement(_row, _col);

            // Записываем новое значение
            _matrix.SetElement(_row, _col, _newValue);

            LogExecution(
                $"Изменена ячейка [{_row},{_col}]: " +
                $"{_oldValue:F2} → {_newValue:F2}"
            );
        }

        /// <summary>
        /// Отменить запись значения в матрицу.
        /// Восстанавливает предыдущее значение.
        /// </summary>
        protected override void DoUndo()
        {
            // Проверяем, что команда была выполнена
            if (!IsExecuted)
            {
                throw new InvalidOperationException(
                    "Невозможно отменить команду, которая не была выполнена"
                );
            }

            // Восстанавливаем старое значение
            _matrix.SetElement(_row, _col, _oldValue);

            LogExecution(
                $"Восстановлена ячейка [{_row},{_col}]: " +
                $"{_newValue:F2} → {_oldValue:F2}"
            );
        }

        /// <summary>
        /// Получить матрицу (для тестирования)
        /// </summary>
        public IMatrix Matrix => _matrix;

        /// <summary>
        /// Получить координаты (для тестирования)
        /// </summary>
        public (int Row, int Col) Position => (_row, _col);

        /// <summary>
        /// Получить значения (для тестирования)
        /// </summary>
        public (float OldValue, float NewValue) Values => (_oldValue, _newValue);
    }
}
