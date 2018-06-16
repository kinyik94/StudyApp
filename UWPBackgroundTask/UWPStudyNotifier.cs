using Microsoft.Toolkit.Uwp.Notifications;
using StudyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace UWPBackgroundTask
{
    class UWPStudyNotifier : IStudyNotifier
    {
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
                        
                    },
                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = "ms-appx:///Images/icon_subject.png",
                        HintCrop = ToastGenericAppLogoCrop.Default
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
