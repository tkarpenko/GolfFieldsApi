using System.Timers;
using Timer = System.Timers.Timer;

using Golf.Fields.Database;


namespace Golf.Fields.Background
{
    public class DbUpdateWorker : IBackgroundWorker
    {

        private ManualResetEvent _workerStart = new ManualResetEvent(false);

        private const int TIMER_INTERVAL_IN_SECONDS = 5;

        private Timer? _worker;

        private bool _isRunning = false;

        private DateTime _nextRunDate;

        private bool _isStarted = false;



        public bool Start()
        {
            Console.WriteLine("Starting DB update worker");

            _isStarted = false;

            if (_worker is Timer)
            {
                _worker.Enabled = true;
            }
            else
            {
                _worker = new Timer(TIMER_INTERVAL_IN_SECONDS * 1000);
                _worker.Elapsed += OnTimer;
                _worker.AutoReset = true;
                _worker.Enabled = true;
                _worker.Start();
            }

            _workerStart.WaitOne();

            return _isStarted;
        }



        public void Stop()
        {
            Console.WriteLine("Stop DB update worker");
            if (_worker != null)
            {
                _worker.Enabled = false;
                _worker.Stop();
                _isStarted = false;
                _isRunning = false;
            }
        }


        private void OnTimer(object? source, ElapsedEventArgs e)
        {

            if (_isRunning)
                return;


            _isRunning = true;



            if (_nextRunDate > DateTime.Now)
            {
                _isRunning = false;
                return;
            }


            try
            {
                string? currVersion = null;
                try
                {
                    Console.Write($"Checking DB version: ");

                    currVersion = DB.GetCurrentVersion().Result;

                    Console.WriteLine($"{currVersion}");
                }
                catch(Exception ex)
                {

                    Console.WriteLine($"DB not found. Start seeding");
                    try
                    {
                        bool isSuccess = DB.Seed().Result;

                        StopWorkerIfNeeded(isSuccess);
                    }
                    catch(Exception ex1)
                    {
                        Console.WriteLine($"Failed seeding {(ex1.InnerException == null ? ex1.Message : ex1.InnerException.Message)}; {Environment.NewLine} {ex1.StackTrace}");
                    }
                }

                if (!string.IsNullOrWhiteSpace(currVersion))
                {
                    var isSuccess = DB.CheckNewerVersions(currVersion).Result;

                    if (!_isStarted)
                        StopWorkerIfNeeded(isSuccess);
                }


                if (_nextRunDate <= DateTime.Now)
                    _nextRunDate = DateTime.Now.AddSeconds(TIMER_INTERVAL_IN_SECONDS);


                _isRunning = false;
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhendled error, {(ex.InnerException == null ? ex.Message : ex.InnerException.Message)}{Environment.NewLine}{ex.StackTrace}");

                Stop();
            }
            finally
            {
                _workerStart.Set();
            }
        }


        private void StopWorkerIfNeeded(bool isDbOperationSuccessful)
        {
            if (isDbOperationSuccessful)
            {
                _isStarted = true;
            }
            else
            {
                _isStarted = false;
                Stop();
            }
        }
    }
}

