namespace Verdure.Common
{
    public class QueryRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? KeyWord { get; set; }
    }
}
