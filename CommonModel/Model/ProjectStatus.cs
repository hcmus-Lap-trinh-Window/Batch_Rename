using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Model
{
    public class ProjectStatus
    {
        public List<RuleJson> RulesList { get; set; }
        public string Preset { get; set; }
        public int currentFilePage { get; set; }
        public int currentFolderPage { get; set; }
        public BindingList<MyFile> FileList { get; set; }
        public BindingList<MyFolder> FolderList { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
    }
}
