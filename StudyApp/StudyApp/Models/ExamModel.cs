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
        
        private string _subjectName;
        [Ignore]
        public string SubjectName {
            get { return _subjectName; }
        }

        public static async Task<List<ExamModel>> GetItemsAsync(DateTime now)
        {
            List<ExamModel> exams = await StudyAppDatabase.Get().database.Table<ExamModel>().Where(s => s.Date >= now).OrderByDescending(s => s.Date).ToListAsync();
            for (int i = exams.Count - 1; i >= 0; --i)
            {
                var c = exams[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == c.subjectID).FirstOrDefaultAsync();
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
            ExamModel exam = await StudyAppDatabase.Get().database.Table<ExamModel>().Where(t => t.ID == ID).FirstOrDefaultAsync();
            if (exam != null)
            {
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == exam.subjectID).FirstOrDefaultAsync();
                if (subj == null || subj.Name == null)
                    return null;
                exam._subjectName = subj.Name;
            }
            return exam;
        }

        public static async Task<int> SaveItemAsync(ExamModel item)
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

        public int GetID()
        {
            return ID;
        }

        public string DisplayString
        {
            get { return SubjectName; }
        }
    }
}