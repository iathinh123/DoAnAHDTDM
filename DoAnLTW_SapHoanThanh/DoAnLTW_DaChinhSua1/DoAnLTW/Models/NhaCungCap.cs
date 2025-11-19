using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DoAnLTW.Models
{
    public class NhaCungCap
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("maNCC")]
        public string MaNCC { get; set; }

        [BsonElement("tenNCC")]
        public string TenNCC { get; set; }

        [BsonElement("diaChi")]
        public string DiaChi { get; set; }

        [BsonElement("soDienThoai")]
        public string SoDienThoai { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}