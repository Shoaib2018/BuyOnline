using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class CartRepository : Repository<Cart>
    {
        public CartRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    
    }
}