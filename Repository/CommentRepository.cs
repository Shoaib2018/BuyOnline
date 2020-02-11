using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyOnline.Models;

namespace BuyOnline.Repository
{
    public class CommentRepository: Repository<Comment>
    {
        public CommentRepository(BuyOnlineEntities entity)
            : base(entity)
        {

        }
    
    }
}