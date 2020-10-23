using Microsoft.Win32;
using System.Windows;
using 

namespace PuzzleAnalyst
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
    }

    public static class ImgSt
    {
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
