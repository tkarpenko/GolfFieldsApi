using System;
namespace Golf.Fields.Api
{
    public class SearchResultModel<T>
    {
        public int Total { get; set; }

        public List<T> Result { get; set; } = new List<T>();

        public string? Error { get; set; }
    }
}

