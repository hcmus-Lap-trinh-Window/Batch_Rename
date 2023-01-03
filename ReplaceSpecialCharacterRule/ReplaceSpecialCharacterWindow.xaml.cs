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
using CommonModel;

namespace RuleWindow
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ReplaceSpecialCharacterWindow : UserControl
    {
        private ReplaceSpecialCharacterRule _replaceSpecialCharacter;
        public ReplaceSpecialCharacterWindow(ref ReplaceSpecialCharacterRule replaceSpecialCharacterRule)
        {
            _replaceSpecialCharacter = replaceSpecialCharacterRule;
            InitializeComponent();
        }

        private void replacingCharacter_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._replaceSpecialCharacter.ReplaceCharacter = ReplacingCharacter.Text;
        }

        private void intoCharacter_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._replaceSpecialCharacter.IntoCharacter = IntoCharacter.Text;
        }

        private void ValidationTextBox1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|\\ ]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ValidationTextBox2(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|\\ ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
