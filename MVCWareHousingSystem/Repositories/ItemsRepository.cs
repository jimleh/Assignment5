using MVCWarehousingSystem.DataAccess;
using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MVCWarehousingSystem.Repositories
{
    public class ItemsRepository : Repository
    {
        StoreContext context;

        // default constructor
        public ItemsRepository()
        {
            context = new StoreContext();
        }

        // Get all items
        public IEnumerable<StockItem> GetAllItems()
        {
            return context.Items;
        }

        // Get a specific item
        public StockItem GetItem(int id)
        {
            return context.Items.FirstOrDefault(item=> item.ArticleNumber == id);
        }

        // For searching
        public IEnumerable<StockItem> GetItemsByID(int id)
        {
            // Is essentially the same as GetItem, but returns a list that will only have one item in it
            return context.Items.Where(items => items.ArticleNumber.Equals(id)); 
        }
        public IEnumerable<StockItem> GetItemsByName(string name)
        {
            return context.Items.Where(items=> items.Name.Contains(name));
        }
        public IEnumerable<StockItem> GetItemsByPrice(double price)
        {
            return context.Items.Where(items => items.Price.Equals(price));
        }

        // For sorting
        public IEnumerable<StockItem> GetItemsSortedByName()
        {
            return context.Items.OrderBy(item => item.Name);
        }

        // Help-methods
        protected void Save()
        {
            context.SaveChanges();
        }

        // Add, Edit, and Remove
        public void AddItem(StockItem item)
        {
            context.Items.Add(item);
            Save();
        }
        public void EditItem(StockItem item)
        {
            context.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void RemoveItem(StockItem item)
        {
            context.Items.Remove(item);
            Save();
        }
        public void RemoveItem(int id) // An overloaded version, for no particular reason
        {
            context.Items.Remove(GetItem(id));
            Save();
        }
    }
}