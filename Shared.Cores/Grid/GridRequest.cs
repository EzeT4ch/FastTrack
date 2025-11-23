namespace Shared.Cores.Grid
{
    public class GridRequest
    {
        public GridRequest() { }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "ASC";
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
    }
}
