using System.ComponentModel;
using Lab07.Data;
using Lab07.Models;

namespace Lab07.Forms;

public partial class FrmLogin : Form
{
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Panel pnlCard = null!;
    private Label lblUsername = null!;
    private TextBox txtUsername = null!;
    private Label lblPassword = null!;
    private TextBox txtPassword = null!;
    private CheckBox chkShowPassword = null!;
    private Button btnLogin = null!;
    private Button btnExit = null!;
    private Panel pnlAlert = null!;
    private Label lblAlert = null!;
    private Label lblHint = null!;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public User? CurrentUser { get; private set; }

    public FrmLogin()
    {
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        SuspendLayout();

        Text = "Đăng nhập hệ thống - ShopManagerMini";
        ClientSize = new Size(460, 480);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(248, 250, 252); // Slate-50 neutral
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = Color.FromArgb(51, 65, 85);

        // 1. Header panel
        pnlHeader = new Panel
        {
            Location = new Point(0, 0),
            Size = new Size(460, 95),
            Dock = DockStyle.Top,
            BackColor = Color.White
        };
        pnlHeader.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        };

        lblTitle = new Label
        {
            Text = "SHOP MANAGER MINI",
            Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(13, 148, 136), // Teal-600
            Location = new Point(0, 18),
            Size = new Size(460, 32),
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblSubtitle = new Label
        {
            Text = "Hệ thống quản lý cửa hàng tiện lợi đa màn hình (MDI)",
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139),
            Location = new Point(0, 52),
            Size = new Size(460, 22),
            TextAlign = ContentAlignment.MiddleCenter
        };

        pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSubtitle });

        // 2. Card Body
        pnlCard = new Panel
        {
            Location = new Point(30, 115),
            Size = new Size(400, 335),
            BackColor = Color.White
        };
        pnlCard.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
        };

        // Alert message banner
        pnlAlert = new Panel
        {
            Location = new Point(20, 15),
            Size = new Size(360, 34),
            BackColor = Color.FromArgb(254, 242, 242), // Soft Rose
            Visible = false
        };
        pnlAlert.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(254, 205, 211), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, pnlAlert.Width - 1, pnlAlert.Height - 1);
        };

        lblAlert = new Label
        {
            Text = "Thông báo cảnh báo lỗi tại đây",
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(225, 29, 72),
            Location = new Point(10, 4),
            Size = new Size(340, 26),
            TextAlign = ContentAlignment.MiddleLeft
        };
        pnlAlert.Controls.Add(lblAlert);

        // Username
        lblUsername = new Label
        {
            Text = "Tên đăng nhập:",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(51, 65, 85),
            Location = new Point(20, 58),
            AutoSize = true
        };

        txtUsername = new TextBox
        {
            Location = new Point(20, 82),
            Size = new Size(360, 28),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
            Text = "admin"
        };
        txtUsername.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtPassword.Focus();
                txtPassword.SelectAll();
            }
        };

        // Password
        lblPassword = new Label
        {
            Text = "Mật khẩu:",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(51, 65, 85),
            Location = new Point(20, 122),
            AutoSize = true
        };

        txtPassword = new TextBox
        {
            Location = new Point(20, 146),
            Size = new Size(360, 28),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
            UseSystemPasswordChar = true,
            Text = "123"
        };
        txtPassword.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                PerformLogin();
            }
        };

        // Show password checkbox
        chkShowPassword = new CheckBox
        {
            Text = "Hiển thị mật khẩu",
            Location = new Point(20, 182),
            AutoSize = true,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139)
        };
        chkShowPassword.CheckedChanged += (s, e) =>
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        };

        // Buttons
        btnLogin = new Button
        {
            Text = "Đăng nhập",
            Location = new Point(20, 218),
            Size = new Size(172, 38),
            BackColor = Color.FromArgb(13, 148, 136), // Sage/Teal-600
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
            Cursor = Cursors.Hand
        };
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.Click += (s, e) => PerformLogin();

        btnExit = new Button
        {
            Text = "Thoát",
            Location = new Point(208, 218),
            Size = new Size(172, 38),
            BackColor = Color.FromArgb(100, 116, 139), // Slate-500
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
            Cursor = Cursors.Hand
        };
        btnExit.FlatAppearance.BorderSize = 0;
        btnExit.Click += (s, e) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        // Demo account hints
        lblHint = new Label
        {
            Text = "Tài khoản mẫu: admin / 123 (Admin) | user / 123 (Nhân viên)",
            Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(148, 163, 184),
            Location = new Point(10, 275),
            Size = new Size(380, 45),
            TextAlign = ContentAlignment.TopCenter
        };

        pnlCard.Controls.AddRange(new Control[]
        {
            pnlAlert, lblUsername, txtUsername, lblPassword, txtPassword,
            chkShowPassword, btnLogin, btnExit, lblHint
        });

        Controls.AddRange(new Control[] { pnlCard, pnlHeader });

        AcceptButton = btnLogin;
        CancelButton = btnExit;

        ResumeLayout(false);
    }

    public void SetCredentials(string username, string password)
    {
        txtUsername.Text = username;
        txtPassword.Text = password;
    }

    public bool PerformLogin(bool showMsgBox = false)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();

        // Kiểm tra bỏ trống
        if (string.IsNullOrEmpty(username))
        {
            ShowAlert("Vui lòng nhập tên đăng nhập!");
            if (showMsgBox)
            {
                MessageBox.Show("Tên đăng nhập không được để trống!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            txtUsername.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowAlert("Vui lòng nhập mật khẩu!");
            if (showMsgBox)
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            txtPassword.Focus();
            return false;
        }

        // Xác thực người dùng qua ShopDataStore
        var user = ShopDataStore.Authenticate(username, password);
        if (user != null)
        {
            CurrentUser = user;
            DialogResult = DialogResult.OK;
            Close();
            return true;
        }
        else
        {
            ShowAlert("Tên đăng nhập hoặc mật khẩu không chính xác!");
            if (showMsgBox)
            {
                MessageBox.Show("Đăng nhập thất bại: Sai tên đăng nhập hoặc mật khẩu!",
                    "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtPassword.SelectAll();
            txtPassword.Focus();
            return false;
        }
    }

    public void ShowAlert(string message)
    {
        lblAlert.Text = message;
        pnlAlert.Visible = true;
    }

    public void ClearAlert()
    {
        pnlAlert.Visible = false;
    }
}
