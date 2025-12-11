using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Interfaces
{
    public interface ICommand
    {
        /// <summary>
        /// Выполнить команду.
        /// Этот метод содержит основную логику команды.
        /// </summary>
        void Execute();

        /// <summary>
        /// Отменить выполнение команды.
        /// Возвращает систему в состояние до выполнения Execute().
        /// </summary>
        void Undo();

        /// <summary>
        /// Можно ли отменить эту команду.
        /// Некоторые команды могут не поддерживать отмену
        /// (например, команда инициализации приложения).
        /// </summary>
        public bool CanUndo { get; }

        /// <summary>
        /// Описание команды для логирования и отладки
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Время выполнения команды
        /// </summary>
        DateTime ExecutedAt { get; }
    }
}
