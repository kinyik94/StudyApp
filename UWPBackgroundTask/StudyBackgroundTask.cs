using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library

using Windows.ApplicationModel.Background;
using StudyApp.Models;
using StudyApp.Helper;
using DryIoc;
using StudyApp.Services;

namespace UWPBackgroundTask
{
    public sealed class StudyBackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

        static IStudyNotifier notifier = null;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (notifier == null)
            {
                Container c = StudyApp.App.Dic;
                if(c.IsRegistered<IStudyNotifier>())
                   notifier = c.Resolve<IStudyNotifier>();
                if (notifier == null)
                    return;
            }

            _deferral = taskInstance.GetDeferral();

            await notifier.CheckNotification();
            notifier.StudyNotify("Debug", "That's all notification!");

            _deferral.Complete();
        }
    }
}
