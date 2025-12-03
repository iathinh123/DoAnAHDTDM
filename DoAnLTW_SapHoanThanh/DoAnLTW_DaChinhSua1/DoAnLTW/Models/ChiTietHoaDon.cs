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
        [BsonElement("MaSP")]
        public string MaSP { get; set; }

        [BsonElement("TenSP")]
        public string TenSP { get; set; }

        [BsonElement("SoLuong")]
        public int SoLuong { get; set; }

        [BsonElement("DonGia")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal DonGia { get; set; }
        [BsonElement("HinhAnh")]
        public string HinhAnh { get; set; }
        [BsonIgnore]
        public SanPham SanPham { get; set; }
    }
}