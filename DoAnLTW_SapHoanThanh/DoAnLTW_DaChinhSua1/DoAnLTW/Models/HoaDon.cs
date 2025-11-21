using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
<<<<<<< HEAD
    [BsonIgnoreExtraElements]
    public class HoaDon
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("maHD")]
=======
    public class HoaDon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
>>>>>>> f37319162f63ae1ccdf14949affd1f3c2fa8c7e4
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
<<<<<<< HEAD
        public NhanVien NhanVien { get; set; }
=======

>>>>>>> f37319162f63ae1ccdf14949affd1f3c2fa8c7e4
        [BsonIgnore]
        public KhachHang KhachHang { get; set; }

        // Lồng danh sách chi tiết vào đơn hàng
        public List<ChiTietHoaDon> ChiTietDonHang { get; set; }
    }
}