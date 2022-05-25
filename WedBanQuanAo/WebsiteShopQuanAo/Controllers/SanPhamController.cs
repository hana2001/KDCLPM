using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteShopQuanAo.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteShopQuanAo.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /Home/

        ShopQuanAoDataContext db = new ShopQuanAoDataContext();

        
        public ActionResult DanhMucPartial()
        {
            return View(db.LOAISPs.OrderBy(t => t.MALSP));
        }

        public ActionResult SanPhamTheoLoai(string maLoai, int? page)
        {
            var dsloai = db.LOAISPs.SingleOrDefault(t => t.MALSP == maLoai);
            string loai = dsloai.TENLOAI.ToString();
            var listSPTheoLoai = db.SANPHAMs.Where(t => t.MALSP == maLoai).OrderBy(t => t.MASP).ToList();
            @ViewBag.loai = loai;
            return View(listSPTheoLoai.ToPagedList(page ?? 1,9));
        }

        public ActionResult TatCaSanPham(int? page)
        {
            var listSP = db.SANPHAMs.OrderBy(t => t.MASP).ToList();
            return View(listSP.ToPagedList(page ?? 1, 9));
        }

        public ActionResult TatCaSanPhamHome(int? page)
        {
            var listSP = db.SANPHAMs.OrderBy(t => t.MASP).ToList();
            return View(listSP.ToPagedList(page ?? 1, 9));
        }
        public ActionResult XemChiTiet(int maSP)
        {
            return View(db.SANPHAMs.SingleOrDefault(t => t.MASP == maSP));
        }
    }
}
