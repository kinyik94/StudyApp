using StudyApp.Helper;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Services
{
    public abstract class IStudyNotifier
    {
        public abstract Task StartNotify();
        public abstract Task StopNotify();
        public abstract void StudyNotify(string title, string text, object context = null);

        public async Task CheckNotification(object context = null)
        {
            var classes = await ClassModel.GetNotifyClasses(DateTime.Today, DateHelper.GetTime(), DateHelper.GetDayOfWeek(), await DateHelper.GetWeek());
            foreach (var c in classes)
            {
                StudyNotify(c.SubjectName, c.StartTime.ToString(@"hh\:mm") + "   " + c.Location, context);
            }
        }
    }
}
