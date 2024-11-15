namespace Inventory.Models.Entities
{
    public class Items
    {
        public Guid Id { get; set; }
        required public string Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }  // Change to int
        public decimal? Price { get; set; } // Change to decimal
        public DateTime? Date { get; set; }




    }
}
