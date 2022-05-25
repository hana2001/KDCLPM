using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebsiteShopQuanAo.Models
{
    public class TaiKhoanValid
    {
        [Required(ErrorMessage="Enter username")]
        public string UserName { get; set; }

        [RegularExpression("^(?=.*[0-9])(?=.*[a-z]).{8,20}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 1 số, 1 kí tự và có độ dài ít nhất là 8 và tối đa là 20 kí tự")]
        [Required(ErrorMessage = "Vui lòng nhập vào mật khẩu")]
        public string Pass { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào nhập lại mật khẩu")]
        public string RePass { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào họ tên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào ngày tháng năm sinh")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào địa chỉ")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào địa chỉ email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email của bạn không đúng định dạng!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào số điện thoại")]
        [RegularExpression("(84|0[3|5|7|8|9])+([0-9]{8})", ErrorMessage = "Số điện thoại của bạn không đúng định dạng")]
        public string SDT { get; set; }
    }
}