using System;

namespace CommonModel
{
    public interface IRule : ICloneable
    {
        public string Name { get; set; }
        public RuleStatus RuleState { get; set; }
        public string Apply(string originValue, object parameters);
    }

    public enum RuleStatus
    {
        InUse = 0,
        NotUse = 1,
    }
}
