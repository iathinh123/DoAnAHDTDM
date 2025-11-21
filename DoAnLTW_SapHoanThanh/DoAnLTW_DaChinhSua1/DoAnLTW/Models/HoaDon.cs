using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
    [BsonIgnoreExtraElements]
    public class HoaDon
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("maHD")]
        public string MaHD { get; set; }
        public string MaKH { get; set; }

        [BsonElement("maNV")]
        public string MaNV { get; set; }
        public DateTime NgayLap { get; set; }
        public string TrangThai { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoaiGiaoHang { get; set; }
        public decimal TongTien { get; set; }
        public NhanVien NhanVien { get; set; }
        [BsonIgnore]
        public KhachHang KhachHang { get; set; }

        // Lồng danh sách chi tiết vào đơn hàng
        public List<ChiTietHoaDon> ChiTietDonHang { get; set; }
    }
}