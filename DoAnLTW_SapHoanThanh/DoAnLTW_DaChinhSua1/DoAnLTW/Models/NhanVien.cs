using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
    public class NhanVien
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("maNV")]
        public string MaNV { get; set; }

        [BsonElement("hoTen")]
        public string HoTen { get; set; }

        [BsonElement("chucVu")]
        public string ChucVu { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("matKhau")]
        public string MatKhau { get; set; }
    }
}