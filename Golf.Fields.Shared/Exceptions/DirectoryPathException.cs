namespace Golf.Fields.Shared
{
    [Serializable]
    public class DirectoryPathException : Exception
    {
        public DirectoryPathException()
        {

        }

        public DirectoryPathException(string message) : base(message)
        {

        }

        public DirectoryPathException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

