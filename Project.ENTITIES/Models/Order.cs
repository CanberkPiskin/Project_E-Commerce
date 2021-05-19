using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Order:BaseEntity
    {
        public string ShippedAddress { get; set; }
        public decimal TotalPrice { get; set; }
        //Siparis işlemleri içersindeki bilgileri daha rahat yakalabilmek için açtığımız property'ler
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? AppUserID { get; set; }


        //Relational properties
        public virtual AppUser AppUser { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }




    }
}
