using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnLTW.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
namespace DoAnLTW.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<SanPham> _sanPhamCollection;
        private readonly IMongoCollection<DanhMuc> _danhMucCollection;
        private readonly IMongoCollection<NhaCungCap> _nhaCungCapCollection;
        private readonly IMongoCollection<HoaDon> _hoaDonCollection;
        private readonly IMongoCollection<KhachHang> _khachHangCollection;

        public AdminController()
        {
            string connectionString = ConfigurationManager.AppSettings["MongoConnection"];
            string dbName = ConfigurationManager.AppSettings["Test_KetNoi"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
            _sanPhamCollection = _database.GetCollection<SanPham>("SanPham");
            _danhMucCollection = _database.GetCollection<DanhMuc>("DanhMuc");
            _khachHangCollection = _database.GetCollection<KhachHang>("KhachHang");
            _nhaCungCapCollection = _database.GetCollection<NhaCungCap>("NhaCungCap");
            _hoaDonCollection = _database.GetCollection<HoaDon>("HoaDon");
        }

        public ActionResult Index()
        {
            var sanPhams = _sanPhamCollection.Find(_ => true).ToList();
            var danhMucs = _danhMucCollection.Find(_ => true).ToList().ToDictionary(d => d.MaDM);
            var nhaCungCaps = _nhaCungCapCollection.Find(_ => true).ToList().ToDictionary(n => n.MaNCC);

            foreach (var sp in sanPhams)
            {
                if (sp.MaDM != null && danhMucs.ContainsKey(sp.MaDM))
                    sp.DanhMuc = danhMucs[sp.MaDM];

                if (sp.MaNCC != null && nhaCungCaps.ContainsKey(sp.MaNCC))
                    sp.NhaCungCap = nhaCungCaps[sp.MaNCC];
            }

            return View(sanPhams);
        }

        // --- HÀM TẠO MỚI (CREATE) ---

        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(_danhMucCollection.Find(_ => true).ToList(), "MaDM", "TenDM");
            ViewBag.MaNCC = new SelectList(_nhaCungCapCollection.Find(_ => true).ToList(), "MaNCC", "TenNCC");
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase HinhAnhUpload)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhUpload != null && HinhAnhUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(HinhAnhUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/HinhAnhAdmin"), fileName);
                    HinhAnhUpload.SaveAs(path);
                    sanPham.HinhAnh = fileName;
                }

                _sanPhamCollection.InsertOne(sanPham);
                return RedirectToAction("Index");
            }
            ViewBag.MaDM = new SelectList(_danhMucCollection.Find(_ => true).ToList(), "MaDM", "TenDM", sanPham.MaDM);
            ViewBag.MaNCC = new SelectList(_nhaCungCapCollection.Find(_ => true).ToList(), "MaNCC", "TenNCC", sanPham.MaNCC);
            return View("Add", sanPham);
        }
        // --- HÀM CHỈNH SỬA (EDIT) ---

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = _sanPhamCollection.Find(s => s.MaSP.Trim() == id).FirstOrDefault();

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaDM = new SelectList(_danhMucCollection.Find(_ => true).ToList(), "MaDM", "TenDM", sanPham.MaDM);
            ViewBag.MaNCC = new SelectList(_nhaCungCapCollection.Find(_ => true).ToList(), "MaNCC", "TenNCC", sanPham.MaNCC);
            return View("Sua", sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham, HttpPostedFileBase HinhAnhUpload)
        {
            if (ModelState.IsValid)
            {
                var sanPhamCu = _sanPhamCollection.Find(s => s.MaSP == sanPham.MaSP).FirstOrDefault();
                if (sanPhamCu == null)
                {
                    return HttpNotFound();
                }
                sanPham.Id = sanPhamCu.Id;

                if (HinhAnhUpload != null && HinhAnhUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(HinhAnhUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/HinhAnhAdmin"), fileName);
                    HinhAnhUpload.SaveAs(path);
                    sanPham.HinhAnh = fileName;
                }
                else
                {
                    sanPham.HinhAnh = sanPhamCu.HinhAnh;
                }
                _sanPhamCollection.ReplaceOne(s => s.Id == sanPham.Id, sanPham);

                return RedirectToAction("Index");
            }

            ViewBag.MaDM = new SelectList(_danhMucCollection.Find(_ => true).ToList(), "MaDM", "TenDM", sanPham.MaDM);
            ViewBag.MaNCC = new SelectList(_nhaCungCapCollection.Find(_ => true).ToList(), "MaNCC", "TenNCC", sanPham.MaNCC);
            return View("Sua", sanPham);
        }

        // --- HÀM XÓA (DELETE) ---

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = _sanPhamCollection.Find(s => s.MaSP.Trim() == id).FirstOrDefault();

            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View("Xoa", sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            _sanPhamCollection.DeleteOne(s => s.MaSP.Trim() == id);

            return RedirectToAction("Index");
        }

    public ActionResult DanhSachDonHang()
        {
            var dsHoaDon = _hoaDonCollection.Find(_ => true)
                                       .SortByDescending(h => h.NgayLap)
                                       .ToList();

            var khachHangs = _khachHangCollection.Find(_ => true).ToList().ToDictionary(k => k.MaKH);

            foreach (var hd in dsHoaDon)
            {
                if (hd.MaKH != null && khachHangs.ContainsKey(hd.MaKH))
                {
                    hd.KhachHang = khachHangs[hd.MaKH];
                }
            }
            return View(dsHoaDon);
        }

        public ActionResult CapNhatDonHang(string MaHD)
        {
            if (string.IsNullOrEmpty(MaHD))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Thay đổi: Tìm hóa đơn
            HoaDon hoaDon = _hoaDonCollection.Find(h => h.MaHD == MaHD).FirstOrDefault();
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            var maSPList = hoaDon.ChiTietDonHang.Select(ct => ct.MaSP).ToList();
            var sanPhams = _sanPhamCollection.Find(sp => maSPList.Contains(sp.MaSP))
                                             .ToList().ToDictionary(sp => sp.MaSP);

            foreach (var ct in hoaDon.ChiTietDonHang)
            {
                if (sanPhams.ContainsKey(ct.MaSP))
                {
                    ct.SanPham = sanPhams[ct.MaSP];
                }
            }
            ViewBag.ChiTiet = hoaDon.ChiTietDonHang;

            return View(hoaDon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatDonHang(string MaHD, string TrangThai)
        {
            if (string.IsNullOrEmpty(MaHD))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var filter = Builders<HoaDon>.Filter.Eq(h => h.MaHD, MaHD);
            var update = Builders<HoaDon>.Update.Set(h => h.TrangThai, TrangThai);

            var result = _hoaDonCollection.UpdateOne(filter, update);

            if (result.MatchedCount == 0)
            {
                return HttpNotFound();
            }

            return RedirectToAction("DanhSachDonHang");
        }

        public ActionResult ThongKeDoanhThu()
        {

            var filter = Builders<HoaDon>.Filter.Eq(h => h.TrangThai, "Đã giao");
            var donDaGiao = _hoaDonCollection.Find(filter)
                                            .SortByDescending(h => h.NgayLap)
                                            .ToList();

            decimal tongDoanhThu = donDaGiao.Sum(h => h.TongTien);
            ViewBag.TongDoanhThu = tongDoanhThu;
            var khachHangs = _khachHangCollection.Find(_ => true).ToList().ToDictionary(k => k.MaKH);
            foreach (var hd in donDaGiao)
            {
                if (hd.MaKH != null && khachHangs.ContainsKey(hd.MaKH))
                {
                    hd.KhachHang = khachHangs[hd.MaKH];
                }
            }

            return View(donDaGiao);
        }

        public ActionResult QuanLyHoaDon()
        {
            try
            {
                long soDonChuaGiao = _hoaDonCollection.CountDocuments(h => h.TrangThai == "Chưa giao");
                ViewBag.SoDonChuaGiao = (int)soDonChuaGiao;
            }
            catch (Exception)
            {
                ViewBag.SoDonChuaGiao = 0;
            }

            try
            {
                long tongSoDon = _hoaDonCollection.CountDocuments(_ => true);
                ViewBag.TongSoDon = (int)tongSoDon;
            }
            catch (Exception)
            {
                ViewBag.TongSoDon = 0;
            }

            return View();
        }
    }
}