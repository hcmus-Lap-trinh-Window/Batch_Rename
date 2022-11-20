using CommonModel;
using CommonModel.Model;
using Config;
using HandyControl.Data;
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
using System.Text.Json;
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
        public ObservableCollection<string> _PresetComboBox { get; private set; }
        public BindingList<MyFile> FileList = new BindingList<MyFile>();
        public BindingList<MyFolder> FolderList = new BindingList<MyFolder>();


        private readonly int itemPerPage = 5;
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

            _PresetComboBox = new ObservableCollection<string>(GetAllJsonFile("Preset").Select(c => c.getFileName()).ToList());
            PresetComboBox.ItemsSource = this._PresetComboBox;

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
            try
            {
                var presetSelected = PresetComboBox.SelectedValue;
                var presetFileName = presetNameInput.Text + ".json";
                var presetDirectory = Directory.GetCurrentDirectory() + $"\\Preset\\{presetSelected}.json";
                var presetContent = File.ReadAllText(presetDirectory);
                var jsonToPreset = JsonSerializer.Deserialize<List<RuleJson>>(presetContent);
                if (jsonToPreset != null && jsonToPreset.Count > 0)
                {
                    _RuleList = new ObservableCollection<IRule>();
                    foreach (var jsonRule in jsonToPreset)
                    {
                        var rule = _RuleFactory.CreateRuleInstance(jsonRule);
                        if (rule != null)
                        {
                            _RuleList.Add(rule);
                        }
                    }
                }
                RuleList.ItemsSource = _RuleList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
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
                if (!presetNameInput.Text.IsNullOrWhiteSpace())
                {
                    if (_RuleList != null && _RuleList.Count > 0)
                    {
                        var presetFileName = presetNameInput.Text + ".json";
                        var presetDirectory = Directory.GetCurrentDirectory() + "\\Preset\\";
                        var presetToJsonList = new List<RuleJson>();
                        foreach (var r in _RuleList)
                        {
                            presetToJsonList.Add(new RuleJson()
                            {
                                Name = r.Name,
                                Json = r.ToJson()
                            });
                        }
                        var presetToJson = JsonSerializer.Serialize(presetToJsonList);
                        SavePreset(presetDirectory + presetFileName, presetToJson);
                        if (!_PresetComboBox.Contains(presetNameInput.Text))
                        {
                            _PresetComboBox.Add(presetNameInput.Text);
                        }
                    }
                }
                else
                {
                    throw new Exception("Input Preset Name First!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }
        private void SavePreset(string filePath, string data)
        {
            var listFile = GetAllJsonFile("Preset");
            var fileName = System.IO.Path.GetFileName(filePath);
            if (!listFile.Contains(fileName, StringComparer.OrdinalIgnoreCase))
            {
                File.WriteAllText(filePath, data);
            }
            else
            {
                MessageBoxResult action = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo()
                {
                    Message = "File existed, do you want to override it?",
                    Caption = "Save preset",
                    Button = MessageBoxButton.YesNo,
                });
                if (action == MessageBoxResult.Yes)
                {
                    File.WriteAllText(filePath, data);
                }
            }
        }
        /// <summary>
        /// Lấy tất cả các file .json có trong FolderName có đường dẫn BatchRename/Batch_Rename/BIN/
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        private List<string> GetAllJsonFile(string FolderName)
        {
            List<string> result = new List<string>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + FolderName);
                FileInfo[] fi = di.GetFiles("*.json");
                if (fi != null && fi.Length > 0)
                {
                    foreach (var file in fi)
                    {
                        result.Add(file.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return result;
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
            openFileDialog.Multiselect = true;

            if(openFileDialog.ShowDialog() == true)
            {
                foreach (string item in openFileDialog.FileNames)
                {
                    addFileToListView(item);
                }
            }
        }

        private void addFileToListView(string fileNamePath)
        {
            if (!isFileExist(fileNamePath))
            {
                if(File.Exists(fileNamePath))
                {
                    MyFile newFile = new MyFile(fileNamePath);
                    FileList.Add(newFile);
                }
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
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string item in dialog.FileNames)
                {
                    addFolder(item);
                }
            }
                
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
