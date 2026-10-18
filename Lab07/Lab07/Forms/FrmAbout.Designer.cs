namespace Lab07
{
    partial class FrmAbout
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblCourse = new System.Windows.Forms.Label();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.lblLab = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();

            // pnlTop – banner nền tối
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.pnlTop.Controls.Add(this.lblAppName);
            this.pnlTop.Controls.Add(this.lblVersion);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 90;

            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Location = new System.Drawing.Point(0, 15);
            this.lblAppName.Size = new System.Drawing.Size(380, 40);
            this.lblAppName.Text = "ShopManagerMini";
            this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblVersion.Location = new System.Drawing.Point(0, 55);
            this.lblVersion.Size = new System.Drawing.Size(380, 20);
            this.lblVersion.Text = "Phiên bản 1.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Info labels
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDesc.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblDesc.Location = new System.Drawing.Point(30, 110);
            this.lblDesc.Size = new System.Drawing.Size(320, 40);
            this.lblDesc.Text = "Ứng dụng quản lý cửa hàng mini sử dụng\nC# WinForms với giao diện MDI nhiều Form.";

            this.lblCourse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCourse.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblCourse.Location = new System.Drawing.Point(30, 165);
            this.lblCourse.Size = new System.Drawing.Size(320, 20);
            this.lblCourse.Text = "Môn học: COMP1019 – Lập trình Windows";

            this.lblTeacher.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTeacher.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblTeacher.Location = new System.Drawing.Point(30, 190);
            this.lblTeacher.Size = new System.Drawing.Size(320, 20);
            this.lblTeacher.Text = "Giảng viên: ThS. Lê Thanh Thoại";

            this.lblLab.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLab.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblLab.Location = new System.Drawing.Point(30, 215);
            this.lblLab.Size = new System.Drawing.Size(320, 20);
            this.lblLab.Text = "Bài thực hành: Lab 07 – Buổi 7";

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(130, 255);
            this.btnClose.Size = new System.Drawing.Size(120, 32);
            this.btnClose.Text = "Đóng";
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Click += (s, e) => this.Close();

            // FrmAbout
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(380, 310);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblCourse);
            this.Controls.Add(this.lblTeacher);
            this.Controls.Add(this.lblLab);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Giới thiệu";
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.Label lblLab;
        private System.Windows.Forms.Button btnClose;
    }
}
