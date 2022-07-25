namespace Golf.Fields.Shared
{
    public class SearchFilter
    {
        public int Skip { get; set; }

        public int Limit { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }
    }
}

