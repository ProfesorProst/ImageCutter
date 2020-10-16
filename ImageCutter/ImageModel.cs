using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageCutter
{
    class ImageModel
    {
        private string path;
        private Image image;

        public ImageModel(string path)
        {
            this.path = path.Remove(path.Length - 4);
            this.image = new Bitmap(path, true);
        }

        public List<System.Windows.Controls.Image> PuzzleImage(int col, int row)
        {
            var bitmaps = new List<System.Windows.Controls.Image>();
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
                    Image bitmap = CropImage(new Rectangle (x, y, baseWidth, baseHeight));
                    bitmap.Save(path + "//" + imageName + "_" + c + "_" + r + ".png");

                    var contrImage = new System.Windows.Controls.Image();
                    contrImage.Source = ImageModelStatic.ToWpfImage(bitmap);
                    contrImage.Tag = Convert.ToString(c + "_" + r);
                    ImageModelStatic.RotateImage(contrImage);
                    
                    bitmaps.Add(contrImage);
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

        public static int[] angles = new int[4] { 0, 90, 180, 270 };
        public static BitmapImage ToWpfImage(this System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage ix = new BitmapImage();
            ix.BeginInit();
            ix.CacheOption = BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit();
            return ix;
        }

        public static System.Drawing.Image ToDrawingImage(this System.Windows.Controls.Image img)
        {
            MemoryStream ms = new MemoryStream();
            System.Windows.Media.Imaging.BmpBitmapEncoder bbe = new BmpBitmapEncoder();
            bbe.Frames.Add(BitmapFrame.Create(new Uri(img.Source.ToString(), UriKind.RelativeOrAbsolute)));

            bbe.Save(ms); 
            System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms);
            return img2;
        }

        public static List<System.Windows.Controls.Image> Shuffle(List<System.Windows.Controls.Image> list)
        {
            int n = list.Count;
            while (n > 0)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
                //list[n].Tag = Convert.ToString(list[k].Tag.ToString() + "_" + anglesNew);
            }
            return list;
        }

        public static System.Windows.Controls.Image RotateImage(System.Windows.Controls.Image img)
        {
            var arrayOfParrams = img.Tag.ToString().Split('_');
            float angle = 0;
            var newTagOfImage = "";
            if (arrayOfParrams.Length > 2)
            {
                string angleS = arrayOfParrams.Last();
                var currentAngle = Convert.ToInt32(angleS);
                angle = ImageModelStatic.angles.SkipWhile(x => x != currentAngle).Skip(1).DefaultIfEmpty(ImageModelStatic.angles[0]).FirstOrDefault();
                newTagOfImage = img.Tag.ToString().Substring(0, img.Tag.ToString().Length - angleS.Length) + angle;
                img.RenderTransform = new RotateTransform(angle, img.ActualWidth / 2, img.ActualHeight / 2);
            }
            else
            {
                angle = angles[rng.Next(angles.Length - 1)];
                newTagOfImage = img.Tag.ToString() + "_" + angle;
                img.RenderTransform = new RotateTransform(angle, img.Source.Width / 2, img.Source.Height / 2);
            }

            img.Tag = Convert.ToString(newTagOfImage);
            return img;
        }
    }
}
