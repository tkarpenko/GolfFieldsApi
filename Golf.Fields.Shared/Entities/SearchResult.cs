namespace Golf.Fields.Shared
{
    public class SearchResult<T>
    {
        public int Total { get; set; }

        public List<T> Result { get; set; } = new List<T>();

        public string? Error { get; set; }
    }
}

