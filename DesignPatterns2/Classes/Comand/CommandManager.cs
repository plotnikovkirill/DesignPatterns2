using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Input;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Comand
{
    public sealed class CommandManager
    {
        #region Singleton Implementation

        // Статическое поле для хранения единственного экземпляра
        private static CommandManager? _instance;

        // Объект для синхронизации в многопоточной среде
        private static readonly object _lock = new object();

        /// <summary>
        /// Получить единственный экземпляр CommandManager.
        /// 
        /// THREAD-SAFE SINGLETON:
        /// Использует double-checked locking для потокобезопасности:
        /// 1. Первая проверка без блокировки (быстро)
        /// 2. Блокировка только при создании
        /// 3. Вторая проверка внутри блокировки (безопасно)
        /// </summary>
        public static CommandManager Instance
        {
            get
            {
                // Первая проверка (без блокировки для производительности)
                if (_instance == null)
                {
                    // Блокировка для потокобезопасности
                    lock (_lock)
                    {
                        // Вторая проверка (на случай, если другой поток уже создал экземпляр)
                        if (_instance == null)
                        {
                            _instance = new CommandManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Приватный конструктор предотвращает создание экземпляров извне.
        /// Это ключевая часть паттерна Singleton.
        /// </summary>
        private CommandManager()
        {
            _commandHistory = new List<ICommand>();
            LogInfo("CommandManager инициализирован");
        }

        /// <summary>
        /// Сброс синглтона (полезно для тестирования).
        /// В production коде обычно не используется.
        /// </summary>
        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }

        #endregion

        #region Command History Management

        // История выполненных команд (stack-like behavior)
        private readonly List<ICommand> _commandHistory;

        // Максимальный размер истории (для предотвращения утечек памяти)
        private const int MaxHistorySize = 100;

        /// <summary>
        /// Получить историю команд (только для чтения)
        /// </summary>
        public IReadOnlyList<ICommand> CommandHistory => _commandHistory.AsReadOnly();

        /// <summary>
        /// Количество команд в истории
        /// </summary>
        public int HistoryCount => _commandHistory.Count;

        /// <summary>
        /// Есть ли команды, которые можно отменить
        /// </summary>
        public bool CanUndo => _commandHistory.Any(c => c.CanUndo);
        //public bool CanUndo => false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Зарегистрировать и выполнить команду.
        /// 
        /// АЛГОРИТМ:
        /// 1. Проверить валидность команды
        /// 2. Выполнить команду (Execute)
        /// 3. Добавить в историю
        /// 4. Управление размером истории
        /// 5. Логирование
        /// 
        /// ВАЖНО: Команда выполняется СРАЗУ при регистрации.
        /// Это отличается от некоторых реализаций Command,
        /// где команды добавляются в очередь.
        /// </summary>
        /// <param name="command">Команда для выполнения</param>
        public void RegisterCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command),
                    "Команда не может быть null");
            }

            try
            {
                // Выполняем команду
                command.Execute();

                // Добавляем в историю
                _commandHistory.Add(command);

                // Ограничиваем размер истории
                TrimHistory();

                LogInfo($"Команда зарегистрирована: {command.Description}");
                LogInfo($"Размер истории: {HistoryCount}");
            }
            catch (Exception ex)
            {
                LogError($"Ошибка при регистрации команды '{command.Description}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Отменить последнюю выполненную команду.
        /// 
        /// АЛГОРИТМ:
        /// 1. Найти последнюю команду, которую можно отменить
        /// 2. Вызвать Undo()
        /// 3. Удалить из истории
        /// 4. Логирование
        /// 
        /// ВАЖНО: Отменяется именно последняя ОТМЕНЯЕМАЯ команда.
        /// Неотменяемые команды (например, InitializeApplication) пропускаются.
        /// </summary>
        /// <returns>true если команда была отменена, false если нет команд для отмены</returns>
        public bool UndoLastCommand()
        {
            // Ищем последнюю отменяемую команду
            ICommand? lastUndoableCommand = null;
            int lastUndoableIndex = -1;

            for (int i = _commandHistory.Count - 1; i >= 0; i--)
            {
                if (_commandHistory[i].CanUndo)
                {
                    lastUndoableCommand = _commandHistory[i];
                    lastUndoableIndex = i;
                    break;
                }
            }

            // Если нет команд для отмены
            if (lastUndoableCommand == null)
            {
                LogWarning("Нет команд для отмены");
                return false;
            }

            try
            {
                // Отменяем команду
                lastUndoableCommand.Undo();

                // Удаляем из истории
                _commandHistory.RemoveAt(lastUndoableIndex);

                LogInfo($"Команда отменена: {lastUndoableCommand.Description}");
                LogInfo($"Размер истории: {HistoryCount}");

                return true;
            }
            catch (Exception ex)
            {
                LogError($"Ошибка при отмене команды '{lastUndoableCommand.Description}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Отменить несколько последних команд.
        /// Полезно для множественной отмены (Undo x N).
        /// </summary>
        /// <param name="count">Количество команд для отмены</param>
        /// <returns>Количество фактически отмененных команд</returns>
        public int UndoCommands(int count)
        {
            if (count <= 0)
                return 0;

            int undoneCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (UndoLastCommand())
                {
                    undoneCount++;
                }
                else
                {
                    break; // Больше нечего отменять
                }
            }

            return undoneCount;
        }

        /// <summary>
        /// Очистить всю историю команд.
        /// ВНИМАНИЕ: После этого отмена будет невозможна!
        /// </summary>
        public void ClearHistory()
        {
            int count = _commandHistory.Count;
            _commandHistory.Clear();
            LogInfo($"История очищена. Удалено команд: {count}");
        }

        /// <summary>
        /// Получить информацию о последней команде
        /// </summary>
        public ICommand? GetLastCommand()
        {
            return _commandHistory.LastOrDefault();
        }

        /// <summary>
        /// Получить описание всей истории команд (для отладки)
        /// </summary>
        public string GetHistoryDescription()
        {
            if (_commandHistory.Count == 0)
                return "История команд пуста";

            var descriptions = _commandHistory
                .Select((cmd, index) => $"{index + 1}. [{cmd.ExecutedAt:HH:mm:ss}] {cmd.Description} " +
                                       $"(Can Undo: {cmd.CanUndo})")
                .ToArray();

            return string.Join("\n", descriptions);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Ограничить размер истории команд.
        /// Удаляет самые старые команды, если превышен лимит.
        /// </summary>
        private void TrimHistory()
        {
            if (_commandHistory.Count > MaxHistorySize)
            {
                int removeCount = _commandHistory.Count - MaxHistorySize;
                _commandHistory.RemoveRange(0, removeCount);
                LogWarning($"История урезана. Удалено старых команд: {removeCount}");
            }
        }

        #endregion

        #region Logging

        private void LogInfo(string message)
        {
            Console.WriteLine($"[CommandManager] {message}");
        }

        private void LogWarning(string message)
        {
            Console.WriteLine($"[CommandManager] WARNING: {message}");
        }

        private void LogError(string message)
        {
            Console.WriteLine($"[CommandManager] ERROR: {message}");
        }

        #endregion

        #region Statistics (for debugging)

        /// <summary>
        /// Получить статистику по типам команд
        /// </summary>
        public Dictionary<string, int> GetCommandStatistics()
        {
            return _commandHistory
                .GroupBy(cmd => cmd.GetType().Name)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        #endregion
    }
}
