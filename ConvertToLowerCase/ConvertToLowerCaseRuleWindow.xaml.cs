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
    /// <summary>
    /// Interaction logic for ConvertToLowerCaseRuleWindow.xaml
    /// </summary>
    public partial class ConvertToLowerCaseRuleWindow : UserControl
    {
        ConvertToLowerCaseRule rule;
        public ConvertToLowerCaseRuleWindow(ConvertToLowerCaseRule rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DataContext = rule;
        }
    }
}
