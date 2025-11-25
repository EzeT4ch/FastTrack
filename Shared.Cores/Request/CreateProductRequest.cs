namespace Shared.Cores.Request
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int KioskId { get; set; }
        public string SkuCode { get; set; }
        public int InitialStock { get; set; } = 0;
        public int Price { get; set; } = 0;
    }
}
