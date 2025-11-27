using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.MatrixClasses;
using DesignPatterns2.Classes.VectorClasses;
using DesignPatterns2.Forms;
using System;
using System.Windows.Forms;

namespace DesignPatterns2
{
    internal static class Programm
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}


