using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfTotalnik
{
    /// <summary>
    /// Логика взаимодействия для FoldersCompare.xaml
    /// </summary>
    ///  
 
    public partial class FoldersCompare : Window
    {
        private string leftPath;
        private string rightPath;
        private const string EQUALLY = "EQUALLY";
        private const string NOT_EQUAL_COPY_TO_LEFT = "NOT_EQUAL_COPY_TO_LEFT";
        private const string NOT_EQUAL_COPY_TO_RIGHT = "NOT_EQUAL_COPY_TO_RIGHT";
        private const string NOT_EXIST_LEFT_SIDE = "NOT_EXIST_LEFT_SIDE";
        private const string NOT_EXIST_RIGHT_SIDE = "NOT_EXIST_RIGHT_SIDE";
        private const string EQUAL_IMG = @"d:\pictures\equal.jpg";
        private const string NOT_EQUAL_IMG = @"d:\pictures\noEqual.jpg";
        private const string RIGHT_ARROW_IMG = @"d:\pictures\rightArrow.jpg";
        private const string LEFT_ARROW_IMG = @"d:\pictures\leftArrow.jpg";
        private const string LEFT = "LEFT";
        private const string RIGHT = "RIGHT";
        private FileInfo[] leftFiles;
        private FileInfo[] rightFiles;
        private List<FileInfo> filesToLeftPathCopy = new List<FileInfo>();
        private List<FileInfo> filesToRightPathCopy = new List<FileInfo>();
        public ObservableCollection<FolderCmpItem> ListCmpFilesCollections { get { return _ListCmpFilesCollections; } }
        ObservableCollection<FolderCmpItem> _ListCmpFilesCollections = new ObservableCollection<FolderCmpItem>();
        public FoldersCompare(string leftPath, string rightPath)
        {
            InitializeComponent();
            this.leftPath = leftPath;
            this.rightPath = rightPath;
        }
        private void CloseFoldersComparing(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FoldersCompareWindow_Loaded(object sender, RoutedEventArgs e)
        {
            folderPathLeft.Text = leftPath;
            folderPathRight.Text = rightPath;
        }
       

        private void clearListFilesView()
        {
            foldersCmpList.ItemsSource = null;
            foldersCmpList.Items.Clear();
            ListCmpFilesCollections.Clear();
        }

        private void updateListview()
        {
            clearListFilesView();

            if (withSubDir.IsChecked == true)
            {
                CompareFilesWithSubDirs(leftPath, rightPath);
            }
            else
            {
                CompareFilesInFolder(leftPath, rightPath);
            }
        }

        private string BuiltPathToCopy(FileInfo file, string side)
        {
            string pathTmp;
            if (side == LEFT)
            {
                pathTmp = (rightPath + file.FullName.Substring(leftPath.Length));
            }
            else
            {
                pathTmp = (leftPath + file.FullName.Substring(rightPath.Length));
            }

            return pathTmp;//.Substring(0, pathTmp.Length - file.Name.Length); ;
        }

        private void CompareFilesInFolder(string leftPath, string rightPath)
        {
            string filter = filterExtension.Text;

            filesToRightPathCopy.Clear();
            filesToLeftPathCopy.Clear();

            leftFiles = MyFile.GetFiles(leftPath, filter);
            rightFiles = MyFile.GetFiles(rightPath, filter);
            string imgPath;
            FolderCmpItem cpmItem = new FolderCmpItem();
            string pathToCopy;

            if (leftFiles.Length > 0 || rightFiles.Length > 0)
            {
                SyncBtn.IsEnabled = true;
            }

            foreach (FileInfo file in leftFiles)
            {
                int index = Array.IndexOf(rightFiles, rightFiles.FirstOrDefault(f => f.Name == file.Name));
                if (index > -1)
                {
                    FileInfo secondFile = rightFiles[index];
                    imgPath = EQUAL_IMG;

                    if(byDate.IsChecked == true)
                    {
                        string dateFile_1 = MyFile.GetFileDate(file);
                        string dateFile_2 = MyFile.GetFileDate(secondFile);

                        DateTime nowdate = DateTime.Today;
                        DateTime filedate_1 = file.LastWriteTime;
                        DateTime filedate_2 = secondFile.LastWriteTime;
                        TimeSpan comparevalue = filedate_2 - filedate_1;
                        
                        if (Math.Abs(comparevalue.Seconds) > 0)
                        {
                            imgPath = NOT_EQUAL_IMG;
                            pathToCopy = BuiltPathToCopy(file, LEFT);
                            string redItem = "red";
                            ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, secondFile, imgPath, NOT_EQUAL_COPY_TO_LEFT, rightPath, pathToCopy, redItem));
                        }
                        else
                        {
                            pathToCopy = BuiltPathToCopy(file, RIGHT);
                            ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, secondFile, imgPath, NOT_EQUAL_COPY_TO_RIGHT, leftPath));
                        }
                    }
                    else
                    {
                        ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, secondFile, imgPath, EQUALLY, leftPath));
                    }
                    
                    //delete this file in rightFiles 
                    List<FileInfo> tmp = new List<FileInfo>(rightFiles);
                    tmp.RemoveAt(index);
                    rightFiles = tmp.ToArray();
                }
                else
                {
                    imgPath = LEFT_ARROW_IMG;
                    pathToCopy = BuiltPathToCopy(file, LEFT);

                    ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, null, imgPath, NOT_EXIST_RIGHT_SIDE, leftPath, pathToCopy));
                    filesToRightPathCopy.Add(file);
                }
            }

            foreach (FileInfo secondFile in rightFiles)
            {
                imgPath = RIGHT_ARROW_IMG;
                pathToCopy = BuiltPathToCopy(secondFile, RIGHT); 
                ListCmpFilesCollections.Add(cpmItem.createCmpItem(null, secondFile, imgPath, NOT_EXIST_LEFT_SIDE, rightPath, pathToCopy));
                filesToLeftPathCopy.Add(secondFile);
            }
            foldersCmpList.ItemsSource = ListCmpFilesCollections;
        }

        private void CompareFilesWithSubDirs(string leftPath, string rightPath)
        {
            HashSet<string> dirsLeft = new HashSet<string>();
            HashSet<string> dirsRight = new HashSet<string>();

            if (!Directory.Exists(leftPath) || !Directory.Exists(rightPath))
            {
                throw new ArgumentException();
            }

            dirsLeft.Add(leftPath);
            dirsRight.Add(rightPath);

            while (dirsLeft.Count > 0 && dirsRight.Count > 0)
            {
                string currentDirLeft;
                string currentDirRight;
                string[] subDirsLeft;
                string[] subDirsRight;
                string lastFolderLeft;
                try
                {
                    currentDirLeft = dirsLeft.First();
                    currentDirRight = dirsRight.First();
                    subDirsLeft = Directory.GetDirectories(currentDirLeft);
                    subDirsRight = Directory.GetDirectories(currentDirRight);
                    lastFolderLeft = Path.GetFileName(Path.GetDirectoryName(currentDirLeft + "\\"));
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                FolderCmpItem cpmItem = new FolderCmpItem();
                
                ListCmpFilesCollections.Add(cpmItem.createCmpItem(currentDirLeft));

                CompareFilesInFolder(currentDirLeft, currentDirRight);
                Console.WriteLine("currentDirLeft: " + currentDirLeft);
                Console.WriteLine("currentDirRight: " + currentDirRight);

                if (dirsLeft.Contains(currentDirLeft))
                {
                    dirsLeft.Remove(currentDirLeft);
                }

                if (dirsRight.Contains(currentDirRight))
                {
                    dirsRight.Remove(currentDirRight);
                }

                List<string> leftSide = new List<string>(Directory.GetDirectories(currentDirLeft));
                List<string> rightSide = new List<string>(Directory.GetDirectories(currentDirRight));

                List<string> leftNames = new List<string>();
                List<string> rightNames = new List<string>();

                foreach (string dir in leftSide)
                {
                    string folderName = Path.GetFileName(Path.GetDirectoryName(dir + "\\"));
                    leftNames.Add(folderName);
                }

                foreach (string dir in rightSide)
                {
                    string folderName = Path.GetFileName(Path.GetDirectoryName(dir + "\\"));
                    rightNames.Add(folderName);
                }

                IEnumerable<string> theSameFolders = leftNames.Where(file => rightNames.Contains(file));
                foreach (string dir in theSameFolders)
                {
                    dirsLeft.Add(currentDirLeft + "\\" + dir);
                    dirsRight.Add(currentDirRight + "\\" + dir);
                }

                IEnumerable<string> firstDiffSecond = leftNames.Where(file => !rightNames.Contains(file));
                foreach (string dir in firstDiffSecond)
                {
                    string path = currentDirLeft + "\\" + dir;
                    RenderFilesWithDeep(path, LEFT);
                }


                IEnumerable<string> secondDiffFirst = rightNames.Where(file => !leftNames.Contains(file));
                foreach (string dir in secondDiffFirst)
                {
                    string path = currentDirRight + "\\" + dir;
                    RenderFilesWithDeep(path, RIGHT);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderCmpItem itemSelected = (FolderCmpItem)foldersCmpList.SelectedItem;
            if (cmpContent.IsChecked == true && itemSelected != null && itemSelected.status == EQUALLY)
            {
                if(itemSelected.firstName != null && itemSelected.secondName != null)
                {
                    Difference diff = new Difference(itemSelected.firstName, itemSelected.secondName);
                    diff.ShowDialog();
                }
            }
            updateListview();
        }

        public void RenderFilesWithDeep(string path, string side)
        {
            Stack<string> dirs = new Stack<string>(20);
            FolderCmpItem cpmItem = new FolderCmpItem();
            string imgPath;

            if (!Directory.Exists(path))
            {
                throw new ArgumentException();
            }
            dirs.Push(path);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDir = Directory.GetDirectories(currentDir);
                string filter = filterExtension.Text;

                FileInfo[] files = MyFile.GetFiles(currentDir, filter);

                ListCmpFilesCollections.Add(cpmItem.createCmpItem(currentDir));
                foreach (FileInfo file in files)
                {
                    if (side == LEFT)
                    {
                        imgPath = LEFT_ARROW_IMG;
                        string pathToCopy = BuiltPathToCopy(file, LEFT);
                        ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, null, imgPath, NOT_EXIST_RIGHT_SIDE, path, pathToCopy));
                        filesToRightPathCopy.Add(file);
                    }

                    if (side == RIGHT)
                    {
                        imgPath = RIGHT_ARROW_IMG;
                        string pathToCopy = BuiltPathToCopy(file, RIGHT);
                        ListCmpFilesCollections.Add(cpmItem.createCmpItem(null, file, imgPath, NOT_EXIST_LEFT_SIDE, path, pathToCopy));
                        filesToLeftPathCopy.Add(file);
                    }
                }

                foreach (string str in subDir)
                {
                    dirs.Push(str);
                }
            }
        }


        private void foldersCmpList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FolderCmpItem itemSelected = (FolderCmpItem) foldersCmpList.SelectedItem;
            Console.WriteLine("Status: {0}, parentDir: {1}, pathToCopy: {2}", itemSelected.status, itemSelected.parentDir, itemSelected.pathWhereNeedCopy);
        }

        private bool showQuestionModal(string filePath, string destDirPath)
        {
            QuestionModal modal = new QuestionModal(filePath, destDirPath);
           
            bool? result = modal.ShowDialog();

            return (result.HasValue && result.Value && filePath != null && destDirPath != null);
        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderCmpItem item = (FolderCmpItem)foldersCmpList.SelectedItem;
            string destDirPath = item.pathWhereNeedCopy;
            string filePath = "";

            if (item.status == NOT_EQUAL_COPY_TO_RIGHT)
            {
                filePath = item.secondName;
            }
            else if (item.status == NOT_EQUAL_COPY_TO_LEFT)
            {
                filePath = item.firstName;
            }
            else if (item.status != EQUALLY && item.status.Length != 0)
            {
                filePath = item.firstName.Length > 0 ? item.firstName : item.secondName;
            }

            if (showQuestionModal(filePath, destDirPath))
            {
                copyFile(filePath, destDirPath);
            }
            updateListview();
        }

        private void foldersCmpList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FolderCmpItem itemSelected = (FolderCmpItem)foldersCmpList.SelectedItem;
            if(itemSelected != null && itemSelected.status != EQUALLY && itemSelected.status.Length != 0)
            {
                CopyBtn.IsEnabled = true;
            }
            else
            {
                CopyBtn.IsEnabled = false;
            }
        }

        private void CompareMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            CompareFilesInFolder(leftPath, rightPath);
        }

        private void CopyMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyBtn_Click(sender, e);
        }

        
        private void copyFile(string filePath, string destDirPath)
        {
            try
            {
                int ind = destDirPath.LastIndexOf("\\");
                string nameFolder = destDirPath.Substring(0, ind);
                if (!Directory.Exists(nameFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destDirPath);
                    di.Delete();
                }
                
                File.Copy(filePath, destDirPath, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        private void SyncBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Слева направо копируется \"" + filesToRightPathCopy.Count + "\" файла", "Копирование файлов", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (FolderCmpItem item in ListCmpFilesCollections)
                {
                    string filePath = item.firstName.Length > 0 ? item.firstName : item.secondName;
                    string destDirPath = item.pathWhereNeedCopy;
                    if (item.status != EQUALLY && item.status.Length != 0)
                    {
                        copyFile(filePath, destDirPath);
                    }
                    else if (byDate.IsChecked == true && item.status == EQUALLY)
                    {
                        copyFile(filePath, destDirPath);
                    }
                }
            }

            updateListview();
        }
    }
}
