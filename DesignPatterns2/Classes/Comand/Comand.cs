using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Comand
{
    public abstract class Comand : ICommand
    {
        private bool _isExecuted = false;

        /// <summary>
        /// Время выполнения команды
        /// </summary>
        public DateTime ExecutedAt { get; private set; }

        /// <summary>
        /// Можно ли отменить эту команду.
        /// По умолчанию - true. Переопределите для команд без отмены.
        /// </summary>
        public virtual bool CanUndo => true;

        /// <summary>
        /// Описание команды. Должно быть переопределено в наследниках.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Выполнить команду.
        /// Использует паттерн Template Method:
        /// 1. Проверяет, не выполнена ли команда уже
        /// 2. Вызывает DoExecute() (реализуется в наследниках)
        /// 3. Логирует выполнение
        /// 4. Запоминает время выполнения
        /// </summary>
        public void Execute()
        {
            if (_isExecuted)
            {
                throw new InvalidOperationException(
                    $"Команда '{Description}' уже была выполнена. " +
                    "Для повторного выполнения создайте новый экземпляр команды."
                );
            }

            try
            {
                ExecutedAt = DateTime.Now;
                DoExecute();
                _isExecuted = true;

                // Логирование (можно отключить в production)
                LogExecution($"✓ Выполнена: {Description}");
            }
            catch (Exception ex)
            {
                LogError($"✗ Ошибка при выполнении '{Description}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Отменить выполнение команды.
        /// Использует паттерн Template Method:
        /// 1. Проверяет, была ли команда выполнена
        /// 2. Проверяет, можно ли отменить команду
        /// 3. Вызывает DoUndo() (реализуется в наследниках)
        /// 4. Логирует отмену
        /// </summary>
        public void Undo()
        {
            if (!_isExecuted)
            {
                throw new InvalidOperationException(
                    $"Нельзя отменить команду '{Description}', которая не была выполнена."
                );
            }

            if (!CanUndo)
            {
                throw new InvalidOperationException(
                    $"Команда '{Description}' не поддерживает отмену."
                );
            }

            try
            {
                DoUndo();
                _isExecuted = false;

                LogExecution($"↶ Отменена: {Description}");
            }
            catch (Exception ex)
            {
                LogError($"✗ Ошибка при отмене '{Description}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Основная логика выполнения команды.
        /// Должна быть реализована в конкретных командах.
        /// </summary>
        protected abstract void DoExecute();

        /// <summary>
        /// Логика отмены команды.
        /// Должна быть реализована в конкретных командах.
        /// По умолчанию выбрасывает исключение.
        /// </summary>
        protected virtual void DoUndo()
        {
            throw new NotImplementedException(
                $"Команда '{Description}' не реализует метод отмены."
            );
        }

        /// <summary>
        /// Логирование выполнения команды.
        /// В production можно заменить на настоящий логгер (NLog, Serilog и т.д.)
        /// </summary>
        protected virtual void LogExecution(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        /// <summary>
        /// Логирование ошибок
        /// </summary>
        protected virtual void LogError(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERROR: {message}");
        }

        /// <summary>
        /// Флаг, указывающий была ли выполнена команда
        /// </summary>
        protected bool IsExecuted => _isExecuted;
    }
}
