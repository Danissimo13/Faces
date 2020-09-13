using DlibDotNet;
using FaceDetection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace FaceDetection.Services
{
    public class FaceRecognizer : IDisposable
    {
        private RgbPixel Color { get; set; } // Outline face pen color
        private Array2D<RgbPixel> Image { get; set; } // This image
        private ModelPathSystem PathSystem { get; set; } // Model path system

        public FaceRecognizer(byte[] image, RgbPixel penColor, ModelPathSystem pathSystem) // Get image from bytes array
        {
            PathSystem = pathSystem;
            Image = image.ToArray2D(PathSystem); 
            Color = penColor;
        }

        public FaceRecognizer(string imageName, RgbPixel penColor, ModelPathSystem pathSystem) // Get image from image file
        {
            PathSystem = pathSystem;
            Image = Dlib.LoadImage<RgbPixel>(Path.Combine(PathSystem.InputsPath, imageName));
            Color = penColor;
        }

        public byte[] GetOutlinedFacesImage()
        {
            using (var faceDetector = Dlib.GetFrontalFaceDetector())
            {
                var faces = faceDetector.Operator(Image); // Detect faces

                foreach (var face in faces) // Draw rectangle around every face
                {
                    Dlib.DrawRectangle(Image, face, Color); 
                }
            }

            return Image.ToByteArray(PathSystem); // Convert image to bytes array and return him
        }

        public string SaveOutlinedFacesImage(string folderName, string imageName)
        {
            string folderPath = Path.Combine(PathSystem.OutputsPath, folderName);
            if (File.Exists(folderPath) == true) File.Delete(folderPath); // Check for exist file with same name and delet him
            if (Directory.Exists(folderName) == false) Directory.CreateDirectory(folderPath); // Check for exist directory with same name and create her

            using (var faceDetector = Dlib.GetFrontalFaceDetector())
            {
                var faces = faceDetector.Operator(Image); // Detect all faces

                foreach (var face in faces) // Draw rectangle around every face
                {
                    Dlib.DrawRectangle(Image, face, Color);
                }
            }

            string imagePath = Path.Combine(folderPath, imageName); // Create image file path
            Dlib.SaveJpeg(Image, imagePath); // Save image to image path

            return imagePath; // Return image path
        }

        public IList<byte[]> GetAllFacesImages()
        {
            var facesImages = new List<byte[]>();

            using (var faceDetector = Dlib.GetFrontalFaceDetector())
            {
                var faces = faceDetector.Operator(Image); // Detect faces
                for (int i = 0; i < faces.Length; i++)
                {
                    DPoint[] dPoints = new DPoint[4] // Create box points
                    {
                        new DPoint(faces[i].TopLeft.X, faces[i].TopLeft.Y),
                        new DPoint(faces[i].TopRight.X, faces[i].TopRight.Y),
                        new DPoint(faces[i].BottomRight.X, faces[i].BottomRight.Y),
                        new DPoint(faces[i].BottomLeft.X, faces[i].BottomLeft.Y),
                    };

                    //Extract face box and covert him to bytes array
                    byte[] faceBuffer = Dlib.ExtractImage4Points(Image, dPoints, (int)faces[i].Width, (int)faces[i].Height).ToByteArray(PathSystem);
                    facesImages.Add(faceBuffer); // Add face bytes array to facesImages list
                }
            }

            return facesImages;
        }

        public IList<string> SaveAllFacesImages(string folderName)
        {
            List<string> imagesPathes = new List<string>();

            string folderPath = Path.Combine(PathSystem.OutputsPath, folderName);
            if (File.Exists(folderPath) == true) File.Delete(folderPath); // Check for exist file with same name and delet him
            if (Directory.Exists(folderPath) == false) Directory.CreateDirectory(folderPath); // Check for exist directory with same name and create her

            using (var faceDetector = Dlib.GetFrontalFaceDetector())
            {
                var faces = faceDetector.Operator(Image); // Detect all faces
                for (int i = 0; i < faces.Length; i++)
                {
                    string imageName = $"face[{i}].jpg";; // Create image name
                    string imagePath = Path.Combine(folderPath, imageName); // Create image path

                    DPoint[] dPoints = new DPoint[4] // Create box points
                    {
                        new DPoint(faces[i].TopLeft.X, faces[i].TopLeft.Y),
                        new DPoint(faces[i].TopRight.X, faces[i].TopRight.Y),
                        new DPoint(faces[i].BottomRight.X, faces[i].BottomRight.Y),
                        new DPoint(faces[i].BottomLeft.X, faces[i].BottomLeft.Y),
                    };

                    //Extract face box
                    var face = Dlib.ExtractImage4Points(Image, dPoints, (int)faces[i].Width, (int)faces[i].Height);

                    imagesPathes.Add(imagePath);
                    Dlib.SaveJpeg(face, imagePath); // Save face image to image path
                }
            }

            return imagesPathes; // Return faces images pathes list
        }

        public void Dispose()
        {
            Image.Dispose();
            PathSystem = null;
            GC.SuppressFinalize(this);
        }
    }
}
