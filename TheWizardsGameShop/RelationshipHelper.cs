using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop
{
    public static class RelationshipHelper
    {

        public static int countRequestsReceived(int? id, TheWizardsGameShopContext context)
        {

            return id == null ? 0 : context.Relationship.Include(r => r.SenderNavigation).Where(r => r.Receiver == id && !(bool)r.IsAccepted).Count();
        }

        public static List<Relationship> getRequestsReceived(int? id, TheWizardsGameShopContext context)
        {
            if (id == null) return null;
            var requests = context.Relationship.Include(r => r.SenderNavigation).Where(r => r.Receiver == id && !(bool)r.IsAccepted);
            return requests.ToList();
        }
    }
}
