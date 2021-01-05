using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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


public class Result
{
    public int Id { get; set; }
    public string Text { get; set; }
}


namespace WpfTotalnik
{
    /// <summary>
    /// Логика взаимодействия для DifferenceFiles.xaml
    /// </summary>
    public partial class DifferenceFiles : Page
    {
        public ObservableCollection<Result> results_1 = new ObservableCollection<Result>();
        public ObservableCollection<Result> results_2 = new ObservableCollection<Result>();

        List<string[]> fileStrings_1 = new List<string[]>();
        List<string[]> fileStrings_2 = new List<string[]>();
        public DifferenceFiles()
        {
            InitializeComponent();


            lvLeft.ItemsSource = results_1;
            lvRight.ItemsSource = results_2;

            string file_1 = @"D:\\123.txt";
            string file_2 = @"D:\\321.txt";

            leftPathFile.Text = file_1;
            rightPathFile.Text = file_2;

            readFile(file_1, 1);
            readFile(file_2, 2);

            void readFile(string myFile, int numberFile)
            {
                if (File.Exists(myFile))
                {
                    // Read file using StreamReader. Reads file line by line  
                    using (StreamReader file = new StreamReader(myFile))
                    {
                        int counter = 0;
                        string ln;

                        while ((ln = file.ReadLine()) != null)
                        {
                            Result tmp = new Result { Id = counter, Text = ln };
                            if (numberFile == 1)
                            {

                                results_1.Add(tmp);
                                fileStrings_1.Add(tmp.Text.Split(' '));
                            }

                            if (numberFile == 2)
                            {
                                results_2.Add(tmp);
                                fileStrings_2.Add(tmp.Text.Split(' '));
                            }
                            //Console.WriteLine(ln);
                            counter++;
                        }
                        file.Close();
                    }
                }
            }
            for (int i = 0; i < fileStrings_1.Count; i++)
            {
                string res = "";
                for (int j = 0; j < fileStrings_2[i].Length; j++)
                {
                    if (!fileStrings_1[i].Contains(fileStrings_2[i][j]))
                    {
                        res += "'" + fileStrings_2[i][j] + "' ";
                    }
                    else
                    {
                        res += fileStrings_2[i][j] + " ";
                    }
                }
                results_2[i].Text = res;
                // Console.WriteLine("res: " + res);
            }

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            {
                FindListViewItem(lvRight);
            }
        }

        private void FindListViewItem(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                ListViewItem lv = obj as ListViewItem;
                if (lv != null)
                {
                    HighlightText(lv);
                }
                FindListViewItem(VisualTreeHelper.GetChild(obj as DependencyObject, i));
            }
        }

        private void HighlightText(Object itx)
        {
            Regex regex;
            if (itx != null)
            {
                if (itx is TextBlock)
                {
                    regex = new Regex("('\\w*')", RegexOptions.IgnoreCase);
                    TextBlock tb = itx as TextBlock;

                    string[] substrings = regex.Split(tb.Text);
                    tb.Inlines.Clear();
                    foreach (var item in substrings)
                    {
                        if (regex.Match(item).Success)
                        {
                            Run runx = new Run(item);
                            runx.Background = Brushes.LightYellow;
                            tb.Inlines.Add(runx);
                        }
                        else
                        {
                            tb.Inlines.Add(item);
                        }
                    }
                    return;
                }
                else
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(itx as DependencyObject); i++)
                    {
                        HighlightText(VisualTreeHelper.GetChild(itx as DependencyObject, i));
                    }
                }
            }
        }

        private void lvRight_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
