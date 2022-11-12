using System;
using System.Linq;
using System.Text;

namespace CommonModel
{
    /// <summary>
    /// Thêm tiền tố cho tên file
    /// </summary>
    public class AddPrefixRule : IRule
    {
        public string Name { get; set; }
        public RuleStatus RuleState { get; set; }

        public string Apply(string originalValue, object parameters)
        {
            try
            {
                if(parameters == null)
                {
                    throw new Exception($"AddPrefixRule failed. Parameter is null");
                }
                var addPrefixRuleData = (AddPrefixRuleData)parameters;
                if(addPrefixRuleData.Prefix.IsNullOrWhiteSpace())
                {
                    throw new Exception($"Prefix value là null, rỗng hoặc khoảng trắng.");
                }
                var prefix = addPrefixRuleData.Prefix;
                return prefix.Concat(originalValue).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }


        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class AddPrefixRuleData
    {
        public string Prefix { get; set; }
    }
}
