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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RuleComboBox.ItemsSource = _RuleFactory.GetAllRuleNames();
        }

        private void New_Project_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Project_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Project_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_As_Project_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartBatching_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartBatchingToFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Clear_All_Preset_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Browse_Rule_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_All_Rule_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_Rule_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void All_Rule_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveRule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RuleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Use_Rule_Checkbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Use_Rule_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_Rule_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {

        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddBatchingFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearAllFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openThisFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openInFileExplorer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteFileMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void FileList_Drop(object sender, DragEventArgs e)
        {

        }

        private void AddBatchingFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearAllFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void page_FolderPageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {

        }

        private void openInFolderExplorer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteFolderMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FolderList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void FolderList_Drop(object sender, DragEventArgs e)
        {

        }

        private void DragOverFilePage(object sender, DragEventArgs e)
        {

        }

        private void DragOverFileList(object sender, DragEventArgs e)
        {

        }

        private void DropFileList(object sender, DragEventArgs e)
        {

        }

        private void page_FilePageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {

        }

        private void DragOverFolderPage(object sender, DragEventArgs e)
        {

        }

        private void DragOverFolderList(object sender, DragEventArgs e)
        {

        }

        private void DropFolderList(object sender, DragEventArgs e)
        {

        }

        private void Auto_Save_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Auto_Save_UnChecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
