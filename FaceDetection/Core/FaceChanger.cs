using System.Drawing;
using AI.ComputerVision;
using AI.ComputerVision.DeepFake;
using FaceDetection.Extensions;

namespace FaceDetection.Core
{
    public static class FaceReplacer
    {
        public static byte[] ReplaceFaces(byte[] firstImage, byte[] secondImage)
        {
            Bitmap firstBitmap = ImgConverter.BmpResizeM(firstImage.ToBitmap(), 1080);
            Bitmap secondBitmap = ImgConverter.BmpResizeM(secondImage.ToBitmap(), 1080);

            Bitmap changedImage = ReplFaceImg.Repl(secondBitmap, firstBitmap);

            return changedImage.ToByteArray();
        }
    }
}
