using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        }

        private void SetImages(List<System.Drawing.Image> bitmaps, int row, int col)
        {
            bitmaps = ImageModelStatic.Shuffle(bitmaps);

            int x = 0, y = 0, bitmapsIter = 0;
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    var imageControl = new Image();
                    imageControl.Source = ImageModelStatic.ToWpfImage(bitmaps[bitmapsIter]);
                    imageControl.SetValue(Grid.RowProperty, x);
                    imageControl.SetValue(Grid.ColumnProperty, y);

                    borderStart.Children.Add(imageControl);
                    bitmapsIter++;
                }
            }
        }

    }
}
