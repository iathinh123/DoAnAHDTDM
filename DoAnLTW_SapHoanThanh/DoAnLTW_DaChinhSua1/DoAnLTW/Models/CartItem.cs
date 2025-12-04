using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DoAnLTW.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using DoAnLTW.Models;
namespace DoAnLTW.Models
{
    
    public class CartItem
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }


        public decimal ThanhTien
        {
            get { return SoLuong * DonGia; }
        }
        public CartItem(SanPham sanPham)
        {
            this.MaSP = sanPham.MaSP;
            this.TenSP = sanPham.TenSP;
            this.HinhAnh = sanPham.HinhAnh;
            this.DonGia = sanPham.Gia;
            this.SoLuong = 1;
        }
    }
}