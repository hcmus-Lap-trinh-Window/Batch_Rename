using CommonModel.Model;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;

namespace CommonModel
{
    /// <summary>
    /// Loại bỏ khoảng trắng trong tên file
    /// </summary>
    public class TrimRule : IRule
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }

        public TrimConfig config { get; set; }
        public TrimRuleWindow ConfigurationUI { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public TrimRule()
        {
            this.Name = @"Trim Rule";
            this.config = default(TrimConfig);
            var instance = this;
            ConfigurationUI = new TrimRuleWindow(ref instance);
        }
        public string Apply(string originString, object parameters)
        {
            try
            {
                if (originString == null || originString.IsNullOrWhiteSpace())
                {
                    throw new Exception($"TrimRule applies failed. originString[{originString}] is null or white spaces");
                }
                switch (config)
                {
                    case TrimConfig.LeadingSpace:
                        return originString.TrimStart();
                    case TrimConfig.TrailingSpace:
                        return originString.TrimEnd();
                    case TrimConfig.Both:
                        return originString.Trim();
                    default:
                        return originString;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }
        public List<string> Apply(List<string> orginStringList, object parameters)
        {

            try
            {
                if (orginStringList == null || orginStringList.Count < 1)
                {
                    throw new Exception("Apply failed. originStringList is null or empty");
                }
                var resultStringList = new List<string>();
                foreach (var orginString in orginStringList)
                {
                    var originStringArray = orginString.Split('.').ToList();
                    var extension = originStringArray.LastOrDefault();
                    originStringArray.RemoveAt(originStringArray.Count - 1);
                    var fileName = String.Join(".", originStringArray);
                    var newFileName = this.Apply(fileName, parameters);
                    resultStringList.Add(String.Format("{0}.{1}", newFileName, extension));
                }
                return resultStringList;
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

        public IRule Clone(RuleJson ruleJson)
        {
            TrimRule instance = null;
            try
            {
                instance = JsonSerializer.Deserialize<TrimRule>(ruleJson.Json);
                if (instance != null)
                {
                    instance.ConfigurationUI = new TrimRuleWindow(ref instance);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return instance;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public enum TrimConfig
    {
        [Description("Khoảng trắng ở đầu")]
        LeadingSpace = 0,
        [Description("Khoảng trắng ở cuối")]
        TrailingSpace = 1,
        [Description("Khoảng trắng cả đầu và cuối")]
        Both = 2
    }
}
