﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        private const string NOT_EQUAL = "NOT_EQUAL";
        private const string NOT_EXIST_LEFT_SIDE = "NOT_EXIST_LEFT_SIDE";
        private const string NOT_EXIST_RIGHT_SIDE = "NOT_EXIST_RIGHT_SIDE";
        private const string EQUAL_IMG = @"d:\pictures\equal.jpg";
        private const string RIGHT_ARROW_IMG = @"d:\pictures\rightArrow2.jpg";
        private const string LEFT_ARROW_IMG = @"d:\pictures\leftArrow.jpg";
        private const string LEFT = "LEFT";
        private const string RIGHT = "RIGHT";
        private FileInfo[] leftFiles;
        private FileInfo[] rightFiles;
        private List<FileInfo> filesToLeftPathCopy = new List<FileInfo>();
        private List<FileInfo> filesToRightPathCopy = new List<FileInfo>();
        private bool cpmSubDir = false;
        public ObservableCollection<FolderCmpItem> ListCmpFilesCollections { get { return _ListCmpFilesCollections; } }
        ObservableCollection<FolderCmpItem> _ListCmpFilesCollections = new ObservableCollection<FolderCmpItem>();
        public FoldersCompare(string leftPath, string rightPath)
        {
            InitializeComponent();
            this.leftPath = leftPath;
            this.rightPath = rightPath;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            
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

        private void CompareFilesInFolder(string leftPath, string rightPath)
        {
            
            filesToRightPathCopy.Clear();
            filesToLeftPathCopy.Clear();

            leftFiles = MyFile.GetFiles(leftPath);
            rightFiles = MyFile.GetFiles(rightPath);
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
                    
                    ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, secondFile, imgPath, EQUALLY, leftPath));

                    //delete this file in rightFiles 
                    List<FileInfo> tmp = new List<FileInfo>(rightFiles);
                    tmp.RemoveAt(index);
                    rightFiles = tmp.ToArray();
                }
                else
                {
                    imgPath = LEFT_ARROW_IMG;
                    pathToCopy = rightPath;

                    ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, null, imgPath, NOT_EXIST_RIGHT_SIDE, leftPath, pathToCopy));
                    filesToRightPathCopy.Add(file);
                }
            }

            foreach (FileInfo secondFile in rightFiles)
            {
                imgPath = RIGHT_ARROW_IMG;
                pathToCopy = leftPath;
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
                //foldersCmpList
                
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
                    //printFilesWithDeep(path, "left");
                }


                IEnumerable<string> secondDiffFirst = rightNames.Where(file => !leftNames.Contains(file));

                foreach (string dir in secondDiffFirst)
                {
                    string path = currentDirRight + "\\" + dir;
                    RenderFilesWithDeep(path, RIGHT);
                    //printFilesWithDeep(currentDirRight + "\\" + dir, "right");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clearListFilesView();
            if (cpmSubDir == true)
            {
                CompareFilesWithSubDirs(leftPath, rightPath);
            }
            else
            {
                CompareFilesInFolder(leftPath, rightPath);
            }
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

                Console.WriteLine("currentDir: {0}", currentDir);
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");

                FileInfo[] files = MyFile.GetFiles(currentDir);

                ListCmpFilesCollections.Add(cpmItem.createCmpItem(currentDir));
                foreach (FileInfo file in files)
                {
                    if (side == LEFT)
                    {
                        imgPath = LEFT_ARROW_IMG;
                        ListCmpFilesCollections.Add(cpmItem.createCmpItem(file, null, imgPath, NOT_EXIST_RIGHT_SIDE, path, "copy to Left Side"));
                        filesToRightPathCopy.Add(file);
                        //Console.WriteLine("file: \"{0}\" !== \"*************\"", file.FullName);
                    }
                    if (side == RIGHT)
                    {
                        imgPath = RIGHT_ARROW_IMG;
                        //pathToCopy = leftPath;
                        ListCmpFilesCollections.Add(cpmItem.createCmpItem(null, file, imgPath, NOT_EXIST_LEFT_SIDE, path, "copy to Right Side"));
                        filesToLeftPathCopy.Add(file);
                        //Console.WriteLine("file: \"*************\" !== \"{0}\"", file.FullName);
                    }
                }

                foreach (string str in subDir)
                {
                    dirs.Push(str);
                }
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
            }
        }


        private void foldersCmpList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FolderCmpItem itemSelected = (FolderCmpItem) foldersCmpList.SelectedItem;
            Console.WriteLine("Status: {0}, parentDir: {1}", itemSelected.status, itemSelected.parentDir);
        }

        private void CopyOneFile(FolderCmpItem itemSelected)
        {
            string sourceFile = "";
            string targetDir = "";
            string fileName;
            string destFile;
            string status = itemSelected.status;

            if (status == NOT_EXIST_LEFT_SIDE)
            {
                if (File.Exists(itemSelected.secondName))
                {
                    sourceFile = itemSelected.secondName != null ? itemSelected.secondName : null;
                    targetDir = leftPath;
                }
            }

            if (status == NOT_EXIST_RIGHT_SIDE)
            {
                if (File.Exists(itemSelected.firstName))
                {
                    sourceFile = itemSelected.firstName != null ? itemSelected.firstName : null ;
                    targetDir = rightPath;
                }
            }

            fileName = Path.GetFileName(sourceFile);
            destFile = Path.Combine(targetDir, fileName);
            QuestionModal modal = new QuestionModal(fileName, targetDir);

            bool? result = modal.ShowDialog();

            if (result.HasValue && result.Value && sourceFile != null && targetDir != null)
            {
                try
                {
                    File.Copy(sourceFile, destFile, true);
                    Console.WriteLine("File copied");
                }
                catch(Exception error)
                {
                    Console.WriteLine("Error: ", error.ToString());
                }
            }
            CompareFilesInFolder(leftPath, rightPath);
        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderCmpItem itemSelected = (FolderCmpItem)foldersCmpList.SelectedItem;

            CopyOneFile(itemSelected);

            Console.WriteLine("Status: {0}", itemSelected.status);
            if(itemSelected.firstName != null)
            {
                Console.WriteLine("itemSelected.firstName: {0}", itemSelected.firstName);
            }

            if (itemSelected.secondName != null)
            {
                Console.WriteLine("itemSelected.secondName: {0}", itemSelected.secondName);
            }
            
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

        private void SyncBtn_Click(object sender, RoutedEventArgs e)
        {
            string destFile;

            MessageBoxResult result = MessageBox.Show("Слева направо копируется \"" + filesToRightPathCopy.Count + "\" файла", "Копирование файлов", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Directory.Exists(rightPath))
                {
                    foreach (FileInfo file in filesToRightPathCopy)
                    {
                        destFile = Path.Combine(rightPath, file.Name);
                        File.Copy(file.FullName, destFile, true);
                    }
                }
                else
                {
                    MessageBox.Show("Source path \"" + rightPath + "\" does not exist!", "Copying files", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            
            result = MessageBox.Show("Справа налево копируется \"" + filesToLeftPathCopy.Count + "\" файла", "Копирование файлов", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if(Directory.Exists(leftPath))
                {
                    foreach (FileInfo file in filesToLeftPathCopy)
                    {
                        destFile = Path.Combine(leftPath, file.Name);
                        File.Copy(file.FullName, destFile, true);
                    }
                }
                else
                {
                    MessageBox.Show("Source path \"" + rightPath + "\" does not exist!", "Copying files", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            clearListFilesView();
            CompareFilesInFolder(leftPath, rightPath);
        }

        private void withSubDir_Checked(object sender, RoutedEventArgs e)
        {
            cpmSubDir = true;
        }

        private void withSubDir_Unchecked(object sender, RoutedEventArgs e)
        {
            cpmSubDir = false;
        }
    }
}