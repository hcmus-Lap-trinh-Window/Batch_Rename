using CommonModel;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddNumberCounterRuleWindow.xaml
    /// </summary>
    public partial class AddNumberCounterRuleWindow : UserControl
    {
        private AddNumberCounterRule rule;
        public AddNumberCounterRuleWindow(ref AddNumberCounterRule rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void startInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+"); // check input must be a number
            e.Handled = regex.IsMatch(e.Text);
        }

        private void stepInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+"); // check input must be a number
            e.Handled = regex.IsMatch(e.Text);
        }

        private void digitsInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+"); // check input must be a number
            e.Handled = regex.IsMatch(e.Text);
        }

        private void startInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!startInput.Text.IsNullOrWhiteSpace())
            {
                this.rule.Start = int.Parse(startInput.Text);
            }
        }

        private void stepInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!stepInput.Text.IsNullOrWhiteSpace())
            {
                this.rule.Step = int.Parse(stepInput.Text);
            }
        }

        private void digitsInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!digitsInput.Text.IsNullOrWhiteSpace())
            {
                this.rule.NumOfDigits = int.Parse(digitsInput.Text);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }
    }
}
