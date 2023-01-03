using CommonModel;
using System;
using System.Collections.Generic;
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

namespace RuleWindow
{

    public partial class AddSuffixRuleWindow : UserControl
    {
        private AddSuffixRule _addSuffixRule;
        public AddSuffixRuleWindow(ref AddSuffixRule addSuffixRule)
        {
            this._addSuffixRule = addSuffixRule;
            InitializeComponent();
        }

        private void suffixInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._addSuffixRule.Suffix = suffixInput.Text;
        }
    }
}
