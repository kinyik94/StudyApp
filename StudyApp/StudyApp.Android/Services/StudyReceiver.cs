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
using DryIoc;
using StudyApp.Helper;
using StudyApp.Models;
using StudyApp.Services;
using static Android.Resource;



namespace StudyApp.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
    [IntentFilter(new[] { Android.Content.Intent.ActionUserPresent })]
    [IntentFilter(new[] { "StartStudyService" })]
    [IntentFilter(new[] { "StudyNotify" })]
    class StudyReceiver : BroadcastReceiver
    {
        private static int NOTIFICATION_ID = 9000;

        Handler h = new Handler();
        Action handlerAction;
        
        static IStudyNotifier notifier = null;

        public async override void OnReceive(Context context, Intent intent)
        {
            if (notifier == null)
            {
                Container c = StudyApp.App.Dic;
                if (c.IsRegistered<IStudyNotifier>())
                    notifier = c.Resolve<IStudyNotifier>();
                if (notifier == null)
                    return;
            }

            
            await notifier.CheckNotification(context);
            notifier.StudyNotify("Debug", "That's all notification!", context);

            AlarmManager am = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent i = new Intent("StudyNotify");
            PendingIntent pi = PendingIntent.GetBroadcast(context, 0, i, 0);
            am.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 15 * 60 * 1000, pi);
        }
    }
}