﻿using SQLite;
using StudyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Models
{
    class TaskModel : ItemModelInterface
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }

        public string Summary { get; set; }
        public int subjectID { get; set; }

        public string UserId { get; set; }

        private string _subjectName;
        [Ignore]
        public string SubjectName
        {
            get { return _subjectName; }
        }

        public static async Task<List<TaskModel>> GetAllTask()
        {
            List<TaskModel> tasks = await StudyAppDatabase.Get().database.Table<TaskModel>().Where(t => t.UserId == App.UserId).OrderBy(t => t.DueDate).ToListAsync();
            for (int i = tasks.Count - 1; i >= 0; --i)
            {
                var task = tasks[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(t => t.ID == task.subjectID && t.UserId == App.UserId).FirstOrDefaultAsync();
                if (subj != null && subj.Name != null)
                    task._subjectName = subj.Name;
                else
                {
                    tasks.RemoveAt(i);
                    await StudyAppDatabase.Get().database.DeleteAsync(task);
                }

            }
            return tasks;
        }

        public static async Task<List<TaskModel>> GetItemsAsync(List<DateTime> dates)
        {
            List<TaskModel> tasks = await StudyAppDatabase.Get().database.Table<TaskModel>().Where(t => t.UserId == App.UserId && dates.Contains(t.DueDate)).OrderBy(t => t.DueDate).ToListAsync();
            for (int i = tasks.Count - 1; i >= 0; --i)
            {
                var task = tasks[i];
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.UserId == App.UserId && s.ID == task.subjectID).FirstOrDefaultAsync();
                if (subj != null && subj.Name != null)
                    task._subjectName = subj.Name;
                else
                {
                    tasks.RemoveAt(i);
                    await StudyAppDatabase.Get().database.DeleteAsync(task);
                }

            }
            return tasks;
        }

        public static async Task<TaskModel> GetItemByIDAsync(int ID)
        {
            TaskModel task = await StudyAppDatabase.Get().database.Table<TaskModel>().Where(t => t.ID == ID && t.UserId == App.UserId).FirstOrDefaultAsync();
            if (task != null)
            {
                SubjectModel subj = await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.ID == task.subjectID).FirstOrDefaultAsync();
                if (subj == null || subj.Name == null)
                    return null;
                task._subjectName = subj.Name;
            }
            return task;
        }

        public static async Task<int> SaveItemAsync(TaskModel item)
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

        public static async Task<int> DeleteItemAsync(TaskModel item)
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
            get { return Title; }
        }

        public string DisplayStartTime
        {
            get { return null; }
        }

        public string DisplayEndTime
        {
            get { return null; }
        }

        public string DisplayDetail
        {
            get { return SubjectName; }
        }

        public string RightUpData
        {
            get { return DueDate.ToString("m"); }
        }

        public string RightDownData
        {
            get { return null; }
        }

        public int LeftSideSize
        {
            get { return 0; }
        }

        public bool DetailVisible
        {
            get { return true; }
        }

        public string ItemColor
        {
            get
            {
                return "White";
            }
        }
    }
}