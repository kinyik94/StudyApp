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

        string RightDownData
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

        string ItemColor
        {
            get;
        }
    }
}
