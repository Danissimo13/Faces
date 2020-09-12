using System;
using System.IO;
using DlibDotNet;
using FaceDetection.Services;

namespace FaceDetection
{
    class Program
    {
        public static void Main(string[] args) // Example use model
        {
            try
            {
                ModelPathSystem pathSystem = new ModelPathSystem("../../../Assets/Input", "../../../Assets/Output"); // create path system with input and output pathes
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("===Press any key===");
            Console.ReadKey();
        }
    }
}
