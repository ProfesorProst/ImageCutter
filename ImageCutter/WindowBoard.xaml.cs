using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageCutter
{
    /// <summary>
    /// Interaction logic for WindowBoard.xaml
    /// </summary>
    public partial class WindowBoard : Window
    {

        public static Image global_sender;
        public WindowBoard(List<Image> bitmaps, int row, int col)
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

        private void SetImages(List<Image> bitmaps, int row, int col)
        {
            bitmaps = ImageModelStatic.Shuffle(bitmaps);

            int x = 0, y = 0, bitmapsIter = 0;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Convert.ToInt32(bitmaps.First().Source.Width), Convert.ToInt32(bitmaps.First().Source.Height));
            using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bmp))
            {
                System.Drawing.Rectangle ImageSize = new System.Drawing.Rectangle(0, 0, Convert.ToInt32(bitmaps.First().Source.Width),
                    Convert.ToInt32(bitmaps.First().Source.Height));
                graph.FillRectangle(System.Drawing.Brushes.White, ImageSize);
            }
            for (int c = 0; c < col; c++)
                for (int r = 0; r < row; r++)
                {
                    var imageControl = bitmaps[bitmapsIter];
                    imageControl.SetValue(TagProperty, bitmaps[bitmapsIter].Tag.ToString());
                    imageControl.SetValue(Grid.RowProperty, x);
                    imageControl.SetValue(Grid.ColumnProperty, y);
                    imageControl.MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseDown);
                    imageControl.MouseRightButtonDown += new MouseButtonEventHandler(Image_RotateDown);
                    borderStart.Children.Add(imageControl);

                    var imageControl1 = new Image();
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

        private void Image_RotateDown(object sender, MouseButtonEventArgs e)
        {
            ImageModelStatic.RotateImage(sender as Image);
        }
        private void Image_Drop(object sender, DragEventArgs e)
        {
            ((Image)sender).Source = global_sender.Source;
            ((Image)sender).Tag = global_sender.Tag;
            ((Image)sender).RenderTransform = global_sender.RenderTransform;

            CheckWin();
        }

        private void CheckWin()
        {
            int win = 0;
            int columns = borderEnd.Columns;

            foreach (Image img in borderEnd.Children)
            {
                var name = ((img as Image).Tag as string).Split('_');
                if (name.First() == "") return;
                int col = Convert.ToInt32(name.First());
                int row = Convert.ToInt32(name[name.Length-2]);
                int angle = Convert.ToInt32(name.Last());

                int index = borderEnd.Children.IndexOf(img);
                int gridCol = index / columns;
                int gridRow = index % columns;

                if (gridCol == col && gridRow == row && angle == 0) win++;
            }

            if(win == borderEnd.Children.Count)
                MessageBox.Show("Виграш");
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img1 = sender as Image;
            global_sender = img1;
            DragDrop.DoDragDrop(img1, img1.Source, DragDropEffects.Copy);
        }
    }
}
