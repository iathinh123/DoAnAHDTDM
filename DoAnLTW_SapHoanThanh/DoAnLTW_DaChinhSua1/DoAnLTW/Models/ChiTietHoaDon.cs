using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
    public class ChiTietHoaDon
    {
        [BsonElement("maSP")]
        public string MaSP { get; set; }

        [BsonElement("tenSP")]
        public string TenSP { get; set; }

        [BsonElement("soLuong")]
        public int SoLuong { get; set; }

        [BsonElement("donGia")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal DonGia { get; set; }
        [BsonElement("HinhAnh")]
        public string HinhAnh { get; set; }
        [BsonIgnore]
        public SanPham SanPham { get; set; }
    }
}