namespace Lab07
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCustomer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.stMain = new System.Windows.Forms.StatusStrip();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuMain.SuspendLayout();
            this.stMain.SuspendLayout();
            this.SuspendLayout();

            // mnuMain
            this.mnuMain.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.mnuMain.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnuMain.ForeColor = System.Drawing.Color.White;
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuSystem, this.mnuData, this.mnuHelp });
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1100, 25);
            this.mnuMain.Renderer = new ToolStripProfessionalRenderer(new DarkMenuColorTable());

            // mnuSystem
            this.mnuSystem.Name = "mnuSystem";
            this.mnuSystem.Size = new System.Drawing.Size(70, 21);
            this.mnuSystem.Text = "Hệ thống";
            this.mnuSystem.ForeColor = System.Drawing.Color.White;
            this.mnuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuLogout, this.mnuSep1, this.mnuExit });

            // mnuLogout
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuLogout.Text = "Đăng xuất";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);

            // mnuSep1
            this.mnuSep1.Name = "mnuSep1";

            // mnuExit
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            this.mnuExit.Text = "Thoát";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);

            // mnuData
            this.mnuData.Name = "mnuData";
            this.mnuData.Size = new System.Drawing.Size(66, 21);
            this.mnuData.Text = "Danh mục";
            this.mnuData.ForeColor = System.Drawing.Color.White;
            this.mnuData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuProduct, this.mnuCustomer });

            // mnuProduct
            this.mnuProduct.Name = "mnuProduct";
            this.mnuProduct.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P;
            this.mnuProduct.Text = "Quản lý sản phẩm";
            this.mnuProduct.Click += new System.EventHandler(this.mnuProduct_Click);

            // mnuCustomer
            this.mnuCustomer.Name = "mnuCustomer";
            this.mnuCustomer.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K;
            this.mnuCustomer.Text = "Quản lý khách hàng";
            this.mnuCustomer.Click += new System.EventHandler(this.mnuCustomer_Click);

            // mnuHelp
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(69, 21);
            this.mnuHelp.Text = "Trợ giúp";
            this.mnuHelp.ForeColor = System.Drawing.Color.White;
            this.mnuHelp.DropDownItems.Add(this.mnuAbout);

            // mnuAbout
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Text = "Giới thiệu";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);

            // stMain
            this.stMain.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.stMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.stMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblStatusUser, this.lblStatusRole, this.lblStatusTime });
            this.stMain.Location = new System.Drawing.Point(0, 628);
            this.stMain.Name = "stMain";
            this.stMain.Size = new System.Drawing.Size(1100, 26);

            // lblStatusUser
            this.lblStatusUser.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblStatusUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Text = "  Người dùng: ...";
            this.lblStatusUser.Spring = true;
            this.lblStatusUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblStatusRole
            this.lblStatusRole.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblStatusRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusRole.Name = "lblStatusRole";
            this.lblStatusRole.Text = "Vai trò: ...  ";
            this.lblStatusRole.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;

            // lblStatusTime
            this.lblStatusTime.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblStatusTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusTime.Name = "lblStatusTime";
            this.lblStatusTime.Text = "  00/00/0000  00:00:00  ";
            this.lblStatusTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;

            // FrmMain
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ClientSize = new System.Drawing.Size(1100, 654);
            this.Controls.Add(this.stMain);
            this.Controls.Add(this.mnuMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMain;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShopManagerMini – Quản lý cửa hàng";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.stMain.ResumeLayout(false);
            this.stMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuData;
        private System.Windows.Forms.ToolStripMenuItem mnuProduct;
        private System.Windows.Forms.ToolStripMenuItem mnuCustomer;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.StatusStrip stMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusRole;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusTime;
    }

    // Custom color table để menu nền tối hiển thị đúng
    internal class DarkMenuColorTable : ProfessionalColorTable
    {
        public override System.Drawing.Color MenuItemSelected =>
            System.Drawing.Color.FromArgb(51, 65, 85);
        public override System.Drawing.Color MenuItemBorder =>
            System.Drawing.Color.FromArgb(71, 85, 105);
        public override System.Drawing.Color MenuBorder =>
            System.Drawing.Color.FromArgb(51, 65, 85);
        public override System.Drawing.Color ToolStripDropDownBackground =>
            System.Drawing.Color.FromArgb(30, 41, 59);
        public override System.Drawing.Color MenuItemSelectedGradientBegin =>
            System.Drawing.Color.FromArgb(51, 65, 85);
        public override System.Drawing.Color MenuItemSelectedGradientEnd =>
            System.Drawing.Color.FromArgb(51, 65, 85);
        public override System.Drawing.Color MenuItemPressedGradientBegin =>
            System.Drawing.Color.FromArgb(71, 85, 105);
        public override System.Drawing.Color MenuItemPressedGradientEnd =>
            System.Drawing.Color.FromArgb(71, 85, 105);
        public override System.Drawing.Color ImageMarginGradientBegin =>
            System.Drawing.Color.FromArgb(30, 41, 59);
        public override System.Drawing.Color ImageMarginGradientMiddle =>
            System.Drawing.Color.FromArgb(30, 41, 59);
        public override System.Drawing.Color ImageMarginGradientEnd =>
            System.Drawing.Color.FromArgb(30, 41, 59);
        public override System.Drawing.Color SeparatorDark =>
            System.Drawing.Color.FromArgb(51, 65, 85);
        public override System.Drawing.Color SeparatorLight =>
            System.Drawing.Color.FromArgb(71, 85, 105);
    }
}
