﻿
namespace Makers.Admin.Models
{
    public class MenuModel
    {
        public MenuModel()
        {
        }

        public MenuModel(string group, int mainIndex, int subIndex)
        {
            this.Group = group;
            this.MainIndex = mainIndex;
            this.SubIndex = subIndex;
        }

        public string Group { get; set; }
        public int MainIndex { get; set; }
        public int SubIndex { get; set; }
    }
}