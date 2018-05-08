using SQLite;
using StudyApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Models
{
    public class SemesterModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public static async Task<List<SemesterModel>> GetItemsAsync()
        {
            return await StudyAppDatabase.Get().database.Table<SemesterModel>().OrderByDescending(t => t.ID).ToListAsync();
        }

        public static async Task<SemesterModel> GetLastAsync()
        {
            return await StudyAppDatabase.Get().database.Table<SemesterModel>().OrderByDescending(t => t.ID).FirstOrDefaultAsync();
        }

        public static async Task<SemesterModel> GetItemByIDAsync(int ID)
        {
            return await StudyAppDatabase.Get().database.Table<SemesterModel>().Where(t => t.ID == ID).FirstOrDefaultAsync();
        }

        public static async Task<int> SaveItemAsync(SemesterModel item)
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
    }
}
