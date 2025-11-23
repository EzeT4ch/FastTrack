namespace Shared.Cores.Response.Products
{
    public class ProductResponse
    {
        public int KioskId { get; set; }

        public string Name { get; set; }

        public string SkuCode { get; set; }

        public int Status { get; set; }

        public DateTime DateAdded { get; set; }

        public int AddedBy { get; set;}

        public int Id { get; set; }

        public DateTime LastUpdate { get; set; }

        public int UpdatedBy { get; set; }
    }
}
