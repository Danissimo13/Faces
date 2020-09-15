using System;
using System.Drawing;
using System.Drawing.Imaging;
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
            string inputFileName = "from.jpg";

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
            string fromImageName = "from.jpg";
            string toImageName = "to.jpg";
            string outputImageName = "deepFake.jpg";


            Console.WriteLine("===Replacing faces===");
            Bitmap changedImage = FaceReplacer.ReplaceFaces(Path.Combine(pathSystem.InputsPath, fromImageName), Path.Combine(pathSystem.InputsPath, toImageName));
            Console.WriteLine("===Saving replaced faces image===");
            using (FileStream reader = new FileStream(Path.Combine(pathSystem.OutputsPath, outputImageName), FileMode.OpenOrCreate))
            {
                changedImage.Save(reader, ImageFormat.Jpeg);
            }
        }
    }
}
