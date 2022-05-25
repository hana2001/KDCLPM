using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteShopQuanAo.Models;
using System.Text.RegularExpressions;

namespace WebsiteShopQuanAo.Controllers
{
    public class TaiKhoanController : Controller
    {
        //
        // GET: /TaiKhoan/

        ShopQuanAoDataContext db = new ShopQuanAoDataContext();
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //khai báo cac biến nhận gtri tu form f
            var username = f["username"];
            var matkhau = f["pw"];
            if (String.IsNullOrEmpty(username))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(matkhau))
            {

                TAIKHOAN tttk = db.TAIKHOANs.SingleOrDefault(c => c.TENTK == username);
                if (tttk != null)
                {
                    Session["User"] = tttk;
                    Session["tdn"] = tttk.TENTK;
                    return RedirectToAction("TatCaSanPhamHome", "SanPham");

                }
                else
                {
                    Response.Write("<script>alert('Tài Khoản hoặc mật khẩu không đúng !!!')</script>");
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["User"] = null;
            Session["tdn"] = null;
            return RedirectToAction("TatCaSanPhamHome", "SanPham");
        }

        //public bool EmailIsValid(string email)
        //{
        //    string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        //    if (Regex.IsMatch(email, expression))
        //    {
        //        if (Regex.Replace(email, expression, string.Empty).Length == 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKy(TaiKhoanValid tkvl, TAIKHOAN tttk, FormCollection f)
        {
            var username = f["Username"];
            var matkhau = f["Pass"];
            var remathau = f["RePass"];
            var hoten = f["Ten"];
            var email = f["Email"];
            var dienthoai = f["SDT"];
            var gioitinh = f["Gioitinh"];
            var diachi = f["DiaChi"];
            var ngaysinh = f["NgaySinh"];
            
            if(ModelState.IsValid)
            {
                TAIKHOAN ktratk = db.TAIKHOANs.SingleOrDefault(t => t.TENTK == username);
                if (String.Compare(matkhau,remathau)!=0)
                {
                    ViewData["LoiComfirmPass"] = "Nhập lại mật khẩu không đúng";
                    //Response.Write("<script>alert('Mật khẩu nhập lại không đúng!!!')</script>");
                }
                else if (ktratk != null)
                {
                    ViewData["LoiDaTonTaiTenTk"] = "Tên đăng nhập đã tồn tại !";
                }
                else
                {
                    //gán giá trị cho đối tượng kh
                    tttk.TENTK = username;
                    tttk.MATKHAU = matkhau;
                    tttk.HOTEN = hoten;
                    tttk.NGSINH = DateTime.Parse(ngaysinh);
                    tttk.EMAIL = email;
                    tttk.SDT = dienthoai;
                    tttk.DCHI = diachi;
                    tttk.GTINH = gioitinh;
                    tttk.MAQUYEN = 2; // 2 = Nhan Vien , 1 = Quan Ly
                    db.TAIKHOANs.InsertOnSubmit(tttk);
                    db.SubmitChanges();
                    //Response.Write("<script>alert('Đăng Ký Thành Công!!!')</script>");
                    return RedirectToAction("DangNhap", "TaiKhoan");
                }

            }
            return View(tkvl);
        }
    }
}
