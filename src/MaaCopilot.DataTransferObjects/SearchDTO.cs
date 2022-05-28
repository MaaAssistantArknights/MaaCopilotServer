namespace MaaCopilot.DataTransferObjects
{
    public class SearchDTO
    {
        public string Query { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public string OrderBy { get; set; } = null;
        public bool OrderByAsending { get; set; } 

    }

}