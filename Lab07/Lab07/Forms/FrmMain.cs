using System;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmMain : Form
    {
        private readonly User _currentUser;
        private System.Windows.Forms.Timer _clockTimer = new System.Windows.Forms.Timer();

        public FrmMain(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng lên StatusStrip
            lblStatusUser.Text = $"  Người dùng: {_currentUser.FullName}";
            lblStatusRole.Text = $"Vai trò: {_currentUser.Role}  ";
            UpdateClock();

            // Timer cập nhật giờ mỗi giây
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += (s, ev) => UpdateClock();
            _clockTimer.Start();
        }

        private void UpdateClock()
        {
            lblStatusTime.Text = $"  {DateTime.Now:dd/MM/yyyy  HH:mm:ss}  ";
        }

        // Mở form con MDI – nếu đã mở thì kích hoạt lại, không mở trùng
        private void OpenChildForm<T>() where T : Form, new()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.Activate();
                    return;
                }
            }
            T form = new T();
            form.MdiParent = this;
            form.Show();
        }

        // Menu: Danh mục → Quản lý sản phẩm
        private void mnuProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmProduct>();
        }

        // Menu: Danh mục → Quản lý khách hàng
        private void mnuCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmCustomer>();
        }

        // Menu: Trợ giúp → Giới thiệu
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            var about = new FrmAbout();
            about.ShowDialog();
        }

        // Menu: Hệ thống → Đăng xuất
        private void mnuLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có muốn đăng xuất không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _clockTimer.Stop();
                this.Close();
                // Mở lại form đăng nhập
                var loginForm = new FrmLogin();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    var mainForm = new FrmMain(loginForm.LoggedInUser);
                    mainForm.Show();
                }
            }
        }

        // Menu: Hệ thống → Thoát
        private void mnuExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn thoát chương trình?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _clockTimer.Stop();
            base.OnFormClosed(e);
        }
    }
}
