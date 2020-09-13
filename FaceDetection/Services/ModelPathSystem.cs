using DlibDotNet;
using System;
using System.IO;

namespace FaceDetection.Services
{
    public class ModelPathSystem
    {
        public string InputsPath { get; set; }
        public string OutputsPath { get; set; }

        public ModelPathSystem(string inputsPath, string outputsPath)
        {
            InputsPath = inputsPath;
            OutputsPath = outputsPath;
        }

        public string CreateInputFile(byte[] data)
        {
            string imageName = GenerateFileName(InputsPath);

            string filePath = Path.Combine(InputsPath, imageName);
            using (FileStream writer = new FileStream(filePath, FileMode.Create))
            {
                writer.Write(data, 0, data.Length);
            }

            return imageName;
        }

        public string CreateOutputFile(Array2D<RgbPixel> image)
        {
            string imageName = GenerateFileName(OutputsPath);
            string filePath = Path.Combine(OutputsPath, imageName);
            Dlib.SaveJpeg(image, filePath);

            return imageName;
        }

        public string GenerateFileName(string path)
        {
            string imageName = Path.GetRandomFileName();
            while(File.Exists(Path.Combine(path, imageName)))
                imageName = Path.GetRandomFileName();

            return imageName;
        }
    }
}
