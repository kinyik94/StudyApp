using System;
using System.Collections.Generic;
using System.Text;

namespace StudyApp.Models
{
    public interface ItemModelInterface
    {
        int GetID();

        string DisplayString
        {
            get;
        }
    }
}
