using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Input;


namespace WpfTotalnik
    
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        private const string LEFT = "LEFT";
        private const string RIGHT = "RIGHT";
        private const string SLASHES = "\\";
        private string leftPath = "d:\\c#\\1";
        private string rightPath = "d:\\c#\\2";
        private List<Button> leftDriveButtons = new List<Button>();
        private List<Button> rightDriveButtons = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void changeDrive(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int idLeftBtn = leftDriveButtons.IndexOf(button);
            int idRightBtn = rightDriveButtons.IndexOf(button);
            
            String path = button.ToString().Substring(button.ToString().Length - 2) + SLASHES;

            if (idLeftBtn > -1)
            {
                leftPath = path;
                textBoxLeft.Text = path;
                BuildList(leftPath, leftListFiles);
            }

            if (idRightBtn > -1)
            {
                rightPath = path;
                textBoxRight.Text = path;
                BuildList(rightPath, rightListFiles);
            }
        }

        private void drawDrivesBtn(string side)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                string name = drive.Name.ToString().Replace("\\", "").ToLower();
                if (side == LEFT)
                {
                    Button tempButton = createBtnDrive(name);
                    leftDriveButtons.Add(tempButton);
                    leftDrives.Children.Add(tempButton);
                }

                if (side == RIGHT)
                {
                    Button tempButton = createBtnDrive(name);
                    rightDriveButtons.Add(tempButton);
                    rightDrives.Children.Add(tempButton);
                }
            }
        }

        private Button createBtnDrive(string text)

        {
            Button btn = new Button
            {
                Height = 20,
                Width = 30
            };

            Thickness margin = btn.Margin;
            
            margin.Right = 5;
            btn.Margin = margin;
            btn.Content = text;
            btn.Background = new SolidColorBrush(Colors.LightGray);

            return btn;
        }

        
        private ImageSource getIcon(string path)
        {
            Icon icon = ShellIcon.GetSmallIcon(path);

            System.Windows.Controls.Image img = new System.Windows.Controls.Image
            {
                Source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions())
            };
            return img.Source;
        }

        private void BuildList(string path, ListView listFiles)
        {
            listFiles.ItemsSource = null;
            listFiles.Items.Clear();
            var list = new System.Collections.Generic.List<MyViewItem>();
            
            foreach (DirectoryInfo dir in MyFile.GetFolders(path))
            {
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    continue;
                }
                
                listFiles.View = MyGridView.CreateGridView();
                list.Add(new MyViewItem { 
                    MyIcon = getIcon(dir.FullName),
                    Name = dir.Name, Type = "<DIR>",
                    Size = " ",
                    Date = dir.LastAccessTime.ToShortDateString() + " " + dir.LastAccessTime.ToShortTimeString() });
            }
            
            foreach (FileInfo file in MyFile.GetFiles(path))
            {
                if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    continue;
                }

                listFiles.View = MyGridView.CreateGridView();
                list.Add(new MyViewItem {
                    MyIcon = getIcon(file.FullName),
                    Name = file.Name,
                    Type = file.Extension.Replace(".", ""),
                    Size = Math.Round((file.Length / 1024f)).ToString() + " Kb",
                    Date = file.LastAccessTime.ToShortDateString() + " " + file.LastAccessTime.ToShortTimeString() });
            }

            string pathTmp;
            if(listFiles.Name == "leftListFiles")
            {
                pathTmp = leftPath;
            }
            else
            {
                pathTmp = rightPath;
            }

            DirectoryInfo DirInfo = new DirectoryInfo(pathTmp);

            MyViewItem noRootItem = new MyViewItem
            {
                MyIcon = null,
                Name = ". . .",
                Type = "",
                Size = "",
                Date = ""
            };

            if (DirInfo.Root.ToString() != DirInfo.FullName.ToString())
            {
                listFiles.View = MyGridView.CreateGridView();
                list.Insert(0, noRootItem);
            }
            listFiles.ItemsSource = list;
            listFiles.SelectedIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildList(rightPath, rightListFiles);
            BuildList(leftPath, leftListFiles);
            textBoxLeft.Text = leftPath;
            textBoxRight.Text = rightPath;
            drawDrivesBtn(LEFT);
            drawDrivesBtn(RIGHT);
            leftDriveButtons.ForEach(x => x.Click += changeDrive);
            rightDriveButtons.ForEach(x => x.Click += changeDrive);
            
        }

        private void CompareFolders(object sender, RoutedEventArgs e)
        {
            FoldersCompare folderCompare = new FoldersCompare(leftPath, rightPath);
            folderCompare.ShowDialog();
        }

        private bool isFile(string path)
        {
            
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                return (attr & FileAttributes.Directory) != FileAttributes.Directory;
            }
            catch(Exception error)
            {
                Console.WriteLine("Error {0}", error.Message);
            }

            return false;
        }
     
        private void changePath(string path, string side)
        {
            if (side == LEFT)
            {
                leftPath = path;
                textBoxLeft.Text = path;
            }

            if (side == RIGHT)
            {
                rightPath = path;
                textBoxRight.Text = path;
            }
        }
        private void itemActivate(MyViewItem item, ListView listFiles, string side)
        {
            string path = side == LEFT ? leftPath : rightPath;
            if (item != null & item.Name == ". . .")
            {
                int ind = path.LastIndexOf("\\");
                if (path.Length > 4 & ind > -1 & path[path.Length - 1] != '\\')
                {
                    path = path.Substring(0, ind);
                }

                if(path.Length < 3)
                {
                    path += SLASHES;
                }
                
                changePath(path, side);
                BuildList(path, listFiles);
                return;
            }

            if (item != null)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(path);

                if (DirInfo.Root.ToString() != DirInfo.FullName.ToString())
                {
                   path += SLASHES;
                }

                string filePath = path + item.Name;

                if (isFile(filePath))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.StackTrace);
                    }
                }
                else
                {
                    path += item.Name;
                    changePath(path, side);
                    BuildList(path, listFiles);
                }
            }
        }
        private void leftListFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MyViewItem item = (MyViewItem)leftListFiles.SelectedItem;

            itemActivate(item, leftListFiles, LEFT);
        }

        private void leftListFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MyViewItem item = (MyViewItem)leftListFiles.SelectedItem;
                itemActivate(item, leftListFiles, LEFT);
            }
        }

        private void rightListFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MyViewItem item = (MyViewItem)rightListFiles.SelectedItem;

            itemActivate(item, rightListFiles, RIGHT);
        }

        private void rightListFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MyViewItem item = (MyViewItem)rightListFiles.SelectedItem;
                itemActivate(item, rightListFiles, RIGHT);
            }
        }
    }
}
