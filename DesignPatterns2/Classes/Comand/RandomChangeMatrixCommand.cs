using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Comand
{
    public class RandomChangeMatrixCommand : Comand
    {
        private readonly IMatrix _matrix;
        private readonly int _changesCount;
        private readonly int _maxValue;
        private readonly Random _random;

        // Список подкоманд для отмены
        private readonly List<SetMatrixValueCommand> _subCommands;

        /// <summary>
        /// Конструктор команды случайного изменения
        /// </summary>
        /// <param name="matrix">Матрица для изменения</param>
        /// <param name="changesCount">Количество ячеек для изменения</param>
        /// <param name="maxValue">Максимальное значение для генерации</param>
        public RandomChangeMatrixCommand(IMatrix matrix, int changesCount = 5, int maxValue = 9)
        {
            _matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));
            _changesCount = Math.Max(1, Math.Min(changesCount, matrix.RowNum * matrix.ColumnNum));
            _maxValue = maxValue;
            _random = new Random();
            _subCommands = new List<SetMatrixValueCommand>();
        }

        public override bool CanUndo => true;

        public override string Description =>
            $"Случайное изменение {_changesCount} ячеек (макс. значение: {_maxValue})";

        protected override void DoExecute()
        {
            // Генерируем уникальные координаты
            var positions = new HashSet<(int, int)>();

            while (positions.Count < _changesCount)
            {
                int row = _random.Next(_matrix.RowNum);
                int col = _random.Next(_matrix.ColumnNum);
                positions.Add((row, col));
            }

            // Создаем и выполняем команды для каждой ячейки
            foreach (var (row, col) in positions)
            {
                float newValue = (float)_random.Next(0, _maxValue + 1);
                var cmd = new SetMatrixValueCommand(_matrix, row, col, newValue);

                // Выполняем подкоманду напрямую (не через CommandManager)
                cmd.Execute();
                _subCommands.Add(cmd);
            }

            LogExecution($"Изменено {_subCommands.Count} ячеек случайным образом");
        }

        protected override void DoUndo()
        {
            // Отменяем в обратном порядке
            for (int i = _subCommands.Count - 1; i >= 0; i--)
            {
                _subCommands[i].Undo();
            }

            _subCommands.Clear();
            LogExecution($"Отменены все случайные изменения");
        }
    }
}
