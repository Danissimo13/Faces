using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
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

            Bitmap changedImage = ReplFaceImg.Repl(firstBitmap, secondBitmap);

            return changedImage.ToByteArray();
        }
    }
}
