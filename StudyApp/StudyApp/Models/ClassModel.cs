﻿using SQLite;
using StudyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Models
{
    class ClassModel : ItemModelInterface
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

        public static async Task<List<ClassModel>> GetItemsAsync(DateTime now, int day, int week)
        {
            List<ClassModel> classes = await StudyAppDatabase.Get().database.Table<ClassModel>().Where(c => c.Day == day && c.Week == week && c.StartDate <= now && c.EndDate >= now).OrderBy(c => c.StartTime).ToListAsync();
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