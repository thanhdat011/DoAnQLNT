using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnQLNT.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnQLNT.Controllers
{
    public class NTStoreController : Controller
    {
        // GET: NTStore
        MydataDataContext data = new MydataDataContext();
        // GET: BookStore

        private List<NOITHAT> Layntmoi(int count)
        {
            //Sap xep giam dan theo Ngaycapnhat, lay count dong dau
            return data.NOITHATs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Keyword = searchString;
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sachmoi = Layntmoi(15);
            //return View(sachmoi.ToPagedList(pageNum, pageSize));
            return View(NOITHAT.GetAll(searchString).ToPagedList(pageNum, pageSize));
        }
        public ActionResult Theloai()
        {
            var theloai = from cd in data.TheLoais select cd;
            return PartialView(theloai);
        }
        public ActionResult Nhacungcap()
        {
            var nhacungcap = from cd in data.NHACCs select cd;
            return PartialView(nhacungcap);
        }
        public ActionResult SPTheochude(int id)
        {
            var noithat = from s in data.NOITHATs where s.MaTheLoai == id select s;
            return View(noithat);
        }
        public ActionResult SPTheoNCC(int id)
        {
            var noithat = from s in data.NOITHATs where s.MaNCC == id select s;
            return View(noithat);
        }
        public ActionResult Details(int id)
        {
            var noithat = from s in data.NOITHATs where s.MaNOITHAT == id select s;
            return View(noithat.Single());
        }
    }
}
