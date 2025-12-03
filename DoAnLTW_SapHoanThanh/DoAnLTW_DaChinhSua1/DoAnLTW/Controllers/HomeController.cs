using DoAnLTW.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
namespace DoAnLTW.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<SanPham> _sanPhamCollection;
        private readonly IMongoCollection<DanhMuc> _danhMucCollection;
        private readonly IMongoCollection<KhachHang> _khachHangCollection;
        private readonly IMongoCollection<NhaCungCap> _nhaCungCapCollection;
        private readonly IMongoCollection<HoaDon> _hoaDonCollection;

        public HomeController()
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
            List<SanPham> dssp = _sanPhamCollection.Find(_ => true).ToList();
            return View(dssp);
        }
        public ActionResult DMLoaiSanPham()
        {
            List<DanhMuc> dmpk = _danhMucCollection.Find(_ => true).ToList();
            return PartialView(dmpk);
        }
        public ActionResult PhanLoaiSPTheoDM(string mp)
        {
            List<SanPham> dssp = _sanPhamCollection.Find(t => t.MaDM == mp).ToList();
            return View("Index", dssp);
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                _khachHangCollection.InsertOne(kh);

                return RedirectToAction("DangNhap", "Home");
            }

            return View(kh);
        }

        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string txtName, string txtPass)
        {
            KhachHang kh = _khachHangCollection.Find(k => k.HoTen == txtName && k.MatKhau == txtPass).FirstOrDefault();

            if (kh != null)
            {
                Session["kh"] = kh;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ThongBao = "Email hoặc mật khẩu không đúng!";
            return View();

        }
        public ActionResult TimKiem()
        {

            return View();
        }

        [HttpGet]
        public ActionResult TimKiem(string tuKhoa)
        {
            List<SanPham> dssp = _sanPhamCollection.Find(t => t.TenSP.Contains(tuKhoa)).ToList();

            return View(dssp);
        }

        public ActionResult ChiTietSanPham(string maSP)
        {
            if (string.IsNullOrEmpty(maSP))
                return HttpNotFound();
            var sanPham = _sanPhamCollection.Find(s => s.MaSP == maSP).FirstOrDefault();

            if (sanPham == null)
                return HttpNotFound();
            if (sanPham.MaDM != null)
            {
                sanPham.DanhMuc = _danhMucCollection.Find(d => d.MaDM == sanPham.MaDM).FirstOrDefault();
            }
            if (sanPham.MaNCC != null)
            {
                sanPham.NhaCungCap = _nhaCungCapCollection.Find(n => n.MaNCC == sanPham.MaNCC).FirstOrDefault();
            }

            return View(sanPham);
        }


        //----------------------Giỏ hàng-------------------------------
        //-------------------------------------------------------------
        public List<CartItem> LayGioHang()
        {
            List<CartItem> lstGioHang = Session["GioHang"] as List<CartItem>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<CartItem>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }


        public ActionResult ThemGioHang(string maSP, string url)
        {
            List<CartItem> lstGioHang = LayGioHang();
            CartItem sanPhamTrongGio = lstGioHang.Find(sp => sp.MaSP == maSP);

            if (sanPhamTrongGio == null)
            {
                SanPham sanPhamTuDb = _sanPhamCollection.Find(s => s.MaSP == maSP).FirstOrDefault();
                if (sanPhamTuDb == null)
                {
                    return Redirect(url);
                }
                CartItem itemMoi = new CartItem(sanPhamTuDb);
                lstGioHang.Add(itemMoi);
            }
            else
            {
                sanPhamTrongGio.SoLuong++;
            }

            return Redirect(url);
        }


        public ActionResult XemGioHang()
        {
            List<CartItem> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                ViewBag.ThongBao = "Giỏ hàng trống";
            }
            ViewBag.TongTien = lstGioHang.Sum(item => item.ThanhTien);
            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult CapNhatGioHang(string maSP, int soLuongMoi)
        {
            List<CartItem> lstGioHang = LayGioHang();
            CartItem sanPham = lstGioHang.SingleOrDefault(n => n.MaSP == maSP);
            if (sanPham != null)
            {
                sanPham.SoLuong = soLuongMoi;
            }
            return RedirectToAction("XemGioHang");
        }


        public ActionResult XoaGioHang(string maSP)
        {
            List<CartItem> lstGioHang = LayGioHang();
            CartItem sanPham = lstGioHang.SingleOrDefault(n => n.MaSP == maSP);
            if (sanPham != null)
            {
                lstGioHang.RemoveAll(n => n.MaSP == maSP);
            }
            return RedirectToAction("XemGioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {

            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            KhachHang kh = Session["kh"] as KhachHang;
            string maKH = kh.MaKH;

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<CartItem> lstGioHang = LayGioHang();
            ViewBag.TongTien = lstGioHang.Sum(n => n.ThanhTien);
            ViewBag.TongSoLuong = lstGioHang.Sum(n => n.SoLuong);
            ViewBag.ThongTinKhachHang = kh;

            return View(lstGioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang(string sdtNhan, string diaChiNhan)
        {
            List<CartItem> lstGioHang = LayGioHang();
            KhachHang kh = Session["kh"] as KhachHang;
            string maKH = kh.MaKH;
            HoaDon ddh = new HoaDon();
            ddh.MaKH = maKH;
            ddh.NgayLap = DateTime.Now;
            ddh.TrangThai = "Chưa giao";
            ddh.PhuongThucThanhToan = "Tiền mặt";
            ddh.DiaChiGiaoHang = diaChiNhan;
            ddh.SoDienThoaiGiaoHang = sdtNhan;
            ddh.TongTien = lstGioHang.Sum(n => n.ThanhTien);

            string soNgauNhien = new Random().Next(10000, 99999).ToString();
            ddh.MaHD = "HD" + soNgauNhien.Substring(soNgauNhien.Length - 3);

            ddh.ChiTiet = new List<ChiTietHoaDon>();
            foreach (var item in lstGioHang)
            {
                ChiTietHoaDon cthd = new ChiTietHoaDon();
                cthd.MaSP = item.MaSP;
                cthd.SoLuong = item.SoLuong;
                cthd.DonGia = item.DonGia;
                ddh.ChiTiet.Add(cthd);
            }
            _hoaDonCollection.InsertOne(ddh);

            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "Home");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        //----------------------Giỏ hàng-------------------------------
        //-------------------------------------------------------------


        public List<CartItem> LayGioHangMuaNgay()
        {
            List<CartItem> gioHangMuaNgay = Session["GioHangMuaNgay"] as List<CartItem>;
            if (gioHangMuaNgay == null)
            {
                gioHangMuaNgay = new List<CartItem>();
                Session["GioHangMuaNgay"] = gioHangMuaNgay;
            }
            return gioHangMuaNgay;
        }

        public ActionResult MuaNgay(string maSP)
        {
            if (string.IsNullOrEmpty(maSP))
                return RedirectToAction("Index");
            SanPham sanPhamTuDb = _sanPhamCollection.Find(s => s.MaSP == maSP).FirstOrDefault();

            if (sanPhamTuDb == null)
            {
                return RedirectToAction("Index");
            }

            CartItem spMuaNgay = new CartItem(sanPhamTuDb);
            Session["GioHangMuaNgay"] = null;

            List<CartItem> gioHang = LayGioHangMuaNgay();
            gioHang.Add(spMuaNgay);

            return RedirectToAction("ThanhToanMuaNgay");
        }

        public ActionResult ThanhToanMuaNgay()
        {
            if (Session["kh"] == null)
                return RedirectToAction("DangNhap", "Home");

            List<CartItem> gioHang = LayGioHangMuaNgay();

            if (gioHang.Count == 0)
                return RedirectToAction("Index");

            ViewBag.TongTien = gioHang.Sum(sp => sp.ThanhTien);
            return View(gioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmMuaNgay(string sdtNhan, string diaChiNhan)
        {
            if (Session["kh"] == null)
                return RedirectToAction("DangNhap");

            KhachHang kh = Session["kh"] as KhachHang;
            List<CartItem> gioHang = LayGioHangMuaNgay();
            HoaDon hd = new HoaDon();

            hd.MaHD = "HD" + new Random().Next(10000, 99999).ToString();
            hd.MaKH = kh.MaKH;
            hd.MaNV = "NV01";
            hd.NgayLap = DateTime.Now;
            hd.TrangThai = "Chưa giao";
            hd.PhuongThucThanhToan = "Tiền mặt";
            hd.DiaChiGiaoHang = diaChiNhan;
            hd.SoDienThoaiGiaoHang = sdtNhan;
            hd.TongTien = gioHang.Sum(n => n.ThanhTien);
            hd.ChiTiet = new List<ChiTietHoaDon>();
            foreach (var item in gioHang)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon();
                ct.MaSP = item.MaSP;
                ct.SoLuong = item.SoLuong;
                ct.DonGia = item.DonGia;
                hd.ChiTiet.Add(ct);
            }
            _hoaDonCollection.InsertOne(hd);

            Session["GioHangMuaNgay"] = null;
            return RedirectToAction("XacNhanDonHang");
        }

      

    }
} 