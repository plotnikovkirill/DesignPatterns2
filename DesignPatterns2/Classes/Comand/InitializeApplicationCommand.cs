using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Classes.Comand
{
    public class InitializeApplicationCommand : Comand
    {
        private readonly string _applicationName;
        private readonly string _version;

        /// <summary>
        /// Конструктор команды инициализации
        /// </summary>
        /// <param name="applicationName">Название приложения</param>
        /// <param name="version">Версия приложения</param>
        public InitializeApplicationCommand(string applicationName = "MatrixApp", string version = "1.0")
        {
            _applicationName = applicationName;
            _version = version;
        }

        /// <summary>
        /// Команду инициализации НЕЛЬЗЯ отменить
        /// </summary>
        public override bool CanUndo => false;

        /// <summary>
        /// Описание команды
        /// </summary>
        public override string Description =>
            $"Инициализация приложения '{_applicationName}' v{_version}";

        /// <summary>
        /// Выполнить инициализацию приложения
        /// </summary>
        protected override void DoExecute()
        {
            // Здесь может быть логика инициализации:
            // - Загрузка настроек
            // - Подключение к БД
            // - Проверка лицензии
            // - Загрузка ресурсов

            LogExecution($"Приложение '{_applicationName}' версии {_version} успешно инициализировано");
            LogExecution($"Время запуска: {DateTime.Now}");
            LogExecution($"Платформа: {Environment.OSVersion}");
            LogExecution($"Пользователь: {Environment.UserName}");
        }

        /// <summary>
        /// Отмена не поддерживается
        /// </summary>
        protected override void DoUndo()
        {
            throw new NotSupportedException(
                "Невозможно отменить инициализацию приложения. " +
                "Эта команда не поддерживает отмену."
                );

        }
    }
}
