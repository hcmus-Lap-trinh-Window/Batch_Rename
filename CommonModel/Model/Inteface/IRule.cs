namespace CommonModel
{
    public interface IRule
    {
        public string Name { get; }
        public RuleStatus RuleState { get; }
    }

    public enum RuleStatus
    {
        InUse = 0,
        NotUse = 1,
    }
}
