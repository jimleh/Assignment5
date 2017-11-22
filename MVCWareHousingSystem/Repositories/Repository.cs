using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCWarehousingSystem.Repositories
{
    public interface Repository
    {
        IEnumerable<StockItem> GetAllItems();
        StockItem GetItem(int id);


        // For searching
        IEnumerable<StockItem> GetItemsByID(int id);
        IEnumerable<StockItem> GetItemsByName(string name);
        IEnumerable<StockItem> GetItemsByPrice(double price);

        // For sorting
        IEnumerable<StockItem> GetItemsSortedByName();

        // Add, Edit, and Remove
        void AddItem(StockItem item);
        void EditItem(StockItem item);
        void RemoveItem(StockItem item);
        void RemoveItem(int id);
    }
}
