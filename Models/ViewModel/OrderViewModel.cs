using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class OrderViewModel
    {
        public long orderID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public int? TypePayment { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string NameProduct { get; set; }
        public string Image { get; set; }
    }
}
