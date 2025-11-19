using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
    public class KhachHang
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Map với _id của MongoDB

        [BsonElement("maKH")]
        public string MaKH { get; set; }

        [BsonElement("hoTen")]
        public string HoTen { get; set; }

        [BsonElement("gioiTinh")]
        public string GioiTinh { get; set; }

        [BsonElement("diaChi")]
        public string DiaChi { get; set; }

        [BsonElement("dienThoai")]
        public string DienThoai { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("matKhau")]
        public string MatKhau { get; set; }
    }
}