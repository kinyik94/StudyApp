using System;
using System.Collections.Generic;
using System.Text;

namespace StudyApp.Models
{
    public class ItemModel : ItemModelInterface
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int GetID()
        {
            return ID;
        }

        public string DisplayString
        {
            get { return Name; }
        }
    }
}
