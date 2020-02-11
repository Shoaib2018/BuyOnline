using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class NotificationRepository : Repository<Notification>
    {
        public NotificationRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    }
}