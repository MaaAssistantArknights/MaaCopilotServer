namespace MaaCopilot.DataTransferObjects
{
    public class SearchResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public SearchResult(IEnumerable<T> data = null, int totalRecords = 0)
        {
            if (data == null)
            {
                Items = new List<T>();
            }
            else
            {
                Items = data;
                TotalRecords = totalRecords; 
            }
        }
    }
}