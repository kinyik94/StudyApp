using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StudyApp.Droid;

namespace StudyApp.Services
{
    class AndroidStudyNotifier : IStudyNotifier
    {
        private static int NOTIFICATION_ID = 9000;
        public override void StudyNotify(string title, string text, object context)
        {
            Context ctx = context as Context;
            if (context == null)
                return;

            Intent notifyIntent = new Intent(ctx, typeof(MainActivity));
            // Set the Activity to start in a new, empty task
            notifyIntent.SetFlags(ActivityFlags.SingleTop);
            // Create the PendingIntent
            PendingIntent notifyPendingIntent = PendingIntent.GetActivity(
                    ctx, 0, notifyIntent, PendingIntentFlags.UpdateCurrent
            );

            Android.App.Notification.Builder notificationBuilder = new Android.App.Notification.Builder(ctx)
                .SetSmallIcon(Resource.Drawable.icon_subject)
                .SetContentTitle(title)
                .SetContentText(text)
                .SetContentIntent(notifyPendingIntent);

            var note = notificationBuilder.Build();
            note.Defaults |= NotificationDefaults.Vibrate;
            note.Defaults |= NotificationDefaults.Sound;

            var notificationManager = (NotificationManager)ctx.GetSystemService(Context.NotificationService);
            notificationManager.Notify(NOTIFICATION_ID, note);
            NOTIFICATION_ID++;
            if (NOTIFICATION_ID >= 10000)
                NOTIFICATION_ID = 9000;
        }

        public override async Task StartNotify()
        {
            Intent intent = new Intent("StartStudyService");
            Android.App.Application.Context.SendBroadcast(intent);
        }

        public override async Task StopNotify()
        {
            Intent intent = new Intent("StopStudyService");
            Android.App.Application.Context.SendBroadcast(intent);
        }
    }
}