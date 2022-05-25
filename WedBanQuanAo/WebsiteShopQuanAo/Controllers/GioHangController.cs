using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteShopQuanAo.Models;

namespace WebsiteShopQuanAo.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        ShopQuanAoDataContext db = new ShopQuanAoDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                //nếu lstGioHang  ch tồn tại thì khởi tạo
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult GioHangEmpty()
        {
            return View();
        }
        public ActionResult ThemGioHang(int msp, string strURL)
        {
            //lấy giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //ktra sách có tồn tại trong sesson["GioHang"] chưa?
            GioHang SanPham = lstGioHang.Find(sp => sp.iMaSP == msp);
            if (SanPham == null)
            {
                SanPham = new GioHang(msp);
                lstGioHang.Add(SanPham);
                SanPham.sSiZe = "S";
                return Redirect(strURL);
            }
            else
            {
                SanPham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        public ActionResult ThemGioHangSLL(int msp, string strURL,FormCollection f)
        {
            string strsl = f["txtSoLuong"];
            string size = f["size"];
            //lấy giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //ktra sách có tồn tại trong sesson["GioHang"] chưa?
            GioHang SanPham = lstGioHang.Find(sp => sp.iMaSP == msp);
            if (SanPham == null)
            {
                SanPham = new GioHang(msp);
                lstGioHang.Add(SanPham);
                SanPham.iSoLuong = int.Parse(strsl);
                SanPham.sSiZe = size;
                return Redirect(strURL);
            }
            else
            {
                SanPham.iSoLuong = SanPham.iSoLuong + int.Parse(strsl);
                SanPham.sSiZe = size;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl += lstGioHang.Sum(sp => sp.iSoLuong);
            }
            return tsl;
        }

        //tổng thành tiền
        private double TongThanhTien()
        {
            double ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.dThanhTien);
            }
            return ttt;
        }

        //xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            //if (Session["GioHang"] == null)
            //{
            //    return RedirectToAction("GioHangEmpty","GioHang");
            //}
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("GioHangEmpty", "GioHang");
            }
            else
            { 
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);
            }
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return PartialView();
        }

        public ActionResult XacNhanDonHang()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            else
            {
                List<GioHang> lstGioHang = LayGioHang();
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongThanhTien = TongThanhTien();
                return View(lstGioHang);
            }
        }

        public ActionResult ThanhToanThanhCong()
        {
            return View();
        }

        public ActionResult ThanhToan()
        {
            var us = Session["User"] as TAIKHOAN;
            List<GioHang> listCart = Session["GioHang"] as List<GioHang>;

            //tao hoa don
            HOADON hoadon = new HOADON();
            hoadon.TENTK = us.TENTK;
            hoadon.NGTAO = DateTime.Now;
            hoadon.TONGTIEN = listCart.Sum(t => t.dThanhTien);
            db.HOADONs.InsertOnSubmit(hoadon);
            db.SubmitChanges();
            List<HOADON> lsthd = db.HOADONs.OrderByDescending(t => t.MAHD).ToList();
            var hd_last = lsthd.First();
            int mhd = hd_last.MAHD;


            //them chi tiet
            //copy gio hang vào ct hoa don
            foreach (GioHang item in listCart)
            {
                CHITIETHD ct = new CHITIETHD();
                ct.MAHD = mhd;
                ct.MASP = item.iMaSP;
                ct.SOLUONG = item.iSoLuong;
                //ct.ThanhTien = item.dThanhTien;

                // insert vao database

                db.CHITIETHDs.InsertOnSubmit(ct);

                KHO kho = db.KHOs.SingleOrDefault(s => s.MASP == item.iMaSP && s.SIZE == "M");
                kho.SOLUONG = kho.SOLUONG - item.iSoLuong;
                db.SubmitChanges();
            }

            
            // thanh toan thanh cong
            List<GioHang> list = LayGioHang();
            list.Clear();
            return RedirectToAction("ThanhToanThanhCong", "GioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Single(s => s.iMaSP == MaSP);
            if (sp != null)
            {
                lstGioHang.RemoveAll(s => s.iMaSP == MaSP);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("GioHangEmpty", "GioHang");
                }
                else
                {
                    return RedirectToAction("GioHang", "GioHang");
                }
            }
            return View();

        }


        
        public ActionResult CapNhatGioHang(int MaSP, FormCollection f)
        {
            
            //lấy giỏ hàng
            string strsl = (f["txtSoLuong"]);
            List<GioHang> lstGioHang = LayGioHang();
            KHO spkho = db.KHOs.SingleOrDefault(s =>s.MASP == MaSP && s.SIZE=="M");
            int slton = int.Parse(spkho.SOLUONG.ToString());
            //ktra sách cần cap nhat có trog giỏ hàng ko?
            if (!strsl.All(char.IsDigit))
            {
                ViewData["SoLuong"] = "Số lượng phải là số";

            }
            else if (int.Parse(strsl)<= 0)
            {
                ViewData["SoLuong"] = "Số lượng phải lớn hơn 0";
            }
            else if (int.Parse(strsl) > slton)
            {
                ViewData["SoLuong"] = "Số lượng trong kho không đủ";
            }
            else
            {
                GioHang sp = lstGioHang.Single(s => s.iMaSP == MaSP);
                if (sp != null)
                {
                    sp.iSoLuong = int.Parse(strsl);
                }
            }
            return RedirectToAction("GioHang", "GioHang");
        }
    }

}
