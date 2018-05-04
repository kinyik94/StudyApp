using SQLite;
using StudyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Models
{
    public class SubjectModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int semesterID { get; set; }

        public static async Task<List<SubjectModel>> GetItemsAsync(int SemesterID)
        {
            return await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(s => s.semesterID == SemesterID).OrderBy(s => s.Name).ToListAsync();
        }

        public static async Task<List<SubjectModel>> GetAllItemsAsync()
        {
            return await StudyAppDatabase.Get().database.Table<SubjectModel>().OrderBy(s => s.Name).ToListAsync();
        }

        public static async Task<SubjectModel> GetItemByIDAsync(int ID)
        {
            return await StudyAppDatabase.Get().database.Table<SubjectModel>().Where(t => t.ID == ID).FirstOrDefaultAsync();
        }

        public static async Task<int> SaveItemAsync(SubjectModel item)
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

        public static async Task<int> RemoveItemAsync(SubjectModel item)
        {
            var db = StudyAppDatabase.Get().database;
            if (item.ID > 0)
            {
                return await db.DeleteAsync(item);
            }
            return 0;
        }
    }
}
