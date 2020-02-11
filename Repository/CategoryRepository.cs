using BuyOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyOnline.Repository
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    }
}