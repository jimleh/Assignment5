using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.DataAccess
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("DefaultConnection") {}

        public DbSet<Models.StockItem> Items { get; set; }
    }
}