using System.ComponentModel.DataAnnotations;

namespace Shared.Cores.Response.Kiosk
{
    public class KioskResponse
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public int AddedBy { get; set; }

        public DateTime LastUpdate { get; set; }

        public int UpdatedBy { get; set; }
    }
}
