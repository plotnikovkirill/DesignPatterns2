using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.Matrix;
using DesignPatterns2.Classes.VectorClasses;
using DesignPatterns2.Forms;
using DesignPatterns2.Tests;
using System;
using System.Windows.Forms;

namespace DesignPatterns2
{
    internal static class Programm
    {
        [STAThread]

        static void Main()
        {
			// Запускаем тесты перед GUI
			try
			{
				CompositeMatrixTests.RunAllTests();
				MessageBox.Show(
					"Все тесты пройдены успешно!",
					"Тесты",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Ошибка в тестах:\n{ex.Message}",
					"Тесты",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}


