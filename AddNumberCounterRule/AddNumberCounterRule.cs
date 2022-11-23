using CommonModel;
using CommonModel.Model;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace CommonModel
{
    public class AddNumberCounterRule : IRule
    {
        public string Name { get; set; }
        public int Start { get;set; }
        public int Step { get; set; }
        public int NumOfDigits { get; set; }
        [JsonIgnore]
        public UserControl ConfigurationUI { get; set; }
        public bool IsInUse { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AddNumberCounterRule()
        {
            Name = "Add Number Counter";
            Step = 1;
            Start = 1;
            NumOfDigits = 0;
            ConfigurationUI = new AddNumberCounterRuleWindow(this);
        }

        public string Apply(string originString, object parameters)
        {
            throw new NotImplementedException();
        }

        public List<string> Apply(List<string> orginStringList, object parameters)
        {
            List<string> result = new List<string>();
            try
            {
                if (orginStringList != null && orginStringList.Count > 0)
                {
                    int indexOfFile = 0;
                    foreach (var origin in orginStringList)
                    {
                        var currentCounter = this.Start + this.Step * indexOfFile;
                        var counterPrefix = getCounterPrefix(currentCounter, NumOfDigits);
                        if (!string.IsNullOrEmpty(counterPrefix))
                        {
                            var outputFileName = counterPrefix + origin;
                            if (outputFileName.FileWithExtensionValidation(outputFileName.getExtension()))
                            {
                                result.Add(outputFileName);
                            }
                        }
                        indexOfFile++;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return result;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
        /// <summary>
        /// get counter prefix from current index and number of digits
        /// </summary>
        /// <param name="currentCounter"></param>
        /// <param name="numofDigits"></param>
        /// <returns></returns>
        private static string getCounterPrefix(int currentCounter, int numofDigits)
        {
            string result = string.Empty;
            var sCurrentCounter = currentCounter.ToString(); // string of current counter
            if (sCurrentCounter.Length >= numofDigits)
            {
                result = currentCounter.ToString();
            }
            else
            {
                result = currentCounter.ToString().PadLeft(numofDigits, '0');
            }
            return result;
        }

        public IRule Clone(RuleJson ruleJson)
        {
            AddNumberCounterRule instance = null;
            try
            {
                instance = JsonSerializer.Deserialize<AddNumberCounterRule>(ruleJson.Json);
                if (instance != null)
                {
                    instance.ConfigurationUI = new AddNumberCounterRuleWindow(instance);
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
}
