using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CommonModel
{
    public class ReplaceSpecialCharacterRule : IRule
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public string ReplaceCharacter { get; set; }    
        public string IntoCharacter { get; set; }
        public ReplaceSpecialCharacterWindow ConfigurationUI { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public ReplaceSpecialCharacterRule()
        {
            this.Name = "Replace Special Character";
            this.ReplaceCharacter = String.Empty;
            this.IntoCharacter = String.Empty;
            var instance = this;
            ConfigurationUI = new ReplaceSpecialCharacterWindow(ref instance);
        }

        public string Apply(string originValue, object parameters)
        {
            try
            {
                if (originValue.Contains(ReplaceCharacter))
                {
                    CheckExceptionBeforeApply();

                    var newValue = originValue.Replace(ReplaceCharacter, IntoCharacter);
                    return newValue;
                }
                return originValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }

        private void CheckExceptionBeforeApply()
        {
            
            if (isNotSpecialCharacter(ReplaceCharacter))
            {
                throw new Exception($"Kí tự trước khi thay thế phải là kí tự đặc biệt hoặc khoảng trắng.");
            }

            if (isNotSpecialCharacter(IntoCharacter))
            {
                throw new Exception($"Kí tự sau khi thay thế phải là kí tự đặc biệt hoặc khoảng trắng.");
            }

            if (ReplaceCharacter == IntoCharacter)
            {
                throw new Exception($"Kí tự trước và sau khi thay thế phải khác nhau.");
            }
        }

        private bool isNotSpecialCharacter(string value)
        {
            Regex regex = new Regex(@"[a-zA-Z0-9]");
            var rs = regex.IsMatch(value);
            return rs;
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
                    var resultString = this.Apply(orginString, parameters);
                    resultStringList.Add(resultString);
                }
                return orginStringList;
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
}
