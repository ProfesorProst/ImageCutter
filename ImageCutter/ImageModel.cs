using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ImageCutter
{
    class ImageModel
    {
        private string path;
        private Image image;

        private List<string> puzzleFileName = new List<string>();

        public ImageModel(string path)
        {
            this.path = path.Remove(path.Length - 4);
            this.image = new Bitmap(path, true);
        }

        public List<Image> PuzzleImage(int col, int row)
        {
            List<Image> bitmaps = new List<Image>();
            System.IO.Directory.CreateDirectory(path);

            Rectangle rectangle = new Rectangle(0,0,0,0);
            int x = 0, y = 0, w = 0, h = 0;
            int baseWidth = image.Width / col;
            int baseHeight = image.Height / row;
            string imageName = path.Split('\\').Last();

            for (int c = 0; c < col; c++)  
                for (int r = 0; r < row; r++)
                {
                    x = r * baseWidth;
                    y = c * baseHeight;
                    w = (r + 1) * baseWidth;
                    h = (c + 1) * baseHeight;
                    Image bitmap = CropImage(new Rectangle (x, y, w, h));
                    bitmap.Tag = Convert.ToString(c + "_" + r);
                    bitmap.Save(path + "//" + imageName + "_" + c + "_" + r + ".png");
                    bitmaps.Add(bitmap);
                }

            return bitmaps;
        }

        public Image CropImage(Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(image, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }



    }

    public static class ImageModelStatic
    {
        private static Random rng = new Random();

        public static BitmapImage ToWpfImage(this System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage ix = new BitmapImage();
            ix.BeginInit();
            ix.CacheOption = BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit();
            return ix;
        }

        public static List<T> Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list.ToList();
        }
    }
}
