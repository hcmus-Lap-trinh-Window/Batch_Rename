using CommonModel;
using Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
            textBlock.Text = "Text: " + String.Join("\n", _RuleFactory.GetAllRuleNames());
        }

        #endregion

    }
}
