﻿using Windows.ApplicationModel.Background;

namespace TheClockEnd.BackgroundTasks
{
    public sealed class ClockBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            WindowsLiveTileSchedule.CreateSchedule();
            deferral.Complete();
        }
    }
}
