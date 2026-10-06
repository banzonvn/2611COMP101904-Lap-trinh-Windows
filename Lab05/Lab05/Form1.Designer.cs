namespace Lab05
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpThongTinHocVien = new System.Windows.Forms.GroupBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblSoDienThoai = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.lblNhanEmail = new System.Windows.Forms.Label();
            this.chkNhanEmail = new System.Windows.Forms.CheckBox();
            this.grpThongTinKhoaHoc = new System.Windows.Forms.GroupBox();
            this.lblKhoaHoc = new System.Windows.Forms.Label();
            this.cboKhoaHoc = new System.Windows.Forms.ComboBox();
            this.lblHinhThuc = new System.Windows.Forms.Label();
            this.radOnline = new System.Windows.Forms.RadioButton();
            this.radOffline = new System.Windows.Forms.RadioButton();
            this.lblSoThang = new System.Windows.Forms.Label();
            this.numSoThang = new System.Windows.Forms.NumericUpDown();
            this.lblTongTienTitle = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.btnDangKy = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.grpThongTinHocVien.SuspendLayout();
            this.grpThongTinKhoaHoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoThang)).BeginInit();
            this.SuspendLayout();
            // 
            // grpThongTinHocVien
            // 
            this.grpThongTinHocVien.Controls.Add(this.lblHoTen);
            this.grpThongTinHocVien.Controls.Add(this.txtHoTen);
            this.grpThongTinHocVien.Controls.Add(this.lblSoDienThoai);
            this.grpThongTinHocVien.Controls.Add(this.txtSoDienThoai);
            this.grpThongTinHocVien.Controls.Add(this.lblNgaySinh);
            this.grpThongTinHocVien.Controls.Add(this.dtpNgaySinh);
            this.grpThongTinHocVien.Controls.Add(this.lblNhanEmail);
            this.grpThongTinHocVien.Controls.Add(this.chkNhanEmail);
            this.grpThongTinHocVien.Location = new System.Drawing.Point(16, 15);
            this.grpThongTinHocVien.Name = "grpThongTinHocVien";
            this.grpThongTinHocVien.Size = new System.Drawing.Size(465, 175);
            this.grpThongTinHocVien.TabIndex = 0;
            this.grpThongTinHocVien.TabStop = false;
            this.grpThongTinHocVien.Text = "Thông tin học viên";
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Location = new System.Drawing.Point(18, 30);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(46, 15);
            this.lblHoTen.TabIndex = 0;
            this.lblHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Location = new System.Drawing.Point(125, 27);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(315, 23);
            this.txtHoTen.TabIndex = 1;
            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Location = new System.Drawing.Point(18, 65);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new System.Drawing.Size(79, 15);
            this.lblSoDienThoai.TabIndex = 2;
            this.lblSoDienThoai.Text = "Số điện thoại:";
            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Location = new System.Drawing.Point(125, 62);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(315, 23);
            this.txtSoDienThoai.TabIndex = 3;
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.AutoSize = true;
            this.lblNgaySinh.Location = new System.Drawing.Point(18, 100);
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.Size = new System.Drawing.Size(63, 15);
            this.lblNgaySinh.TabIndex = 4;
            this.lblNgaySinh.Text = "Ngày sinh:";
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySinh.Location = new System.Drawing.Point(125, 97);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(315, 23);
            this.dtpNgaySinh.TabIndex = 5;
            // 
            // lblNhanEmail
            // 
            this.lblNhanEmail.AutoSize = true;
            this.lblNhanEmail.Location = new System.Drawing.Point(18, 136);
            this.lblNhanEmail.Name = "lblNhanEmail";
            this.lblNhanEmail.Size = new System.Drawing.Size(65, 15);
            this.lblNhanEmail.TabIndex = 6;
            this.lblNhanEmail.Text = "Thông báo:";
            // 
            // chkNhanEmail
            // 
            this.chkNhanEmail.AutoSize = true;
            this.chkNhanEmail.Location = new System.Drawing.Point(125, 135);
            this.chkNhanEmail.Name = "chkNhanEmail";
            this.chkNhanEmail.Size = new System.Drawing.Size(147, 19);
            this.chkNhanEmail.TabIndex = 7;
            this.chkNhanEmail.Text = "Nhận email thông báo";
            this.chkNhanEmail.UseVisualStyleBackColor = true;
            // 
            // grpThongTinKhoaHoc
            // 
            this.grpThongTinKhoaHoc.Controls.Add(this.lblKhoaHoc);
            this.grpThongTinKhoaHoc.Controls.Add(this.cboKhoaHoc);
            this.grpThongTinKhoaHoc.Controls.Add(this.lblHinhThuc);
            this.grpThongTinKhoaHoc.Controls.Add(this.radOnline);
            this.grpThongTinKhoaHoc.Controls.Add(this.radOffline);
            this.grpThongTinKhoaHoc.Controls.Add(this.lblSoThang);
            this.grpThongTinKhoaHoc.Controls.Add(this.numSoThang);
            this.grpThongTinKhoaHoc.Controls.Add(this.lblTongTienTitle);
            this.grpThongTinKhoaHoc.Controls.Add(this.lblTongTien);
            this.grpThongTinKhoaHoc.Location = new System.Drawing.Point(16, 200);
            this.grpThongTinKhoaHoc.Name = "grpThongTinKhoaHoc";
            this.grpThongTinKhoaHoc.Size = new System.Drawing.Size(465, 180);
            this.grpThongTinKhoaHoc.TabIndex = 1;
            this.grpThongTinKhoaHoc.TabStop = false;
            this.grpThongTinKhoaHoc.Text = "Thông tin khóa học";
            // 
            // lblKhoaHoc
            // 
            this.lblKhoaHoc.AutoSize = true;
            this.lblKhoaHoc.Location = new System.Drawing.Point(18, 30);
            this.lblKhoaHoc.Name = "lblKhoaHoc";
            this.lblKhoaHoc.Size = new System.Drawing.Size(60, 15);
            this.lblKhoaHoc.TabIndex = 0;
            this.lblKhoaHoc.Text = "Khóa học:";
            // 
            // cboKhoaHoc
            // 
            this.cboKhoaHoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhoaHoc.FormattingEnabled = true;
            this.cboKhoaHoc.Location = new System.Drawing.Point(125, 27);
            this.cboKhoaHoc.Name = "cboKhoaHoc";
            this.cboKhoaHoc.Size = new System.Drawing.Size(315, 23);
            this.cboKhoaHoc.TabIndex = 1;
            this.cboKhoaHoc.SelectedIndexChanged += new System.EventHandler(this.cboKhoaHoc_SelectedIndexChanged);
            // 
            // lblHinhThuc
            // 
            this.lblHinhThuc.AutoSize = true;
            this.lblHinhThuc.Location = new System.Drawing.Point(18, 68);
            this.lblHinhThuc.Name = "lblHinhThuc";
            this.lblHinhThuc.Size = new System.Drawing.Size(62, 15);
            this.lblHinhThuc.TabIndex = 2;
            this.lblHinhThuc.Text = "Hình thức:";
            // 
            // radOnline
            // 
            this.radOnline.AutoSize = true;
            this.radOnline.Checked = true;
            this.radOnline.Location = new System.Drawing.Point(125, 66);
            this.radOnline.Name = "radOnline";
            this.radOnline.Size = new System.Drawing.Size(60, 19);
            this.radOnline.TabIndex = 3;
            this.radOnline.TabStop = true;
            this.radOnline.Text = "Online";
            this.radOnline.UseVisualStyleBackColor = true;
            // 
            // radOffline
            // 
            this.radOffline.AutoSize = true;
            this.radOffline.Location = new System.Drawing.Point(220, 66);
            this.radOffline.Name = "radOffline";
            this.radOffline.Size = new System.Drawing.Size(119, 19);
            this.radOffline.TabIndex = 4;
            this.radOffline.Text = "Offline (Trực tiếp)";
            this.radOffline.UseVisualStyleBackColor = true;
            // 
            // lblSoThang
            // 
            this.lblSoThang.AutoSize = true;
            this.lblSoThang.Location = new System.Drawing.Point(18, 105);
            this.lblSoThang.Name = "lblSoThang";
            this.lblSoThang.Size = new System.Drawing.Size(58, 15);
            this.lblSoThang.TabIndex = 5;
            this.lblSoThang.Text = "Số tháng:";
            // 
            // numSoThang
            // 
            this.numSoThang.Location = new System.Drawing.Point(125, 103);
            this.numSoThang.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numSoThang.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numSoThang.Name = "numSoThang";
            this.numSoThang.Size = new System.Drawing.Size(120, 23);
            this.numSoThang.TabIndex = 6;
            this.numSoThang.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numSoThang.ValueChanged += new System.EventHandler(this.numSoThang_ValueChanged);
            // 
            // lblTongTienTitle
            // 
            this.lblTongTienTitle.AutoSize = true;
            this.lblTongTienTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTongTienTitle.Location = new System.Drawing.Point(18, 145);
            this.lblTongTienTitle.Name = "lblTongTienTitle";
            this.lblTongTienTitle.Size = new System.Drawing.Size(83, 15);
            this.lblTongTienTitle.TabIndex = 7;
            this.lblTongTienTitle.Text = "Tổng học phí:";
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.Red;
            this.lblTongTien.Location = new System.Drawing.Point(125, 142);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(53, 20);
            this.lblTongTien.TabIndex = 8;
            this.lblTongTien.Text = "0 VND";
            // 
            // btnDangKy
            // 
            this.btnDangKy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDangKy.Location = new System.Drawing.Point(105, 395);
            this.btnDangKy.Name = "btnDangKy";
            this.btnDangKy.Size = new System.Drawing.Size(95, 35);
            this.btnDangKy.TabIndex = 2;
            this.btnDangKy.Text = "Đăng ký";
            this.btnDangKy.UseVisualStyleBackColor = true;
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(215, 395);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(95, 35);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(325, 395);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(95, 35);
            this.btnThoat.TabIndex = 4;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 445);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnDangKy);
            this.Controls.Add(this.grpThongTinKhoaHoc);
            this.Controls.Add(this.grpThongTinHocVien);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ĐĂNG KÝ KHÓA HỌC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpThongTinHocVien.ResumeLayout(false);
            this.grpThongTinHocVien.PerformLayout();
            this.grpThongTinKhoaHoc.ResumeLayout(false);
            this.grpThongTinKhoaHoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoThang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpThongTinHocVien;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblSoDienThoai;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblNhanEmail;
        private System.Windows.Forms.CheckBox chkNhanEmail;
        private System.Windows.Forms.GroupBox grpThongTinKhoaHoc;
        private System.Windows.Forms.Label lblKhoaHoc;
        private System.Windows.Forms.ComboBox cboKhoaHoc;
        private System.Windows.Forms.Label lblHinhThuc;
        private System.Windows.Forms.RadioButton radOnline;
        private System.Windows.Forms.RadioButton radOffline;
        private System.Windows.Forms.Label lblSoThang;
        private System.Windows.Forms.NumericUpDown numSoThang;
        private System.Windows.Forms.Label lblTongTienTitle;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Button btnDangKy;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnThoat;
    }
}