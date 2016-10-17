using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BackgroundAgentDemo
{
    public sealed class ApressBackgroundAgent : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["LastBackgrounAgentRunTimestamp"] = DateTime.Now.ToString();

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            // Do asynchronous stuff here using Async-Await
            deferral.Complete();
        }
    }
}
