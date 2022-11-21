using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CommonModel;

namespace RuleWindow
{
    public partial class AddPrefixRuleWindow : UserControl
    {
        AddPrefixRule _AddPrefixRule;
        //public AddPrefixRuleWindow(ref AddPrefixRule addPrefixRule)
        public AddPrefixRuleWindow(AddPrefixRule addPrefixRule)
        {
            this._AddPrefixRule = addPrefixRule;
            InitializeComponent();
        }

        private void LoadingFirstly(object sender, RoutedEventArgs e)
        {
            //this.DataContext = this._AddPrefixRule;
        }

        private void prefixInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._AddPrefixRule.Prefix = prefixInput.Text;
        }
    }
}
