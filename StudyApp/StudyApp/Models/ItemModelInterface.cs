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

        string RightUpData
        {
            get;
        }

        int LeftSideSize
        {
            get;
        }

        string DisplayStartTime
        {
            get;
        }

        string DisplayEndTime
        {
            get;
        }

        string DisplayDetail
        {
            get;
        }

        bool DetailVisible
        {
            get;
        }
    }
}
