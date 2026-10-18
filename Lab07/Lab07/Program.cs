using System;
using System.Windows.Forms;

namespace Lab07
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Mở form đăng nhập trước, nếu OK thì mới mở FrmMain
            var loginForm = new FrmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmMain(loginForm.LoggedInUser));
            }
        }
    }
}
