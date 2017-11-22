namespace MVCWareHousingSystem.Migrations
{
    using MVCWarehousingSystem.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCWarehousingSystem.DataAccess.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCWarehousingSystem.DataAccess.StoreContext context)
        {
            context.Items.AddOrUpdate(
                item => item.ArticleNumber,
                    new StockItem { ArticleNumber = 1, Name = "Pizza", Price = 29.99, ShelfPosition = "1D", Quantity = 25, Description = "Frozen Pizza1" },
                    new StockItem { ArticleNumber = 1, Name = "Pizza", Price = 25.99, ShelfPosition = "1E", Quantity = 25, Description = "Frozen Pizza2" }

                );
        }
    }
}
