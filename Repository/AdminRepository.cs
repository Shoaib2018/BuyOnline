using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class AdminRepository : Repository<Admin>
    {
        public AdminRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    
    }
}