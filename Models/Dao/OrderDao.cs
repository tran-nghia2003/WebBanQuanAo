using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class OrderDao
    {
        OnlineShopDbConext db = null;
        public OrderDao()
        {
            db = new OnlineShopDbConext();
        }

        public Order GetByOrderCode(long orderCode)
        {
            return db.Orders.FirstOrDefault(x => x.ID == orderCode);
        }
    }
}
