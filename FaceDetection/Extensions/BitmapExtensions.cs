using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FaceDetection.Extensions
{
    internal static class BitmapExtensions
    {
        public static Bitmap ToBitmap(this byte[] image)
        {
            Bitmap bitmap;
            using (MemoryStream reader = new MemoryStream(image))
            {
                Image img = Image.FromStream(reader);
                bitmap = new Bitmap(img);
            }

            return bitmap;
        }

        public static byte[] ToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream reader = new MemoryStream())
            {
                Image img = Image.FromHbitmap(bitmap.GetHbitmap());
                img.Save(reader, ImageFormat.Jpeg);

                return reader.ToArray();
            }
        }
    }
}
