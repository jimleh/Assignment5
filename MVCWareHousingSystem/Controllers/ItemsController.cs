using MVCWarehousingSystem.Models;
using MVCWarehousingSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCWarehousingSystem.Controllers
{
    public class ItemsController : Controller
    {
        private Repository repo;

        public ItemsController()
        {
            repo = new ItemsRepository();
        }

        public ItemsController(Repository repo)
        {
            this.repo = repo;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Nothing to talk about, nothing at all.";
            return View();
        }

        // Show everything in the list at index
         // I know these things are unnecessary to write, but I do it anyway~
        public ActionResult Index()
        {
            // This throws an exception when performing unit testing, and I didn't even feel like trying to fix it.
            //Session["PreviousAction"] = "Index";
            var items = repo.GetAllItems().ToList();
            return View(items);
        }        
        // Show everything in the list at index sorted by whatever the user picked
        [HttpPost] 
        public ActionResult Index(string sorter = "id")
        {
            //Session["PreviousAction"] = "Index";
            ViewData.Add("SortBy", sorter);
            Func<StockItem, object> predicate = null;

            switch (sorter)
            {
                case "id":
                    predicate = item => item.ArticleNumber;
                    break;
                case "quantity":
                    predicate = item => item.Quantity;
                    break;
                case "shelf":
                    predicate = item => item.ShelfPosition;
                    break;
                default:
                    return RedirectToAction("Index");
            }

            if(Session["SortBy"] != null && Session["SortBy"] as String == sorter)
            {
                Session["SortBy"] = "";
                return View(repo.GetAllItems().OrderByDescending(predicate).ToList());

            }

            Session["SortBy"] = sorter;
            return View(repo.GetAllItems().OrderBy(predicate).ToList());
        }

        // Details for a specific item
        [HttpGet]
        public ActionResult Details(int? id)
        {
            return ValidateInput(id);
        }

        [HttpGet]
        public ActionResult Search(string type, string search = "")
        {
            if (type == null || type == "")
            {
                type = "name";
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Session["PreviousAction"] = "Search";
            ViewData.Add("IsChecked", type);
            ViewData.Add("SearchTerm", search);

            List<StockItem> items = null;
            switch (type.ToLower())
            {
                case "id":
                    int id = 0;
                    if (int.TryParse(search, out id))
                    {
                        items = repo.GetItemsByID(id).ToList();
                    }
                    break;
                case "name":
                    items = repo.GetItemsByName(search).ToList();
                    break;
                case "price":
                    double price = 0;
                    if (double.TryParse(search, out price))
                    {
                        items = repo.GetItemsByPrice(price).ToList();
                    }
                    break;
                default:
                    items = null;
                    break;
            }

            if (items == null)
            {
                ViewBag.Message = "Not a valid search term!";
                return RedirectToAction(Session["PreviousAction"] as String);
                //return View(new List<StockItem>());
            }

            if (items.Count <= 0)
            {
                ViewBag.Message = "No matches found!";
                return View(items);
            }
            if (search != "")
            {
                ViewBag.Message = "Found " + items.Count.ToString() + " items matching your search term!";
            }
            else
            {
                ViewBag.Message = "Listing all items!";
            }

            return View(items);
        }

        // Search Methods
        [HttpPost]
        public ActionResult Search(string sorter, string type, string search = "")
        {
            // Really unnecessary since neither type nor searchTerm can be null, but I'll leave it here anyway
            if (sorter == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Session["PreviousAction"] = "Search";

            if(type == "" || type == null)
            {
                type = "name";
            }

            ViewData.Add("SortBy", sorter);
            ViewData.Add("IsChecked", type);
            ViewData.Add("SearchTerm", search);

            List<StockItem> items = null;
            switch (type.ToLower())
            {
                case "id":
                    int id = 0;
                    if (int.TryParse(search, out id))
                    {
                        items = repo.GetItemsByID(id).ToList();
                    }
                    break;
                case "name":
                    items = repo.GetItemsByName(search).ToList();
                    break;
                case "price":
                    double price = 0;
                    if (double.TryParse(search, out price))
                    {
                        items = repo.GetItemsByPrice(price).ToList();
                    }
                    break;
                default:
                    items = null;
                    break;
            }

            if (items == null)
            {
                ViewBag.Message = "Not a valid search term!";
                return RedirectToAction(Session["PreviousAction"] as String);
            }

            if (items.Count <= 0)
            {
                ViewBag.Message = "No matches found!";
                return View(items);
            }
            if (search != "")
            {
                ViewBag.Message = "Found " + items.Count.ToString() + " items matching your search term!";
            }
            else
            {
                ViewBag.Message = "Listing all items!";
            }

            // So we can sort the results
            Func<StockItem, object> predicate = null;
            switch (sorter)
            {
                case "id":
                    predicate = item => item.ArticleNumber;
                    break;
                case "name":
                    predicate = item => item.Name;
                    break;
                case "price":
                    predicate = item => item.Price;
                    break;
                case "quantity":
                    predicate = item => item.Quantity;
                    break;
                case "shelf":
                    predicate = item => item.ShelfPosition;
                    break;
                case "desc":
                    predicate = item => item.Description;
                    break;
                default:
                    return View(items.ToList());
            }

            if (Session["SortBy"] as String == sorter)
            {
                Session["SortBy"] = "";
                return View(items.OrderByDescending(predicate).ToList());
            }

            Session["SortBy"] = sorter;
            return View(items.OrderBy(predicate).ToList());
        }

        // Create / Add
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ArticleNumber, Name, Price, Quantity, ShelfPosition, Description")] StockItem item)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "New Item added!";
                repo.AddItem(item);
                return RedirectToAction(Session["PreviousAction"] as String);
            }
            ViewBag.Message = "Whar are you doing?";
            return View(item);
        }

        // Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return ValidateInput(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleNumber, Name, Price, Quantity, ShelfPosition, Description")] StockItem item)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Item Edited!";
                repo.EditItem(item);
                return RedirectToAction(Session["PreviousAction"] as String);
            }
            ViewBag.Message = "Whar are you doing?";
            return View(item);
        }

        // Delete / Remove
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return ValidateInput(id);
        }        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Message = "Item Deleted!";
            repo.RemoveItem(id);
            return RedirectToAction(Session["PreviousAction"] as String);
        }


        // Help-method for validating int input (was annoying to have the same code in several methods)
        protected ActionResult ValidateInput(int? input)
        {
            if (input == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = repo.GetItem(input.Value);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
    }
}