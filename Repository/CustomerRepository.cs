using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    }
}