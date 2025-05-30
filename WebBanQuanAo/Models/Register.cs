﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanQuanAo.Models
{
    public class Register
    {
        [Key]
        public long ID { set; get; }
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage ="(*)Yêu cầu nhập tên đăng nhập")]
        public string UserName { set; get; }
        [Display(Name ="Mật khẩu")]
        [StringLength(20,MinimumLength = 6, ErrorMessage = "(*)Độ dài mật khẩu ít nhất 6 ký tự")]
        [Required(ErrorMessage = "(*)Yêu cầu nhập mật khẩu")]
        public string Password { set; get; }
        [Display(Name ="Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "(*)Xác nhật mật khẩu không đúng.")]
        public string ConfirmPassword { set; get; }
        [Display(Name ="Họ tên")]
        [Required(ErrorMessage = "(*)Yêu cầu nhập họ tên")]
        public string Name { set; get; }
        [Display(Name ="Địa chỉ")]
        public string Address { set; get; }
        [Display(Name ="Email")]
        public string Email { set; get; }
        [Display(Name ="Điện thoại")]
        public string Phone { set; get; }

    }
}