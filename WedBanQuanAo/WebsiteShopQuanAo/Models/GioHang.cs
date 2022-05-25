using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteShopQuanAo.Models;

namespace WebsiteShopQuanAo.Models
{
    public class GioHang
    {
        ShopQuanAoDataContext db = new ShopQuanAoDataContext();
        public int iMaSP { get; set; }

        public string sTenSP { get; set; }

        public string sSiZe { get; set; }
        public string sAnh { get; set; }

        public int iSoLuong { get; set; }

        public int dDonGia { get; set; }

        public int dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        //Khởi tạo giỏ hàng
        public GioHang(int maSP)
        {
            SANPHAM sp = db.SANPHAMs.Single(s => s.MASP == maSP);
            iMaSP = sp.MASP;
            sTenSP = sp.TENSP;
            sAnh = sp.HINHANH;
            dDonGia = int.Parse(sp.DONGIA.ToString());
            iSoLuong = 1;
        }
    }
}