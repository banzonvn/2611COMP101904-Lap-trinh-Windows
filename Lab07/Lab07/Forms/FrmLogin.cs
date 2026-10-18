using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmLogin : Form
    {
        // Danh sách tài khoản mẫu
        private readonly List<User> _users = new List<User>
        {
            new User("admin", "123", "Quản trị viên", "Admin"),
            new User("user",  "123", "Nhân viên bán hàng", "User")
        };

        // Thông tin user sau khi đăng nhập thành công
        public User LoggedInUser { get; private set; } = null!;

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Tìm user khớp username
            User? found = null;
            foreach (var u in _users)
            {
                if (u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    found = u;
                    break;
                }
            }

            if (found == null)
            {
                MessageBox.Show("Tài khoản không tồn tại.", "Đăng nhập thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.SelectAll();
                txtUsername.Focus();
                return;
            }

            if (found.Password != password)
            {
                MessageBox.Show("Mật khẩu không đúng.", "Đăng nhập thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Đăng nhập thành công
            LoggedInUser = found;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Nhấn Enter ở ô username thì chuyển sang password
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtPassword.Focus();
        }

        // Nhấn Enter ở ô password thì đăng nhập luôn
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin_Click(sender, e);
        }
    }
}
