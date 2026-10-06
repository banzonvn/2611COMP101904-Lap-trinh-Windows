namespace Lab05
{
    public class Course
    {
        public string TenKhoaHoc { get; set; }
        public decimal HocPhiThang { get; set; }

        public Course(string tenKhoaHoc, decimal hocPhiThang)
        {
            TenKhoaHoc = tenKhoaHoc;
            HocPhiThang = hocPhiThang;
        }

        public override string ToString()
        {
            return $"{TenKhoaHoc} ({HocPhiThang:N0} VND/tháng)";
        }
    }
}