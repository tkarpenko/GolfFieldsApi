namespace Golf.Fields.Background;

public class BackgroundWorkers
{

    private const int WORKERS_BEFORE_APP_RUN_COUNT = 1;

    private const int WORKERS_AFTER_APP_RUN_COUNT = 0;


    private readonly IBackgroundWorker[] _workersBeforeAppRun = new IBackgroundWorker[WORKERS_BEFORE_APP_RUN_COUNT];

    private readonly IBackgroundWorker[] _workersAfterAppRun = new IBackgroundWorker[WORKERS_AFTER_APP_RUN_COUNT];



    public BackgroundWorkers()
    {
        _workersBeforeAppRun[0] = new DbUpdateWorker();
    }





    public bool RunWorkersBeforeAppRun()
    {
        foreach (var worker in _workersBeforeAppRun)
        {
            var isWorkerStarted = worker.Start();

            if (!isWorkerStarted)
            {
                StopWorkersBeforeAppRun();
                return false;
            }
        }

        return true;
    }



    public void RunWorkersAfterAppStart()
    {
        foreach (var worker in _workersAfterAppRun)
        {
            worker.Start();
        }
    }



    private void StopWorkersBeforeAppRun()
    {
        foreach (var worker in _workersBeforeAppRun)
        {
            worker.Stop();
        }
    }
}

