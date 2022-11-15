using Batch_Rename_App.Models;
using CommonModel;
using Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class MainWindow : Window
    {
        private readonly RuleFactory _RuleFactory;
        private readonly RuleConfig _RuleConfig;
        public ObservableCollection<IRule> _RuleList { get; private set; }
        public BindingList<MyFile> FileList = new BindingList<MyFile>();
        public BindingList<MyFolder> FolderList = new BindingList<MyFolder>();


        private readonly int itemPerPage = 2;
        private int currentFilePage = 1;
        private int currentFolderPage = 1;


        public MainWindow(IOptionsSnapshot<RuleConfig> ruleConfig)
        {
            this._RuleConfig = ruleConfig.Value;
            this._RuleFactory = RuleFactory.GetInstance(ruleConfig);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._RuleList = new ObservableCollection<IRule>();
            
            RuleComboBox.ItemsSource = _RuleFactory.GetAllRuleNames();
            RuleList.ItemsSource = this._RuleList;

            // Set status file and folder to 0
            NumberOfFiles.DataContext = 0;
            NumberOfBatchingFiles.DataContext = 0;
            NumberOfErrorFiles.DataContext = 0;

            NumberOfFolders.DataContext = 0;
            NumberOfBatchingFolders.DataContext = 0;
            NumberOfErrorFolders.DataContext = 0;
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

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Clear_All_Preset_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRuleName = (string)RuleComboBox.SelectedItem;
            var selectedRuleInstance = _RuleFactory.CreateRuleInstance(selectedRuleName);
            _RuleList.Add(selectedRuleInstance);
        }

        private void Browse_Rule_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveRule_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == true)
            {
                addFileToListView(openFileDialog.FileName);
            }
        }

        private void addFileToListView(string fileNamePath)
        {
            if (!isFileExist(fileNamePath))
            {
                MyFile newFile = new MyFile(fileNamePath);
                FileList.Add(newFile);
            }
            update_Filepage();
        }

        private void update_Filepage()
        {
            FilePagination.MaxPageCount = (int)Math.Ceiling(FileList.Count() * 1.0 / itemPerPage);
            IEnumerable<MyFile> subFileList = FileList.Skip((currentFilePage - 1) * itemPerPage).Take(itemPerPage);
            fileList.ItemsSource = subFileList;

            int batchingSuccess = 0;
            int batchingError = 0;
            for (int i = 0; i < FileList.Count(); i++)
            {
                if (FileList[i].FileStatus.Contains("Error Files"))
                {
                    batchingError++;
                }
                else if (FileList[i].FileStatus.Contains("Batching Successful"))
                {
                    batchingSuccess++;
                }
            }
            NumberOfFiles.DataContext = FileList.Count();
            NumberOfBatchingFiles.DataContext = batchingSuccess;
            NumberOfErrorFiles.DataContext = batchingError;
        }

        private bool isFileExist(string fileNamePath)
        {
            foreach (MyFile item in FileList)
            {
                if(item.FilePath.Equals(fileNamePath))
                {
                    return true;
                }                
            }
            return false;
        }

        private void ClearAllFile_Click(object sender, RoutedEventArgs e)
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
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                addFolder(dialog.FileName);
        }

        private void addFolder(string folderNamePath)
        {
            if (!isFolderExist(folderNamePath))
            {
                MyFolder newfolder = new MyFolder(folderNamePath);
                FolderList.Add(newfolder);
            }
            update_Folderpage();
        }

        private void update_Folderpage()
        {
            FolderPagination.MaxPageCount = (int)Math.Ceiling(FolderList.Count() * 1.0 / itemPerPage);
            IEnumerable<MyFolder> subFolderList = FolderList.Skip((currentFolderPage - 1) * itemPerPage).Take(itemPerPage);
            folderList.ItemsSource = subFolderList;

            int batchingSuccess = 0;
            int batchingError = 0;
            for (int i = 0; i < FolderList.Count(); i++)
            {
                if (FolderList[i].FolderStatus.Contains("Error Folders"))
                {
                    batchingError++;
                }
                else if (FolderList[i].FolderStatus.Contains("Batching Successful"))
                {
                    batchingSuccess++;
                }
            }
            NumberOfFolders.DataContext = FolderList.Count();
            NumberOfBatchingFolders.DataContext = batchingSuccess;
            NumberOfErrorFolders.DataContext = batchingError;
        }

        private bool isFolderExist(string folderNamePath)
        {
            foreach (MyFolder item in FolderList)
            {
                if(item.FolderPath.Equals(folderNamePath))
                {
                    return true;
                }
            }

            return false;
        }

        private void ClearAllFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void page_FolderPageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            currentFolderPage = e.Info;
            update_Folderpage();
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
            currentFilePage = e.Info;
            update_Filepage();
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
