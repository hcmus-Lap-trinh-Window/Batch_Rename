﻿using CommonModel;
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
    /// Interaction logic for ChangeExtensionRuleWindow.xaml
    /// </summary>
    public partial class ChangeExtensionRuleWindow : UserControl
    {
        ChangeExtensionRule rule;
        public ChangeExtensionRuleWindow(ref ChangeExtensionRule rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }

        private void extensionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.rule.Extension  = extensionInput.Text;
        }
    }
}