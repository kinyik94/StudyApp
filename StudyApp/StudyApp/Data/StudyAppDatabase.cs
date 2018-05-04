using SQLite;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StudyApp.Data
{
    class StudyAppDatabase
    {
        private static StudyAppDatabase _instance;

        public readonly SQLiteAsyncConnection database;
        private StudyAppDatabase()
        {
            string path = DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3");

            database = new SQLiteAsyncConnection(System.IO.Path.Combine(path));
            database.CreateTableAsync<SemesterModel>().Wait();
            database.CreateTableAsync<SubjectModel>().Wait();
            database.CreateTableAsync<TaskModel>().Wait();
            database.CreateTableAsync<ClassModel>().Wait();
            database.CreateTableAsync<ExamModel>().Wait();
        }

        public static StudyAppDatabase Get()
        {
            if (_instance == null)
                _instance = new StudyAppDatabase();
            return _instance;
        }
    }
}
