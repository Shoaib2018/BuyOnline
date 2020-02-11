using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class DiscountRepository : Repository<Discount>
    {
        public DiscountRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    
    }
}