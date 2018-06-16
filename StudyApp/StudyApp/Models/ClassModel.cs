using SQLite;
using StudyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Models
{
    public class ClassModel : ItemModelInterface
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int Day { get; set; }
        public TimeSpan StartTime { get; set; }

        public int Duration { get; set; }

        public bool Repeats { get; set; }
        public int Week { get; set; }
        public string Location { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int subjectID { get; set; }

        private string _subjectName;
        [Ignore]
        public string SubjectName
        {
            get { return _subjectName; }
        }

        public static async Task<List<ClassModel>> GetAllClass()
        {
            List<ClassModel> classes = await StudyAppDatabase.Get().database.Table<ClassModel>().ToListAsync();
            for (int i = classes.Count - 1; i >= 0; --i)
            {
                var c = classes[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == c.subjectID).FirstOrDefaultAsync();
                if (subj != null && subj.Name != null)
                    c._subjectName = subj.Name;
                else
                {
                    classes.RemoveAt(i);
                    await StudyAppDatabase.Get().database.DeleteAsync(c);
                }

            }
            return classes;
        }

        public static async Task<List<ClassModel>> GetNotifyClasses(DateTime now, TimeSpan time, int day, int week)
        {
            try
            {
                TimeSpan nextTime = time + TimeSpan.FromMinutes(15);
                TimeSpan nextEndTime = nextTime + TimeSpan.FromMinutes(15);

                List<ClassModel> classes = await StudyAppDatabase.Get().database.Table<ClassModel>()
                .Where(c => (
                            ((c.Repeats && c.Day == day && (c.Week == week || c.Week == 0) && c.StartDate <= now && c.EndDate >= now) ||
                            (!c.Repeats && c.StartDate == now)) && (nextTime <= c.StartTime) && (c.StartTime < nextEndTime)
                      ))
                .ToListAsync();
                for (int i = classes.Count - 1; i >= 0; --i)
                {
                    var c = classes[i];
                    SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == c.subjectID).FirstOrDefaultAsync();
                    if (subj != null && subj.Name != null)
                        c._subjectName = subj.Name;
                    else
                    {
                        classes.RemoveAt(i);
                        await StudyAppDatabase.Get().database.DeleteAsync(c);
                    }

                }
                return classes;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<List<ClassModel>> GetItemsAsync(DateTime now, int day, int week)
        {
            List<ClassModel> classes = await StudyAppDatabase.Get().database.Table<ClassModel>()
                .Where(c => (
                            (c.Repeats && c.Day == day && (c.Week == week || c.Week == 0) && c.StartDate <= now && c.EndDate >= now) || 
                            (!c.Repeats && c.StartDate == now)
                      ))
                .OrderBy(c => c.StartTime).ToListAsync();
            for(int i = classes.Count - 1; i >= 0; --i)
            {
                var c = classes[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == c.subjectID).FirstOrDefaultAsync();
                if (subj != null && subj.Name != null)
                    c._subjectName = subj.Name;
                else
                {
                    classes.RemoveAt(i);
                    await StudyAppDatabase.Get().database.DeleteAsync(c);
                }
            }
            return classes;
        }

        public static async Task<ClassModel> GetItemByIDAsync(int ID)
        {
            ClassModel c = await StudyAppDatabase.Get().database.Table<ClassModel>().Where(t => t.ID == ID).FirstOrDefaultAsync();
            if (c != null)
            {
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == c.subjectID).FirstOrDefaultAsync();
                if (subj == null || subj.Name == null)
                    return null;
                c._subjectName = subj.Name;
            }
            return c;
        }

        public static async Task<int> SaveItemAsync(ClassModel item)
        {
            var db = StudyAppDatabase.Get().database;
            if (item.ID > 0)
            {
                return await db.UpdateAsync(item);
            }
            else
            {
                return await db.InsertAsync(item);
            }
        }

        public static async Task<int> DeleteItemAsync(ClassModel item)
        {
            var db = StudyAppDatabase.Get().database;
            if (item.ID > 0)
            {
                return await db.DeleteAsync(item);
            }
            return 0;
        }

        public int GetID()
        {
            return ID;
        }

        public string DisplayString
        {
            get { return SubjectName; }
        }

        public string DisplayStartTime
        {
            get { return StartTime.ToString(@"hh\:mm"); }
        }

        public string DisplayEndTime
        {
            get { return (StartTime + TimeSpan.FromMinutes(Duration)).ToString(@"hh\:mm"); }
        }

        public string DisplayDetail
        {
            get { return null; }
        }

        public string RightUpData
        {
            get { return Location; }
        }

        public string RightDownData
        {
            get { return null; }
        }

        public int LeftSideSize
        {
            get { return 60; }
        }

        public bool DetailVisible
        {
            get { return false; }
        }

        public string ItemColor
        {
            get
            {
                bool past = DateTime.Now.TimeOfDay > StartTime;
                return past ? "LightGray" : "White";
            }
        }

    }
}