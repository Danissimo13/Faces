using AI;
using AI.ComputerVision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace FaceDetection.Core
{
    internal static class ImgProccesing
    {
        public static Bitmap BlendImages(Bitmap source, Bitmap insert, Rectangle r)
        {
            Bitmap bitmap = new Bitmap(source.Width, source.Height);
            Bitmap mask2 = new Bitmap(insert.Width, insert.Height);

            Vector centr = new double[] { r.Width / 2, r.Height / 2 }.ToVector();

            for (int i = 0; i < insert.Width; i++)
            {
                for (int j = 0; j < insert.Height; j++)
                {
                    Color color = insert.GetPixel(i, j);

                    int k = Coef(centr, i, j, r.Height, r.Width);
                    mask2.SetPixel(i, j, Color.FromArgb(k, color));
                }
            }

            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(source, 0, 0);
            g.DrawImage(mask2, r.X, r.Y);

            return bitmap;
        }

        private static int Coef(Vector centr, int i, int j, double h, double w)
        {
            double r = Math.Pow(Math.Pow((centr[0] - i) / (1.2 * w), 2) + Math.Pow((centr[1] - j) / h, 2), 3);
            r = Math.Exp(-190 * r);
            return (int)(255 * r);
        }

        private static Rectangle GetRectangleMask(Bitmap mask)
        {

            Matrix matr = ImgConverter.BmpToMatr(mask);

            int startX = 0, endX = 0, startY = 1, endY = mask.Height;

            for (int i = 1; i < mask.Width; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    if (matr[j, i - 1] < 0.1 && matr[j, i] > 0.1)
                        startX = i;
                    if (matr[j, i - 1] > 0.1 && matr[j, i] < 0.1)
                    {
                        endX = i;
                        goto r;
                    }
                    if (matr[j - 1, i] < 0.1 && matr[j, i] > 0.1)
                        startY = j;
                    if (matr[j - 1, i] > 0.1 && matr[j, i] < 0.1)
                        endY = j;
                }
            }

            r:

            return new Rectangle(startX, startY, endX - startX, endY - startY);
        }

        private static Bitmap NewColor(Bitmap source, Bitmap insert, Rectangle r)
        {

            List<Vector> srV = new List<Vector>();
            List<Vector> inV = new List<Vector>(); ;


            for (int i = r.X; i < r.X + r.Width - 2; i += 3)
            {
                for (int j = r.Y; j < r.Y + r.Height - 3; j += 4)
                {
                    Color color = source.GetPixel(i, j);
                    Color color2 = insert.GetPixel(i, j);
                    srV.Add(new double[] { color.R, color.G, color.B }.ToVector());
                    inV.Add(new double[] { color2.R, color2.G, color2.B }.ToVector());
                }
            }

            Vector meanSrc = Vector.Mean(srV.ToArray()) / 255;
            Vector meanInk = Vector.Mean(inV.ToArray()) / 255;

            Tensor tensor = ImgConverter.BmpToTensor(insert.Clone(r, PixelFormat.Format32bppArgb));


            tensor = tensor.DivD(meanInk);
            tensor = tensor.PlusD(meanSrc);

            tensor = tensor.TransformTensor(x =>
            {
                if (x < 0) x = 0;
                if (x > 1) x = 1;
                return x;
            });

            return ImgConverter.TensorToBitmap(tensor);
        }

        public static Bitmap GetBitmap(Bitmap source, Bitmap insert, Bitmap mask)
        {
            Rectangle r = GetRectangleMask(mask);

            Bitmap bitmap = NewColor(source, insert, r);

            bitmap = BlendImages(source, bitmap, r);

            return bitmap;
        }
    }
}
