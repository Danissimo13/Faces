using DlibDotNet;
using FaceDetection.Services;
using System.IO;

namespace FaceDetection.Extensions
{
    static class Array2DExtensions
    {
        public static Array2D<RgbPixel> ToArray2D(this byte[] data, ModelPathSystem pathSystem) // Convert Array2D to byte array
        {
            Array2D<RgbPixel> img;

            string imageName = pathSystem.CreateInputFile(data); // Create temp file and get file name
            img = Dlib.LoadImage<RgbPixel>(Path.Combine(pathSystem.InputsPath, imageName)); // Load image from temp file
            File.Delete(Path.Combine(pathSystem.InputsPath, imageName)); // Delete image temp file

            return img; // Return image 2D Array
        } 

        public static byte[] ToByteArray(this Array2D<RgbPixel> data, ModelPathSystem pathSystem) // Convert byte array to Array2D
        {
            byte[] img;

            string imageName = pathSystem.CreateOutputFile(data); // Create temp file and get file name
            string imagePath = Path.Combine(pathSystem.OutputsPath, imageName); // Create full path for image temp file

            Dlib.SaveJpeg(data, imagePath); // Save image data to temp file
            using(FileStream reader = new FileStream(imagePath, FileMode.OpenOrCreate)) // Read bytes from saved temp file to buffer
            {
                img = new byte[reader.Length];
                reader.Read(img, 0, img.Length);
            }

            File.Delete(Path.Combine(pathSystem.OutputsPath, imageName)); // Delete temp file

            return img; // Return image bytes
        }
    }
}
