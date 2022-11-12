using CommonModel;
using Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Batch_Rename_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RuleFactory _RuleFactory;
        private readonly RuleConfig _RuleConfig;

        public MainWindow(IOptionsSnapshot<RuleConfig> ruleConfig)
        {
            this._RuleConfig = ruleConfig.Value;
            this._RuleFactory = RuleFactory.GetInstance(ruleConfig);
            InitializeComponent();
        }

        #region Main methods

        #endregion

        #region Private methods

        ///<author>Nguyen Tuan Khanh</author>
        /// <summary>
        /// FirstLoad: Xử lý khi màn hình được load lên lần đầu tiên
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void FistLoad(object sender, RoutedEventArgs e)
        {
            menu.ItemsSource = _RuleFactory.GetAllRuleNames();
        }

        private void LoadFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var fileDictionary = new Dictionary<long, string>();
            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);
                originFileName.Text = fileInfo.Name;
            }
        }

        #endregion

        private void SelectRule(object sender, SelectionChangedEventArgs e)
        {
            var ruleType = ((ComboBox)sender).SelectedItem;
            IRule rule = _RuleFactory.CreateRuleInstance(ruleType.ToString());
            var ruleData = new { Prefix = "123" };
            var originText = originFileName.Text;
            var newString = rule.Apply(originText, ruleData);
            previewFileName.Text = newString;
        }
    }

    class SingletonCount
    {
        private static SingletonCount _Instance;
        public int _Count { get; set; }

        private SingletonCount()
        {
            _Count = 0;
        }

        public static SingletonCount GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new SingletonCount();
            }
            return _Instance;
        }
    }
}
