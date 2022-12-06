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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
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
        public ProjectStatus projectStatus = new ProjectStatus();
        public bool FirstInit = true;
        public bool isAutoSaveMode = false;
        private Thread AutoSaveThread;

        private readonly int itemPerPage = 5;
        private int currentFilePage { get; set; } = 1;
        private int currentFolderPage { get; set; } = 1;


        public MainWindow(IOptionsSnapshot<RuleConfig> ruleConfig)
        {
            this._RuleConfig = ruleConfig.Value;
            this._RuleFactory = RuleFactory.GetInstance(ruleConfig);
            InitializeComponent();
        }
        private bool InitProjectFromPreviousStatus()
        {
            bool isInit = false;
            FirstInit = true;
            try
            {
                var previousStatus = GetAllJsonFile("ProjectStatus").LastOrDefault();
                if (previousStatus != null)
                {
                    var projectStatusDir = Directory.GetCurrentDirectory() + $"\\ProjectStatus\\{previousStatus}";
                    var previousStatusJson = File.ReadAllText(projectStatusDir);
                    if (previousStatusJson != null)
                    {
                        var projectStatus = JsonSerializer.Deserialize<ProjectStatus>(previousStatusJson);
                        if (projectStatus != null)
                        {
                            isInit = InitProjectFromPreviousStatus(projectStatus);
                        }
                    }
                    if (isInit)
                    {
                        FirstInit = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = ex.Message,
                    Caption = "Init Project From Previous Status",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            return isInit;
        }
        private bool InitProjectFromPreviousStatus(string fileName)
        {
            bool isInit = false;
            FirstInit = true;
            try
            {
                var previousStatus = GetAllJsonFile("ProjectStatus").Where(c => c == fileName).FirstOrDefault();
                if (previousStatus != null)
                {
                    var projectStatusDir = Directory.GetCurrentDirectory() + $"\\ProjectStatus\\{previousStatus}";
                    var previousStatusJson = File.ReadAllText(projectStatusDir);
                    if (previousStatusJson != null)
                    {
                        var projectStatus = JsonSerializer.Deserialize<ProjectStatus>(previousStatusJson);
                        if (projectStatus != null)
                        {
                            isInit = InitProjectFromPreviousStatus(projectStatus);
                        }
                    }
                    if (isInit)
                    {
                        FirstInit = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = String.Format("{0}. {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : String.Empty),
                    Caption = "Init Project From Previous Status",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            return isInit;
        }
        private bool InitProjectFromPreviousStatus(ProjectStatus projectStatus)
        {
            bool isInit = false;
            FirstInit = true;
            try
            {
                if (projectStatus != null)
                {
                    _RuleList = new ObservableCollection<IRule>();
                    if (projectStatus.RulesList != null)
                    {
                        foreach (var ruleJson in projectStatus.RulesList)
                        {
                            var rule = _RuleFactory.CreateRuleInstance(ruleJson);
                            _RuleList.Add(rule);
                        }
                    }
                    // set current file page and folder page
                    this.currentFilePage = projectStatus.currentFilePage;
                    FilePagination.PageIndex = projectStatus.currentFilePage;
                    this.currentFolderPage = projectStatus.currentFolderPage;
                    FolderPagination.PageIndex = projectStatus.currentFolderPage;
                    // set project resolution
                    this.Height = projectStatus.Height;
                    this.Width = projectStatus.Width;
                    // set rule combo box
                    this.RuleComboBox.ItemsSource = _RuleFactory.GetAllRuleNames();
                    // set rule list
                    this.RuleList.ItemsSource = _RuleList;
                    // set text and item source for preset combo box
                    this.PresetComboBox.SelectedItem = projectStatus.Preset;
                    this._PresetComboBox = new ObservableCollection<string>(GetAllJsonFile("Preset").Select(c => c.getFileName()).OrderBy(c => c).ToList());
                    this.PresetComboBox.ItemsSource = this._PresetComboBox;
                    // set text for preset name input
                    this.presetNameInput.Text = projectStatus.Preset;
                    // set file
                    this.FileList = projectStatus.FileList;
                    update_Filepage();
                    // set folder
                    this.FolderList = projectStatus.FolderList;
                    update_Folderpage();

                    isInit = true;
                }
                if (isInit)
                {
                    FirstInit = false;
                }
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = String.Format("{0}. {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : String.Empty),
                    Caption = "Init Project From Previous Status",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            return isInit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._RuleList = new ObservableCollection<IRule>();

            RuleComboBox.ItemsSource = _RuleFactory.GetAllRuleNames();
            RuleList.ItemsSource = this._RuleList;

            _PresetComboBox = new ObservableCollection<string>(GetAllJsonFile("Preset").Select(c => c.getFileName()).OrderBy(c => c).ToList());
            PresetComboBox.ItemsSource = this._PresetComboBox;

            // Set status file and folder to 0
            NumberOfFiles.DataContext = 0;
            NumberOfBatchingFiles.DataContext = 0;
            NumberOfErrorFiles.DataContext = 0;

            var canInit = InitProjectFromPreviousStatus("ProjectStatus.json");
            if (canInit)
            {
                FirstInit = false;
                return;
            }
        }

        private void New_Project_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Project_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var msgError = string.Empty;
                ProjectStatus? ps = null;
                try
                {
                    string sStatus = File.ReadAllText(filePath);
                    ps = JsonSerializer.Deserialize<ProjectStatus>(sStatus);
                }
                catch (Exception ex)
                {
                    msgError = ex.Message;
                }
                if (ps == null)
                {
                    HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = msgError,
                        Caption = "Open Project Failed",
                        Button = MessageBoxButton.OK,
                        IconBrushKey = ResourceToken.AccentBrush,
                        IconKey = ResourceToken.ErrorGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                }
                else
                {
                    var canInit = InitProjectFromPreviousStatus(ps);
                }
            }
        }

        private void Save_Project_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveProjectStatus();
        }

        private void Save_As_Project_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Filter = "JSON files(*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                var saveAs = SaveProjectStatus();
                if (saveAs != null)
                {
                    File.WriteAllText(filePath, saveAs);
                }
            }
        }

        /// <summary>
        /// StartBatching_Click: handler cho sự kiện click vào Button Start Bactching
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StartBatching_Click(object sender, RoutedEventArgs e)
        {
            string targetFolder = String.Empty;
            try
            {
                #region validate

                if (_RuleList == null || _RuleList.Count < 1)
                {
                    throw new Exception("Rule list is empty or null");
                }
                if (!_RuleList.Any(rule => rule.IsInUse))
                {
                    throw new Exception("No Rule has been selected to rename");
                }
                var isFileListAndFolderListEmpty = (FileList == null || FileList.Count == 0) && (FolderList == null || FolderList.Count == 0);
                if (isFileListAndFolderListEmpty)
                {
                    throw new Exception("Both File list and Folder list is empty. Please select at least one file or folder.");
                }

                #endregion

                #region Chọn folder                

                CommonOpenFileDialog folderBrowserDialog = new CommonOpenFileDialog();
                folderBrowserDialog.IsFolderPicker = true;
                if (folderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    targetFolder = folderBrowserDialog.FileName;
                }

                #endregion

                #region Copy & rename

                foreach (var file in FileList)
                {
                    string destinationFilePath = System.IO.Path.Combine(targetFolder, file.NewFileName);
                    var newFileInfo = new FileInfo(destinationFilePath);
                    if (newFileInfo.Exists)
                    {
                        newFileInfo.Delete();
                    }
                    using (FileStream SourceStream = File.Open(file.FilePath, FileMode.Open))
                    {
                        using (FileStream DestinationStream = File.Create(destinationFilePath))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                }

                foreach (var folder in FolderList)
                {
                    BatchFolderRecursively(folder, new DirectoryInfo(targetFolder));
                }

                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "Batching Successfully",
                    Caption = "Batching",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.SuccessBrush,
                    IconKey = ResourceToken.SuccessGeometry,
                    StyleKey = "MessageBoxCustom"
                });

                #endregion
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = ex.Message,
                    Caption = ex.InnerException?.Message,
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            return;
        }

        private async void BatchFolderRecursively(MyFolder folder, DirectoryInfo destinationDirectoryInfo)
        {
            try
            {
                if (!destinationDirectoryInfo.Exists)
                {
                    destinationDirectoryInfo.Create();
                }

                var sourceDirectoryInfo = new DirectoryInfo(folder.FolderPath);
                var destinationSubDirectory = destinationDirectoryInfo.CreateSubdirectory(folder.NewFolderName);
                CopyAll(sourceDirectoryInfo, destinationSubDirectory);
                //var subDirectorieInfoArray = sourceDirectoryInfo.GetDirectories();
                //if (subDirectorieInfoArray.Length < 1)
                //{
                //    return;
                //}

                //foreach (var subDirectoryInfo in subDirectorieInfoArray)
                //{
                //    var subFolder = FolderList.First(folder => String.Equals(subDirectoryInfo.FullName, folder.FolderPath));    // lấy thông tin sub folder.
                //    if (subFolder != null)
                //    {
                //        var destinationSubDirectory = destinationDirectoryInfo.CreateSubdirectory(subFolder.NewFolderName);     // create new sub folder in target
                //        BatchFolderRecursively(subFolder, destinationSubDirectory);
                //    }
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                // Copy each file into it's new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(System.IO.Path.Combine(target.ToString(), fi.Name), true);
                }

                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }

        /// <author>Nguyen Tuan Khanh</author>
        /// <summary>
        /// ApplyRuleToFiles: Apply tát cả rule vào danh sách các file.
        /// </summary>
        private void ApplyRulesToFiles(object sender = null)
        {
            try
            {
                if (this._RuleList == null || this._RuleList.Count < 1)
                {
                    RestoreFilesName();
                    return;
                }
                var inUseRuleList = this._RuleList.Where(rule => rule.IsInUse).ToList(); // get tất cả các rule ở trạng thái in use.
                if (inUseRuleList.Count < 1)
                {
                    RestoreFilesName();
                    return;
                }
                if (this.FileList == null || this.FileList.Count < 1)
                {
                    return;
                }
                var count = this.FileList.Count;
                var fileNameList = this.FileList.Select(file => file.FileName).ToList();
                foreach (var rule in inUseRuleList)
                {
                    var tempList = rule.Apply(fileNameList, null);
                    fileNameList.Clear();
                    fileNameList.AddRange(tempList);
                }
                for (int i = 0; i < count; i++)
                {
                    this.FileList[i].NewFileName = fileNameList[i];
                }
            }
            catch (Exception ex)
            {
                handleException(ex.Message, sender);
            }
        }
        private async void handleException(string exceptionMessage, object sender)
        {
            await Task.Run(() =>
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = exceptionMessage,
                    Caption = "Apply Rule Error",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });

            });

            CheckBox b = sender as CheckBox;
            IRule rule = b.CommandParameter as IRule;

            rule.IsInUse = false;
            RuleList.SelectedItem = rule;
        }

        /// <author>Nguyen Tuan Khanh</author>
        /// <summary>
        /// ApplyRuleToFolders: Apply tất cả rules vào danh sách các folder.
        /// </summary>
        private void ApplyRuleToFolders(object sender = null)
        {
            try
            {
                if (this._RuleList == null || this._RuleList.Count < 1)
                {
                    return;
                }
                var inUseRuleList = this._RuleList.Where(rule => rule.IsInUse).ToList();
                if (inUseRuleList.Count < 1)
                {
                    return;
                }
                if (this.FolderList == null || this.FolderList.Count < 1)
                {
                    return;
                }
                var count = this.FolderList.Count;
                var folderNameList = this.FolderList.Select(folder => folder.FolderName).ToList();
                foreach (var rule in inUseRuleList)
                {
                    var tempList = rule.Apply(folderNameList, null);
                    folderNameList.Clear();
                    folderNameList.AddRange(tempList);
                }
                for (int i = 0; i < count; i++)
                {
                    this.FolderList[i].NewFolderName = folderNameList[i];
                }
            }
            catch (Exception ex)
            {
                handleException(ex.Message, sender);
            }
        }

        private void RestoreFilesName()
        {
            foreach (var file in this.FileList)
            {
                file.NewFileName = file.FileName;
            }
        }

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FirstInit)
                {
                    return;
                }
                var presetSelected = PresetComboBox.SelectedValue;
                if (presetSelected == null)
                {
                    return;
                }
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
                presetNameInput.Text = presetSelected.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }

        private void Clear_All_Preset_Button_Click(object sender, RoutedEventArgs e)
        {
            presetNameInput.Text = string.Empty;
            _RuleList.Clear();
            PresetComboBox.SelectedIndex = -1;
        }

        private void RuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRuleName = (string)RuleComboBox.SelectedItem;
            var selectedRuleInstance = _RuleFactory.CreateRuleInstance(selectedRuleName);
            checkAndAddRule(selectedRuleName, selectedRuleInstance);
        }

        private void checkAndAddRule(string selectedRuleName, IRule selectedRule)
        {
            var checkRuleExist = false;

            foreach (var item in _RuleList)
            {
                if (item.Name == selectedRuleName)
                {
                    checkRuleExist = true;
                    break;
                }
            }

            if (checkRuleExist)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "Your selected rule has already chosen!",
                    Caption = "Rule duplication",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            else
                _RuleList.Add(selectedRule);
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
                        SaveJson(presetDirectory + presetFileName, presetToJson);
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
        private void SaveJson(string filePath, string data, bool isOverride = false)
        {
            try
            {
                var folderName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                if (folderName == null)
                {
                    throw new Exception("Folder name not existed");
                }
                var listFile = GetAllJsonFile(folderName);
                var fileName = System.IO.Path.GetFileName(filePath);
                if (!listFile.Contains(fileName, StringComparer.OrdinalIgnoreCase) || isOverride == true)
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
            catch (DirectoryNotFoundException dnfe)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            catch (IOException ioee)
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }

        }

        /// <summary>
        /// Lấy tất cả các file .json có trong FolderName có đường dẫn BatchRename/Batch_Rename/BIN/FolderName
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        private List<string> GetAllJsonFile(string FolderName)
        {
            List<string> result = new List<string>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + FolderName + "\\");
                FileInfo[] fi = di.GetFiles("*.json");
                if (fi != null && fi.Length > 0)
                {
                    foreach (var file in fi)
                    {
                        result.Add(file.Name);
                    }
                }
            }
            catch (DirectoryNotFoundException dnfe)
            {
                System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"\\{FolderName}\\");
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
            ApplyRulesToFiles(sender);
            ApplyRuleToFolders(sender);
        }

        private void Use_Rule_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyRulesToFiles(sender);
            ApplyRuleToFolders(sender);
        }

        private void Remove_Rule_Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            IRule rule = b.CommandParameter as IRule;
            _RuleList.Remove(rule);
            ApplyRulesToFiles();
            ApplyRuleToFolders();
        }

        private void AddBatchingFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string item in openFileDialog.FileNames)
                {
                    addFileToListView(item, out bool checkFileExist);

                    if (checkFileExist)
                    {
                        AddBatchingFile_Click(sender, e);
                    }
                }
            }
        }

        private void addFileToListView(string fileNamePath, out bool checkFileExist)
        {
            if (!isFileExist(fileNamePath))
            {
                if (File.Exists(fileNamePath))
                {
                    MyFile newFile = new MyFile(fileNamePath);
                    FileList.Add(newFile);
                    ApplyRulesToFiles();
                }
                else if (Directory.Exists(fileNamePath))              // thêm đệ quy khi đưa vào folder
                {
                    string[] InsideFilesList = Directory.GetFiles(fileNamePath, "*", SearchOption.AllDirectories);

                    foreach (var item in InsideFilesList)
                    {
                        addFileToListView(item, out bool isChildFileExist);
                        if (isChildFileExist) break;
                    }
                }
                checkFileExist = false;
            }
            else
            {
                checkFileExist = true;
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "Your selected file is already exist!",
                    Caption = "File already exists",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
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
                if (item.FilePath.Equals(fileNamePath))
                {
                    return true;
                }
            }
            return false;
        }

        private void ClearAllFile_Click(object sender, RoutedEventArgs e)
        {
            if (FileList.Count == 0) return;

            MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
            {
                Message = "Do you want to clear all current files?",
                Caption = "Clear All Files",
                Button = MessageBoxButton.YesNo,
                IconBrushKey = ResourceToken.AccentBrush,
                IconKey = ResourceToken.AskGeometry,
                StyleKey = "MessageBoxCustom"
            });

            switch (result)
            {
                case MessageBoxResult.Yes:
                    FileList.Clear();
                    update_Filepage();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void FileList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem)
            {
                ListViewItem? draggedItem = sender as ListViewItem;
                DataObject data = new DataObject("FILE", draggedItem?.DataContext);
                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void FileList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FILE"))
            {
                MyFile? droppedData = e.Data.GetData("FILE") as MyFile;
                MyFile? target = ((ListViewItem)(sender)).DataContext as MyFile;

                int removedIdx = FileList.IndexOf(droppedData);
                int targetIdx = FileList.IndexOf(target);
                if (removedIdx < 0 || targetIdx < 0 || removedIdx > FileList.Count || targetIdx > FileList.Count)
                {
                    return;
                }
                else if (removedIdx < targetIdx)
                {
                    FileList.Insert(targetIdx + 1, droppedData);
                    FileList.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (FileList.Count + 1 > remIdx)
                    {
                        FileList.Insert(targetIdx, droppedData);
                        FileList.RemoveAt(remIdx);
                    }
                }
            }
            update_Filepage();
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
                    addFolderToListView(item, out bool checkFolderExist);

                    if (checkFolderExist)
                    {
                        AddBatchingFolder_Click(sender, e);
                    }
                }
            }

        }

        private void addFolderToListView(string folderNamePath, out bool checkFolderExist)
        {
            if (!isFolderExist(folderNamePath))
            {
                MyFolder newFolder = new MyFolder(folderNamePath);
                FolderList.Add(newFolder);

                string[] InsideFoldersList = Directory.GetDirectories(folderNamePath, "*", SearchOption.TopDirectoryOnly);

                foreach (var item in InsideFoldersList)                     // thêm đệ quy
                {
                    addFolderToListView(item, out bool checkChildFolderExist);

                    if (checkChildFolderExist) break;
                }

                checkFolderExist = false;
                ApplyRuleToFolders();

            }
            else
            {
                checkFolderExist = true;

                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = "Your selected folder is already exist!",
                    Caption = "Folder already exists",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });

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
                if (item.FolderPath.Equals(folderNamePath))
                {
                    return true;
                }
            }

            return false;
        }

        private void ClearAllFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FolderList.Count == 0) return;

            MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
            {
                Message = "Do you want to clear all current folders?",
                Caption = "Clear All Folders",
                Button = MessageBoxButton.YesNo,
                IconBrushKey = ResourceToken.AccentBrush,
                IconKey = ResourceToken.AskGeometry,
                StyleKey = "MessageBoxCustom"
            });

            switch (result)
            {
                case MessageBoxResult.Yes:
                    FolderList.Clear();
                    update_Folderpage();
                    break;
                case MessageBoxResult.No:
                    break;
            }
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
            if (sender is ListViewItem)
            {
                ListViewItem? draggedItem = sender as ListViewItem;
                DataObject data = new DataObject("FOLDER", draggedItem.DataContext);
                DragDrop.DoDragDrop(draggedItem, data, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void FolderList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FOLDER"))
            {
                MyFolder? droppedData = e.Data.GetData("FOLDER") as MyFolder;
                MyFolder? target = ((ListViewItem)(sender)).DataContext as MyFolder;

                int removedIdx = FolderList.IndexOf(droppedData);
                int targetIdx = FolderList.IndexOf(target);
                if (removedIdx < 0 || targetIdx < 0 || removedIdx > FolderList.Count || targetIdx > FolderList.Count)
                {
                    return;
                }
                else if (removedIdx < targetIdx)
                {
                    FolderList.Insert(targetIdx + 1, droppedData);
                    FolderList.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (FolderList.Count + 1 > remIdx)
                    {
                        FolderList.Insert(targetIdx, droppedData);
                        FolderList.RemoveAt(remIdx);
                    }
                }
            }
            update_Folderpage();
        }

        private void DragOverFileList(object sender, DragEventArgs e)
        {

        }

        private void DropFileList(object sender, DragEventArgs e)
        {
            string[]? droppedFilePaths = e.Data?.GetData(DataFormats.FileDrop, true) as string[];
            if (droppedFilePaths != null)
            {
                foreach (string filePath in droppedFilePaths)
                {
                    addFileToListView(filePath, out bool checkFileExist);
                }
            }
        }

        private void page_FilePageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            currentFilePage = e.Info;
            update_Filepage();
        }

        private void DragOverFolderList(object sender, DragEventArgs e)
        {

        }

        private void DropFolderList(object sender, DragEventArgs e)
        {
            string[]? droppedFolderPaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            if (droppedFolderPaths != null)
            {

                foreach (string folderPath in droppedFolderPaths)
                {
                    addFolderToListView(folderPath, out bool checkFolderExist);
                }
            }
        }

        /// <author>Do Thai Duy</author>
        /// <edit>Nguyen Tuan Khanh</edit>
        /// <summary>
        /// Xử lý sự kiện check vào auto save
        /// Khanh: đặt chế độ autosave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auto_Save_Checked(object sender, RoutedEventArgs e)
        {
            this.isAutoSaveMode = true;
            if (AutoSaveThread == null)
            {
                try
                {

                    AutoSaveThread = new Thread(new ThreadStart(() =>
                    {
                        AutoSaveProjectStatus();
                    }));
                    AutoSaveThread.IsBackground = true;
                    AutoSaveThread.Start();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        /// <author>Do Thai Duy</author>
        /// <edit>Nguyen Tuan Khanh</edit>
        /// <summary>
        ///  Xử lý sự kiện uncheck vào auto save
        /// Khanh: tắt chế độ autosave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auto_Save_UnChecked(object sender, RoutedEventArgs e)
        {

            this.isAutoSaveMode = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveProjectStatus();
        }

        /// <author>Truong Cong Thanh, Nguyen Tuan Khanh</author>
        /// <summary>
        /// Lưu lại trạng thái của project.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private string SaveProjectStatus()
        {
            string jsonResult = string.Empty;
            try
            {
                ProjectStatus projectStatus = new ProjectStatus();
                projectStatus.Width = this.Width;
                projectStatus.Height = this.Height;
                projectStatus.currentFolderPage = this.currentFolderPage;
                projectStatus.currentFilePage = this.currentFilePage;
                projectStatus.FileList = this.FileList;
                projectStatus.FolderList = this.FolderList;
                projectStatus.RulesList = new List<RuleJson>();
                projectStatus.Preset = PresetComboBox.SelectedValue != null ? PresetComboBox.SelectedValue.ToString() : string.Empty;
                if (_RuleList != null)
                {
                    foreach (var rule in _RuleList)
                    {
                        projectStatus.RulesList.Add(new RuleJson()
                        {
                            Name = rule.Name,
                            Json = rule.ToJson()
                        });
                    }
                }

                var projectStatusJson = JsonSerializer.Serialize(projectStatus);
                var fileName = Directory.GetCurrentDirectory() + @"\\ProjectStatus\\ProjectStatus.json";
                SaveJson(fileName, projectStatusJson, true);
                jsonResult = projectStatusJson;
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = String.Format("{0}. {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : String.Empty),
                    Caption = "Lưu lại trạng thái của project.",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
            return jsonResult;
        }

        /// <author>Nguyen Tuan Khanh</author>
        /// <summary>
        /// Tự động lưu lại trạng thái của project
        /// </summary>
        private void AutoSaveProjectStatus()
        {
            try
            {
                while (isAutoSaveMode)
                {
                    this.Dispatcher.Invoke(() => SaveProjectStatus());
                    Thread.Sleep(60 * 1000);
                }
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = String.Format("{0}. {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : String.Empty), Caption = "Init Project From Previous Status", Button = MessageBoxButton.OK, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.ErrorGeometry, StyleKey = "MessageBoxCustom" });
            }
        }
    }
}
