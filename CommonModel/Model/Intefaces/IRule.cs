using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CommonModel
{
    public interface IRule : ICloneable, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public RuleStatus RuleState { get; set; }
        public string Apply(string originString, object parameters);
        public List<string> Apply(List<string> orginStringList, object parameters);
    }

    public enum RuleStatus
    {
        InUse = 0,
        NotUse = 1,
    }
}
