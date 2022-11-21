using CommonModel.Model;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CommonModel
{
    public class ConvertToPascalCaseRule : IRule
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public UserControl ConfigurationUI { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public ConvertToPascalCaseRule()
        {
            this.Name = "Convert To PascalCase";
            this.IsInUse = false;
            this.ConfigurationUI = new ConvertToPascalCaseRuleWindow(this);
        }

        public string Apply(string originalString, object parameters)
        {
            try
            {
                if (originalString == null || originalString.IsNullOrWhiteSpace())
                {
                    throw new Exception($"ConvertToPascalCaseRule applies failed. originString[{originalString}] is null or white spaces");
                }
                var originalWordArray = Regex.Split(originalString, @"[\s\.\-+!#$%^&*:;',<>|]");
                var modifiedWordList =new List<string>();
                for(int i = 0; i < originalWordArray.Length; i++)
                {
                    var captializedWord = Regex.Replace(originalWordArray[i], "^[a-z]", c => c.Value.ToUpper());
                    modifiedWordList.Add(captializedWord);
                }
                return String.Join("", modifiedWordList);
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

        public IRule Clone(RuleJson ruleJson)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
