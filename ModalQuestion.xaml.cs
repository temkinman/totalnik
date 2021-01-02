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
using System.Windows.Shapes;

namespace WpfTotalnik
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class QuestionModal : Window
    {
        string fileNameSource;
        string folderDest;
        public QuestionModal(string fileNameSource, string folderDest)
        {
            InitializeComponent();
            this.fileNameSource = fileNameSource;
            this.folderDest = folderDest;
        }

        private void question_Loaded(object sender, RoutedEventArgs e)
        {
            question.Text = "Копировать файл \"" + fileNameSource + "\" в папку:";
            targetPath.Text = folderDest;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
