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
    /// Interaction logic for ConvertToPascalCaseRuleWindow.xaml
    /// </summary>
    public partial class ConvertToPascalCaseRuleWindow : UserControl
    {
        ConvertToPascalCaseRule _Rule;
        public ConvertToPascalCaseRuleWindow(ConvertToPascalCaseRule rule)
        {
            this._Rule = rule;
            InitializeComponent();
        }
    }
}
