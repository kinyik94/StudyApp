using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using DryIoc;

namespace StudyApp.Services
{
    class UWPStudyNotifier : IStudyNotifier
    {
        public override async Task StartNotify()
        {
            var studyTaskName = "StudyBackgroundTask";

            foreach (var t in BackgroundTaskRegistration.AllTasks)
            {
                if (t.Value.Name == studyTaskName)
                {
                    //taskRegistered = true;
                    t.Value.Unregister(true);
                    break;
                }
            }
            var builder = new BackgroundTaskBuilder();

            builder.Name = studyTaskName;
            builder.TaskEntryPoint = "UWPBackgroundTask.StudyBackgroundTask";
            builder.SetTrigger(new TimeTrigger(15, false));

            var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTaskRegistration task = builder.Register();

            try
            {
                var notifier = StudyApp.App.Dic.Resolve<IStudyNotifier>();
                if (notifier != null)
                {
                    await notifier.CheckNotification();
                    notifier.StudyNotify("Debug", "That's all notification!");
                }
            }
            catch { }
        }

        public override async Task StopNotify()
        {
            var studyTaskName = "StudyBackgroundTask";

            foreach (var t in BackgroundTaskRegistration.AllTasks)
            {
                if (t.Value.Name == studyTaskName)
                {
                    //taskRegistered = true;
                    t.Value.Unregister(true);
                    break;
                }
            }
        }

        public override void StudyNotify(string title, string text, object context = null)
        {
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title
                        },

                        new AdaptiveText()
                        {
                            Text = text
                        }
                    }
                }
            };

            // Now we can construct the final toast content
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
            };

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
