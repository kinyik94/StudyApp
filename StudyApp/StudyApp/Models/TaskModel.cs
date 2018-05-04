using SQLite;
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

        public static async Task<List<TaskModel>> GetItemsAsync(DateTime now)
        {
            return await StudyAppDatabase.Get().database.Table<TaskModel>().Where(s => s.DueDate >= now).OrderByDescending(s => s.DueDate).ToListAsync();
        }

        public static async Task<TaskModel> GetItemByIDAsync(int ID)
        {
            return await StudyAppDatabase.Get().database.Table<TaskModel>().Where(t => t.ID == ID).FirstOrDefaultAsync();
        }

        public static async Task<int> SaveItemAsync(TaskModel item)
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
            get { return Title; }
        }
    }
}