namespace Lab07.Forms;

public partial class FrmAbout : Form
{
    public FrmAbout()
    {
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        SuspendLayout();

        Text = "Giới thiệu ứng dụng - ShopManagerMini";
        ClientSize = new Size(540, 420);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(248, 250, 252); // Slate-50
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = Color.FromArgb(51, 65, 85);

        // Header Panel
        var pnlHeader = new Panel
        {
            Location = new Point(0, 0),
            Size = new Size(540, 85),
            Dock = DockStyle.Top,
            BackColor = Color.White
        };
        pnlHeader.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        };

        var lblAppTitle = new Label
        {
            Text = "SHOP MANAGER MINI v1.0",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(13, 148, 136), // Teal-600
            Location = new Point(25, 16),
            AutoSize = true
        };

        var lblAppDesc = new Label
        {
            Text = "Phần mềm quản lý bán hàng và khách hàng theo kiến trúc MDI WinForms",
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139),
            Location = new Point(27, 48),
            AutoSize = true
        };

        pnlHeader.Controls.AddRange(new Control[] { lblAppTitle, lblAppDesc });

        // Info Card
        var pnlCard = new Panel
        {
            Location = new Point(25, 105),
            Size = new Size(490, 245),
            BackColor = Color.White
        };
        pnlCard.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
        };

        int startY = 18;
        int stepY = 32;

        void AddInfoRow(string label, string value, Color valColor, bool isBold = false)
        {
            var lblCaption = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(20, startY),
                Size = new Size(150, 24)
            };
            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9F, isBold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = valColor,
                Location = new Point(175, startY),
                Size = new Size(300, 24)
            };
            pnlCard.Controls.AddRange(new Control[] { lblCaption, lblValue });
            startY += stepY;
        }

        AddInfoRow("Học phần:", "Lập trình trên Windows (COMP1019)", Color.FromArgb(15, 23, 42));
        AddInfoRow("Giảng viên:", "ThS. Lê Thanh Thoại", Color.FromArgb(15, 23, 42));
        AddInfoRow("Sinh viên thực hiện:", "Lê Hoàng Quân", Color.FromArgb(13, 148, 136), true);
        AddInfoRow("Mã số sinh viên:", "51.01.104.082", Color.FromArgb(15, 23, 42), true);
        AddInfoRow("Lớp - Nhóm:", "51.CNTT.A - Nhóm thực hành 04", Color.FromArgb(15, 23, 42));
        AddInfoRow("Công nghệ:", "C# WinForms, .NET 10.0, MDI Interface", Color.FromArgb(100, 116, 139));
        AddInfoRow("Kho lưu trữ:", "In-memory BindingList<T> đa Form", Color.FromArgb(100, 116, 139));

        // Close Button
        var btnClose = new Button
        {
            Text = "Đóng cửa sổ",
            Location = new Point(365, 365),
            Size = new Size(150, 36),
            BackColor = Color.FromArgb(71, 85, 105), // Slate-600
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            Cursor = Cursors.Hand
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => Close();

        Controls.AddRange(new Control[] { pnlHeader, pnlCard, btnClose });
        CancelButton = btnClose;

        ResumeLayout(false);
    }
}
