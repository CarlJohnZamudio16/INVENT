using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;
using Inventory.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext dbContext;

        public ItemController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // List items with sorting functionality
        public async Task<IActionResult> List(string sortOrder)
        {
            var items = await dbContext.Items.ToListAsync();

            // Apply sorting based on the sortOrder parameter
            switch (sortOrder)
            {
                case "name_asc":
                    items = items.OrderBy(i => i.Name).ToList();
                    break;
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name).ToList();
                    break;
                case "quantity_asc":
                    items = items.OrderBy(i => i.Quantity).ToList();
                    break;
                case "quantity_desc":
                    items = items.OrderByDescending(i => i.Quantity).ToList();
                    break;
                default:
                    items = items.OrderBy(i => i.Name).ToList(); // Default: Sort by Name (A to Z)
                    break;
            }

            // Store the selected sort order in ViewData for maintaining the state of the dropdown
            ViewData["SortOrder"] = sortOrder;

            return View(items);
        }

        // Get method to display the add item form
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Post method to create a new item
        [HttpPost]
        public async Task<ActionResult> Add(AddItemViewModel viewModel)
        {
            var item = new Items
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Quantity = viewModel.Quantity,
                Date = viewModel.Date,
                Price = viewModel.Price
            };

            await dbContext.Items.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Get method to edit an existing item
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var item = await dbContext.Items.FindAsync(Id);
            return View(item);
        }

        // Post method to update an item
        [HttpPost]
        public async Task<IActionResult> Edit(Items viewModel)
        {
            var item = await dbContext.Items.FindAsync(viewModel.Id);

            if (item != null)
            {
                item.Name = viewModel.Name;
                item.Description = viewModel.Description;
                item.Quantity = viewModel.Quantity;
                item.Date = viewModel.Date;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Item");
        }

        // Delete an item
        [HttpPost]
        public async Task<IActionResult> Delete(Items viewModel)
        {
            var item = await dbContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if (item != null)
            {
                dbContext.Items.Remove(item);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Item");
        }

        // Report function (not modified here, assuming it's already implemented)
        public IActionResult Report(DateTime? selectedDate)
        {
            var items = dbContext.Items.ToList();

            if (selectedDate.HasValue)
            {
                int selectedYear = selectedDate.Value.Year;
                int selectedMonth = selectedDate.Value.Month;

                items = items
                    .Where(i => i.Date.HasValue && i.Date.Value.Year == selectedYear && i.Date.Value.Month == selectedMonth)
                    .ToList();
            }

            int totalQuantity = items.Select(i => i.Quantity ?? 0).Sum();
            decimal totalPrice = items.Select(i => (i.Quantity ?? 0) * (i.Price ?? 0)).Sum();

            ViewData["TotalQuantity"] = totalQuantity;
            ViewData["TotalPrice"] = totalPrice;
            ViewData["SelectedDate"] = selectedDate?.ToString("yyyy-MM");

            return View(items);
        }

        // Search function
        public async Task<IActionResult> Search(string query)
        {
            var items = await dbContext.Items
                .Where(i => i.Name.Contains(query) || i.Description.Contains(query))
                .ToListAsync();

            return View("List", items); // Render the "List" view with filtered items
        }
    }
}
