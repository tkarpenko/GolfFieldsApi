using System;
namespace Golf.Fields.BackgroundWork
{
    public static class BackgroundWorkers
    {
        private static readonly int WORKERS_COUNT = 1;

        private static readonly IBackgroundWorker[] _workers = new IBackgroundWorker[WORKERS_COUNT];

        private static readonly IBackgroundWorker[] _runningWorkers = new IBackgroundWorker[WORKERS_COUNT];



        public bool Start()
        {
            bool isSuccess = true;

            _workers[0] = new DbUpdateWorker();

            int i = 0;
            foreach (var worker in _workers)
            {
                var success = worker.Start();
                if (success)
                {
                    _runningWorkers[i++] = worker;
                }
                else
                {
                    Stop();
                    isSuccess = false;
                    break;
                }
            }

            return isSuccess;
        }



        private bool Stop()
        {
            bool isSuccess = true;

            foreach (var worker in _runningWorkers)
            {
                var workerStopped = worker.Stop();
                if (!workerStopped)
                    isSuccess = false;
            }

            return isSuccess;
        }
    }
}

