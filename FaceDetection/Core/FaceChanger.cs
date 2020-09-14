using System.Drawing;
using AI.ComputerVision.DeepFake;
using FaceDetection.Extensions;

namespace FaceDetection.Core
{
    public static class FaceReplacer
    {
        public static byte[] ReplaceFaces(byte[] firstImage, byte[] secondImage)
        {
            Bitmap firstBitmap = firstImage.ToBitmap();
            Bitmap secondBitmap = secondImage.ToBitmap();

            Bitmap changedImage = ReplFaceImg.Repl(secondBitmap, firstBitmap);

            return changedImage.ToByteArray();
        }
    }
}
