using CommonModel;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CommonModel
{
    public class AddNumberCounterRule : IRule
    {
        public string Name { get; set; }
        public int Start { get;set; }
        public int Step { get; set; }
        public int NumOfDigits { get; set; }
        public AddNumberCounterRuleWindow ConfigUI { get; set; }
        public bool IsInUse { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public AddNumberCounterRule()
        {
            this.Name = "Add Number Counter";
            this.Step = 1;
            this.Start = 1;
            this.NumOfDigits = 0;
            var instance = this;
            this.ConfigUI = new AddNumberCounterRuleWindow(ref instance);
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
                if (parameters == null)
                {
                    throw new Exception("AddNumberCounterRule failed. Parameters cannot be null");
                }
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
    }
}
