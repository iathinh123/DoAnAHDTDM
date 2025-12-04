using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
    public class SanPham
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("maSP")]
        public string MaSP { get; set; }

        [BsonElement("tenSP")]
        public string TenSP { get; set; }

        [BsonElement("moTa")]
        public string MoTa { get; set; }

        [BsonElement("hinhAnh")]
        public string HinhAnh { get; set; }

        [BsonElement("gia")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Gia { get; set; }

        [BsonElement("soLuong")]
        public int SoLuong { get; set; }

        [BsonElement("tinhTrang")]
        public string TinhTrang { get; set; }
        [BsonElement("maDM")]
        public string MaDM { get; set; }

        [BsonElement("maNCC")]
        public string MaNCC { get; set; }
        [BsonIgnore]
        public DanhMuc DanhMuc { get; set; }
        [BsonIgnore]
        public NhaCungCap NhaCungCap { get; set; }
    }
}