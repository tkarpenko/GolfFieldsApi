namespace Golf.Fields.Shared
{
    [Serializable]
    public class TcpException : Exception
    {
        public TcpException()
        {

        }

        public TcpException(string message) : base(message)
        {

        }

        public TcpException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

