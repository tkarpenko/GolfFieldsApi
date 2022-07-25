namespace Golf.Fields.Shared
{
    public class MethodResult<T>
    {
        public bool IsSuccess { get; set; }

        public T? Result { get; set; }

        public string? Error { get; set; }
    }
}

