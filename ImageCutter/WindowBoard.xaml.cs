using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageCutter
{
    /// <summary>
    /// Interaction logic for WindowBoard.xaml
    /// </summary>
    public partial class WindowBoard : Window
    {
        public WindowBoard(List<System.Drawing.Image> bitmaps, int row, int col)
        {
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            InitializeComponent();
            borderStart.Rows = row;
            borderStart.Columns = col;

            borderEnd.Rows = row;
            borderEnd.Columns = col;

            SetImages(bitmaps, row, col);

            borderStart.Width = borderStart.Width;
            borderStart.Height = borderStart.Height;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;

        }

        private void SetImages(List<System.Drawing.Image> bitmaps, int row, int col)
        {
            bitmaps = ImageModelStatic.Shuffle(bitmaps);

            int x = 0, y = 0, bitmapsIter = 0;

            Bitmap bmp = new Bitmap(bitmaps.First().Width, bitmaps.First().Height);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, bitmaps.First().Width, bitmaps.First().Height);
                graph.FillRectangle(System.Drawing.Brushes.White, ImageSize);
            }

            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    var imageControl = new System.Windows.Controls.Image();
                    imageControl.Source = ImageModelStatic.ToWpfImage(bitmaps[bitmapsIter]);
                    imageControl.SetValue(TagProperty, bitmaps[bitmapsIter].Tag.ToString());
                    imageControl.SetValue(Grid.RowProperty, x);
                    imageControl.SetValue(Grid.ColumnProperty, y);
                    imageControl.MouseDown += new MouseButtonEventHandler(Image_MouseDown);
                    borderStart.Children.Add(imageControl);

                    var imageControl1 = new System.Windows.Controls.Image();
                    imageControl1.Source = ImageModelStatic.ToWpfImage(bmp);
                    imageControl1.SetValue(Grid.RowProperty, x);
                    imageControl1.SetValue(Grid.ColumnProperty, y);
                    imageControl1.AllowDrop = true;
                    imageControl1.Drop += new DragEventHandler(Image_Drop);
                    imageControl1.SetValue(TagProperty, "");

                    borderEnd.Children.Add(imageControl1);
                    bitmapsIter++;
                }
            }
        }

        public static System.Windows.Controls.Image global_sender;
        private void Image_Drop(object sender, DragEventArgs e)
        {
            ((System.Windows.Controls.Image)sender).Source = global_sender.Source;
            ((System.Windows.Controls.Image)sender).Tag = global_sender.Tag;

            CheckWin();
        }

        private void CheckWin()
        {
            bool win = true;
            int columns = borderEnd.Columns;

            foreach (System.Windows.Controls.Image img in borderEnd.Children)
            {
                var name = ((img as System.Windows.Controls.Image).Tag as string).Split('_');
                if (name.First() == "") return;
                int col = Convert.ToInt32(name.First());
                int row = Convert.ToInt32(name.Last());

                int index = borderEnd.Children.IndexOf(img);
                int gridCol = index / columns;
                int gridRow = index % columns;

                if (gridCol != col && gridRow != row) win = false;
            }

            if(win)
                MessageBox.Show("Виграш");
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img1 = sender as System.Windows.Controls.Image;
            global_sender = img1;
            DragDrop.DoDragDrop(img1, img1.Source, DragDropEffects.Copy);
        }
    }
}
