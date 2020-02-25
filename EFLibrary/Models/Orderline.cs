using EFLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EFLibrary.Models {
    public class Orderline {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public override string ToString() => $"{Id}|{Quantity}|{ProductId}|{OrderId}";
        public Orderline() { }
    }
}
