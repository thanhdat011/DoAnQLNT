using DoAnQLNT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace DoAnQLNT.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        MydataDataContext db = new MydataDataContext();
        // GET: Admin
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult NoiThat(int? page, string searchString)
        {
            ViewBag.Keyword = searchString;
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(NOITHAT.GetAll(searchString).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập tên mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("NoiThat", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult ThemmoiNoiThat()
        {
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");
            ViewBag.MaNCC = new SelectList(db.NHACCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiNoiThat(NOITHAT nt, HttpPostedFileBase fileupload)
        {
            // Dua du lieu vao dropdownload
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");
            ViewBag.MaNCC = new SelectList(db.NHACCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    // Luu ten fie, luu y bo sung thu vien using System.I0;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        // Luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    nt.Anhbia = fileName;
                    //Luu vao CSDL
                    db.NOITHATs.InsertOnSubmit(nt);
                    db.SubmitChanges();
                }
                return RedirectToAction("NoiThat");
            }
        }
        public ActionResult Chitietsanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNOITHAT == id);
            ViewBag.MaNOITHAT = noithat.MaNOITHAT;
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(noithat);
        }
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNOITHAT == id);
            ViewBag.MaNOITHAT = noithat.MaNOITHAT;
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(noithat);
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNOITHAT == id);
            ViewBag.MaNOITHAT = noithat.MaNOITHAT;
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NOITHATs.DeleteOnSubmit(noithat);
            db.SubmitChanges();
            return RedirectToAction("NoiThat");
        }
        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNOITHAT == id);
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai", noithat.MaTheLoai);
            ViewBag.MaNCC = new SelectList(db.NHACCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", noithat.MaNCC);
            return View(noithat);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(NOITHAT noithat, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai", noithat.MaTheLoai);
            ViewBag.MaNCC = new SelectList(db.NHACCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", noithat.MaNCC);
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);

                    }
                    noithat.Anhbia = fileName;
                    //Luu vao CSDL
                    UpdateModel(noithat);
                    db.SubmitChanges();
                }
                return RedirectToAction("NoiThat");
            }
        }
    }

}
