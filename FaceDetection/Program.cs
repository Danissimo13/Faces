using System;
using System.Drawing.Text;
using System.IO;
using DlibDotNet;
using FaceDetection.Core;

namespace FaceDetection
{
    class Program
    {
        private static ModelPathSystem pathSystem;

        public static void Main(string[] args) // Example use model
        {
            try
            {
                pathSystem = new ModelPathSystem("../../../Assets/Input", "../../../Assets/Output");

                TestDetect();
                TestChange();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("===Press any key===");
            Console.ReadKey();
        }

        private static void TestDetect()
        {
             // create path system with input and output pathes
            string inputFileName = "1.jpg";

            byte[] img;
            using (FileStream reader = new FileStream(Path.Combine(pathSystem.InputsPath, inputFileName), FileMode.Open))
            {
                img = new byte[reader.Length];
                reader.Read(img, 0, img.Length); // read image to bytes array
            }

            FaceRecognizer faceRecognizer = new FaceRecognizer(img, new RgbPixel(0, 255, 255), pathSystem); // create recognition model

            var imageWithAllFaces = faceRecognizer.SaveOutlinedFacesImage("FirstTest", "main.jpg"); // save image with faces to Output/FirstTest/main.jpg
            var imagePathes = faceRecognizer.SaveAllFacesImages("FirstTest"); // save all faces images to Output/FirstTest/ folder
            foreach (string imagePath in imagePathes)
                Console.WriteLine($"===Face saved in {imagePath}===");
            Console.WriteLine($"===Main image saved in {imageWithAllFaces}===");
        }

        private static void TestChange()
        {
            string fromImageName = "1.jpg";
            string toImageName = "2.jpg";
            string outputImageName = "deepFake.jpg";

            Console.WriteLine("===Reading from image file===");
            byte[] fromImage;
            using(FileStream reader = new FileStream(Path.Combine(pathSystem.InputsPath, fromImageName), FileMode.Open))
            {
                fromImage = new byte[reader.Length];
                reader.Read(fromImage, 0, fromImage.Length);
            }

            Console.WriteLine("===Reading to image file===");
            byte[] toImage;
            using (FileStream reader = new FileStream(Path.Combine(pathSystem.InputsPath, toImageName), FileMode.Open))
            {
                toImage = new byte[reader.Length];
                reader.Read(toImage, 0, toImage.Length);
            }

            Console.WriteLine("===Replacing faces===");
            byte[] changedImage = FaceReplacer.ReplaceFaces(fromImage, toImage);

            Console.WriteLine("===Saving replaced faces image===");
            using (FileStream reader = new FileStream(Path.Combine(pathSystem.OutputsPath, outputImageName), FileMode.OpenOrCreate))
            {
                reader.Write(changedImage);
            }
        }
    }
}
