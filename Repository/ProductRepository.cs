using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    }
}