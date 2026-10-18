using System;
using System.Windows.Forms;

namespace Lab06
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmProduct());
        }
    }
}