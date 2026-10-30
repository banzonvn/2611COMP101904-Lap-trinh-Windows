using System.ComponentModel;
using Lab07.Models;

namespace Lab07.Forms;

public partial class FrmMain : Form
{
    private readonly User _currentUser;
    private readonly System.Windows.Forms.Timer _clockTimer = new();

    // MenuStrip controls
    private MenuStrip menuStrip = null!;
    private ToolStripMenuItem mnuSystem = null!;
    private ToolStripMenuItem mnuLogout = null!;
    private ToolStripMenuItem mnuExit = null!;
    private ToolStripMenuItem mnuCatalog = null!;
    private ToolStripMenuItem mnuProduct = null!;
    private ToolStripMenuItem mnuCustomer = null!;
    private ToolStripMenuItem mnuHelp = null!;
    private ToolStripMenuItem mnuAbout = null!;

    // StatusStrip controls
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel lblStatusUser = null!;
    private ToolStripStatusLabel lblStatusRole = null!;
    private ToolStripStatusLabel lblStatusApp = null!;
    private ToolStripStatusLabel lblStatusTime = null!;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public User CurrentUser => _currentUser;

    public FrmMain(User user)
    {
        _currentUser = user;
        InitializeCustomComponents();
        ApplyCustomMdiTheme();
    }

    private void InitializeCustomComponents()
    {
        SuspendLayout();

        Text = "ShopManagerMini - Hệ thống quản lý cửa hàng đa màn hình MDI";
        ClientSize = new Size(1200, 780);
        MinimumSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        IsMdiContainer = true;
        BackColor = Color.FromArgb(241, 245, 249); // Slate-100
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

        // 1. MenuStrip
        menuStrip = new MenuStrip
        {
            BackColor = Color.White,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            RenderMode = ToolStripRenderMode.System,
            Dock = DockStyle.Top
        };

        // --- Menu Hệ thống ---
        mnuSystem = new ToolStripMenuItem("Hệ thống (&S)");
        mnuLogout = new ToolStripMenuItem("Đăng xuất (&L)", null, (s, e) => Logout()) { ShortcutKeys = Keys.F5 };
        mnuExit = new ToolStripMenuItem("Thoát (&X)", null, (s, e) => ConfirmAndExit()) { ShortcutKeys = Keys.Alt | Keys.F4 };
        mnuSystem.DropDownItems.AddRange(new ToolStripItem[] { mnuLogout, new ToolStripSeparator(), mnuExit });

        // --- Menu Danh mục ---
        mnuCatalog = new ToolStripMenuItem("Danh mục (&C)");
        mnuProduct = new ToolStripMenuItem("Quản lý sản phẩm (&P)", null, (s, e) => OpenChildForm<FrmProduct>()) { ShortcutKeys = Keys.Control | Keys.P };
        mnuCustomer = new ToolStripMenuItem("Quản lý khách hàng (&C)", null, (s, e) => OpenChildForm<FrmCustomer>()) { ShortcutKeys = Keys.Control | Keys.K };
        mnuCatalog.DropDownItems.AddRange(new ToolStripItem[] { mnuProduct, mnuCustomer });

        // --- Menu Trợ giúp ---
        mnuHelp = new ToolStripMenuItem("Trợ giúp (&H)");
        mnuAbout = new ToolStripMenuItem("Giới thiệu ứng dụng (&A)", null, (s, e) => OpenAbout()) { ShortcutKeys = Keys.F1 };
        mnuHelp.DropDownItems.Add(mnuAbout);

        menuStrip.Items.AddRange(new ToolStripItem[] { mnuSystem, mnuCatalog, mnuHelp });

        // 2. StatusStrip
        statusStrip = new StatusStrip
        {
            BackColor = Color.White,
            Dock = DockStyle.Bottom,
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        };

        lblStatusUser = new ToolStripStatusLabel
        {
            Text = $"Người dùng: {_currentUser.FullName}",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = false,
            Width = 240,
            TextAlign = ContentAlignment.MiddleLeft
        };

        lblStatusRole = new ToolStripStatusLabel
        {
            Text = $"Vai trò: {_currentUser.Role}",
            BorderSides = ToolStripStatusLabelBorderSides.Left,
            BorderStyle = Border3DStyle.Etched,
            ForeColor = _currentUser.IsAdmin ? Color.FromArgb(13, 148, 136) : Color.FromArgb(71, 85, 105),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            AutoSize = false,
            Width = 160,
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblStatusApp = new ToolStripStatusLabel
        {
            Text = "ShopManagerMini v1.0 | COMP1019 - Buổi 7",
            BorderSides = ToolStripStatusLabelBorderSides.Left,
            BorderStyle = Border3DStyle.Etched,
            ForeColor = Color.FromArgb(100, 116, 139),
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft
        };

        lblStatusTime = new ToolStripStatusLabel
        {
            Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss"),
            BorderSides = ToolStripStatusLabelBorderSides.Left,
            BorderStyle = Border3DStyle.Etched,
            ForeColor = Color.FromArgb(71, 85, 105),
            AutoSize = false,
            Width = 200,
            TextAlign = ContentAlignment.MiddleCenter
        };

        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusUser, lblStatusRole, lblStatusApp, lblStatusTime });

        // 3. Clock Timer
        _clockTimer.Interval = 1000;
        _clockTimer.Tick += (s, e) =>
        {
            lblStatusTime.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
        };
        _clockTimer.Start();

        // Form events
        FormClosing += FrmMain_FormClosing;

        Controls.Add(menuStrip);
        Controls.Add(statusStrip);
        MainMenuStrip = menuStrip;

        ResumeLayout(false);
        PerformLayout();
    }

    private void ApplyCustomMdiTheme()
    {
        // Duyệt tìm MdiClient control để đổi màu nền sang xám sáng hài hòa (thay vì xám đậm mặc định của Windows)
        foreach (Control c in Controls)
        {
            if (c is MdiClient mdiClient)
            {
                mdiClient.BackColor = Color.FromArgb(241, 245, 249); // Slate-100
                break;
            }
        }
    }

    /// <summary>
    /// Phương thức mở Form con MDI đảm bảo KHÔNG MỞ TRÙNG FORM CÙNG LOẠI.
    /// Nếu Form đã mở -> Activate; Nếu chưa mở -> Tạo mới và Show.
    /// </summary>
    public T OpenChildForm<T>() where T : Form, new()
    {
        foreach (Form form in MdiChildren)
        {
            if (form is T existingForm)
            {
                if (existingForm.WindowState == FormWindowState.Minimized)
                {
                    existingForm.WindowState = FormWindowState.Normal;
                }
                if (!existingForm.Visible)
                {
                    existingForm.Visible = true;
                }
                existingForm.BringToFront();
                existingForm.Activate();
                return existingForm;
            }
        }

        T child = new T
        {
            MdiParent = this,
            StartPosition = FormStartPosition.CenterScreen
        };
        child.Show();
        child.BringToFront();
        child.Activate();
        return child;
    }

    public void OpenAbout()
    {
        using var frm = new FrmAbout();
        frm.ShowDialog(this);
    }

    public void Logout()
    {
        var result = MessageBox.Show(
            "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống không?",
            "Xác nhận đăng xuất",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            _clockTimer.Stop();
            Close();
        }
    }

    private void ConfirmAndExit()
    {
        Close();
    }

    private void FrmMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // Nếu người dùng nhấn X trên cửa sổ chính, hỏi xác nhận thoát
        if (e.CloseReason == CloseReason.UserClosing)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát khỏi ứng dụng ShopManagerMini?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                _clockTimer.Stop();
            }
        }
    }

    // Methods hỗ trợ automation và screenshot testing
    public FrmProduct OpenProductManagement() => OpenChildForm<FrmProduct>();
    public FrmCustomer OpenCustomerManagement() => OpenChildForm<FrmCustomer>();
}
