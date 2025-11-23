namespace Shared.Cores.Grid
{
    public class GridResponse<T>
    {
        public GridResponse(int totalRecords, T[] values)
        {
            Values = values;
            TotalRecords = totalRecords;
        }
        public int TotalRecords { get; set; } = 0;
        public T[] Values { get; set; } = Array.Empty<T>();
    }
}
