namespace Models.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoanQuanTriVien")]
    public partial class TaiKhoanQuanTriVien
    {
        [Key]
        public int MaTaiKhoan { get; set; }

        [StringLength(50)]
        public string HoTenDem { get; set; }

        [StringLength(50)]
        public string Ten { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string VaiTro { get; set; }

        public bool? TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }
    }
}
