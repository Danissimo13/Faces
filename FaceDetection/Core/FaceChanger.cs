using System.Drawing;
using FaceDetection.Extensions;
using AI.Dlib;
using System.Drawing.Imaging;

namespace FaceDetection.Core
{
    public static class FaceReplacer
    {
        public static byte[] ReplaceFaces(byte[] fromImage, byte[] toImage)
        {
            Bitmap fromBitmap = fromImage.ToBitmap();
            Bitmap toBitmap = toImage.ToBitmap();

            Bitmap changedImage = ReplaceFaces(fromBitmap, toBitmap);
          
            return changedImage.ToByteArray();
        }

        public static Bitmap ReplaceFaces(string fromImagePath, string toImagePath)
        {
            Image fromImage = Image.FromFile(fromImagePath);
            Bitmap fromBitmap = new Bitmap(fromImage);

            Image toImage = Image.FromFile(toImagePath);
            Bitmap toBitmap = new Bitmap(toImage);

            Bitmap changedImage = ReplaceFaces(fromBitmap, toBitmap);

            return changedImage;
        }

        public static Bitmap ReplaceFaces(Bitmap from, Bitmap to)
        {
            FaceData haar = new FaceData();
            
            Rectangle fromRectangle = haar.AccurateFaceDetection(from);
            Rectangle toRectangle = haar.AccurateFaceDetection(to);

            if (fromRectangle.X < 0) fromRectangle.X = 0;
            if (fromRectangle.Y < 0) fromRectangle.Y = 0;
            if ((fromRectangle.X + fromRectangle.Width) > from.Width) fromRectangle.Width = from.Width - fromRectangle.X;
            if ((fromRectangle.Y + fromRectangle.Height) > from.Height) fromRectangle.Height = from.Height - fromRectangle.Y;

            Bitmap face = from.Clone(fromRectangle, PixelFormat.Format32bppArgb);
            face = new Bitmap(face, toRectangle.Width, toRectangle.Height);

            Bitmap mask = new Bitmap(to.Width, to.Height);
            Bitmap dst = new Bitmap(to.Width, to.Height);
           
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.Clear(Color.Black);
                g.DrawImage(face, toRectangle);
            }

            using (Graphics g1 = Graphics.FromImage(mask))
            {
                g1.Clear(Color.Black);
                g1.FillRectangle(Brushes.White, toRectangle);
            }

            Bitmap swapedFaces = ImgProccesing.GetBitmap(to, dst, mask);

            return swapedFaces;
        }
    }
}