using SQLite;
using StudyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Models
{
    class ExamModel : ItemModelInterface
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        public int Duration { get; set; }
        public string Location { get; set; }
        
        public int subjectID { get; set; }

        public string UserId { get; set; }

        private string _subjectName;
        [Ignore]
        public string SubjectName {
            get { return _subjectName; }
        }

        public static async Task<List<ExamModel>> GetAllExam()
        {
            List<ExamModel> exams = await StudyAppDatabase.Get().database.Table<ExamModel>().Where(e => e.UserId == App.UserId).OrderBy(e => e.Date).ToListAsync();
            for (int i = exams.Count - 1; i >= 0; --i)
            {
                var c = exams[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.UserId == App.UserId && s.ID == c.subjectID).FirstOrDefaultAsync();
                if (subj != null && subj.Name != null)
                    c._subjectName = subj.Name;
                else
                {
                    exams.RemoveAt(i);
                    await StudyAppDatabase.Get().database.DeleteAsync(c);
                }

            }
            return exams;
        }

        public static async Task<List<ExamModel>> GetItemsAsync(List<DateTime> dates)
        {
            List<ExamModel> exams = await StudyAppDatabase.Get().database.Table<ExamModel>().Where(e => e.UserId == App.UserId && dates.Contains(e.Date)).OrderBy(s => s.Date).ThenBy(s => s.StartTime).ToListAsync();
            for (int i = exams.Count - 1; i >= 0; --i)
            {
                var c = exams[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.UserId == App.UserId && s.ID == c.subjectID).FirstOrDefaultAsync();
                if (subj != null && subj.Name != null)
                    c._subjectName = subj.Name;
                else
                {
                    exams.RemoveAt(i);
                    await StudyAppDatabase.Get().database.DeleteAsync(c);
                }

            }
            return exams;
        }

        public static async Task<ExamModel> GetItemByIDAsync(int ID)
        {
            ExamModel exam = await StudyAppDatabase.Get().database.Table<ExamModel>().Where(e => e.UserId == App.UserId && e.ID == ID).FirstOrDefaultAsync();
            if (exam != null)
            {
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.UserId == App.UserId && s.ID == exam.subjectID).FirstOrDefaultAsync();
                if (subj == null || subj.Name == null)
                    return null;
                exam._subjectName = subj.Name;
            }
            return exam;
        }

        public static async Task<int> SaveItemAsync(ExamModel item)
        {
            var db = StudyAppDatabase.Get().database;
            item.UserId = App.UserId;
            if (item.ID > 0)
            {
                return await db.UpdateAsync(item);
            }
            else
            {
                return await db.InsertAsync(item);
            }
        }

        public static async Task<int> DeleteItemAsync(ExamModel item)
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
            get { return Date.ToString("m"); }
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
                bool past = DateTime.Today >= Date &&  DateTime.Now.TimeOfDay > StartTime;
                return past ? "LightGray" : "White";
            }
        }
    }
}