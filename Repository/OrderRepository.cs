using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    }
}