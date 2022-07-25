using System;
namespace Golf.Fields.Background
{
    public interface IBackgroundWorker
    {

        public bool Start();

        public void Stop();
    }
}

