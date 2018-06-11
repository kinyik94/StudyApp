using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StudyApp.Droid;

namespace StudyApp.Notification
{
    class AndroidStudyNotifier : IStudyNotifier
    {
        private static int NOTIFICATION_ID = 9000;
        public override void StudyNotify(string title, string text, object context)
        {
            Context ctx = context as Context;
            if (context == null)
                return;

            Android.App.Notification.Builder notificationBuilder = new Android.App.Notification.Builder(ctx)
                .SetSmallIcon(Resource.Drawable.icon_subject)
                .SetContentTitle(title)
                .SetContentText(text);

            var notificationManager = (NotificationManager)ctx.GetSystemService(Context.NotificationService);
            notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());
            NOTIFICATION_ID++;
            if (NOTIFICATION_ID >= 10000)
                NOTIFICATION_ID = 9000;
        }
    }
}