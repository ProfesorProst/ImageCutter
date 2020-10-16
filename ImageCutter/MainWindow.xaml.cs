using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace ImageCutter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " files (*.jpg)|*.jpg|file (*.png)|*.png";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
                programPath.Text = openFileDialog.FileName;
        }

        private void btnPazl_Click(object sender, RoutedEventArgs e)
        {
            ImageModel imageModel = new ImageModel(programPath.Text);
            var bitmaps = imageModel.PuzzleImage(Convert.ToInt32(programCol.Text),Convert.ToInt32(programRow.Text));

            WindowBoard window1 = new WindowBoard(bitmaps, Convert.ToInt32(programCol.Text), Convert.ToInt32(programRow.Text));
            window1.Show();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
