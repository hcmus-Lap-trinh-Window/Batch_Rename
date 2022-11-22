using CommonModel.Model;
using Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CommonModel
{
    public class RuleFactory
    {
        private static readonly object _InstanceLock = new object();
        static private Dictionary<string, IRule> _Prototypes = new Dictionary<string, IRule>();
        static private RuleFactory _Instance = null;
        private readonly RuleConfig _RuleConfig;

        private RuleFactory(RuleConfig ruleConfig)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var ruleDllFolder = Path.Combine(currentDirectory, ruleConfig.RuleFolder);
            if (ruleConfig.RuleList == null)
            {
                return;
            }
            foreach (var ruleName in ruleConfig.RuleList)
            {
                var ruleFilePath = Path.Combine(ruleDllFolder, $"{ruleName}.dll");
                LoadRuleDll(ruleName, ruleFilePath);
            }
        }

        public static RuleFactory GetInstance(IOptionsSnapshot<RuleConfig>? ruleConfig)
        {
            if (_Instance == null)
            {
                lock (_InstanceLock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new RuleFactory(ruleConfig: ruleConfig?.Value);
                    }
                }
            }
            return _Instance;
        }

        public IRule CreateRuleInstance(string type)
        {
            try
            {
                IRule rule = (IRule)(_Prototypes[type] as ICloneable)!.Clone();
                return rule;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IRule CreateRuleInstance(RuleJson ruleJson)
        {
            IRule result = null;
            try
            {
                result = (IRule)_Prototypes[ruleJson.Name].Clone(ruleJson);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return result;
        }

        #region Static Methods

        /// <author>Nguyen Tuan Khanh</author>
        /// <summary>
        /// GetAllRuleNames: Lấy hết các rule names mà factory đang có.
        /// </summary>
        /// <returns>List<string></returns>
        public List<string> GetAllRuleNames()
        {
            return _Prototypes.GetAllKeys();
        }
        #endregion

        #region Private methods
        /// <author>Nguyen Tuan Khanh</author>
        /// <summary>
        /// LoadRuleDll: Load file dll của rule
        /// </summary>
        /// <param name="ruleName">Tên rule</param>
        /// <param name="ruleFilePath">Đường dẫn file dll của rule</param>
        /// <returns>bool</returns>
        private static bool LoadRuleDll(string ruleName, string ruleFilePath)
        {
            try
            {
                if (ruleFilePath.IsNullOrWhiteSpace())
                {
                    return false;
                }
                if (!File.Exists(ruleFilePath))
                {
                    return false;
                }
                var assembly = Assembly.Load(File.ReadAllBytes(ruleFilePath));
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && typeof(IRule).IsAssignableFrom(type))
                    {
                        IRule ruleInstance = (IRule)Activator.CreateInstance(type);
                        if (_Prototypes.ContainsKey(ruleInstance.Name))
                        {
                            return false;
                        }
                        _Prototypes.Add(ruleInstance.Name, ruleInstance);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }

}
