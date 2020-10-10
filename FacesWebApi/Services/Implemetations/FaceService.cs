using DlibDotNet;
using FaceDetection.Core;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FacesWebApi.Services.Implemetations
{
    public class FaceService : IFaceService
    {
        private readonly IStorage storage;
        private readonly IFileService fileService;
        private readonly ModelPathSystem facePathSystem;

        public FaceService(IStorage storage, IFileService fileService, ModelPathSystem facePathSystem)
        {
            this.storage = storage;
            this.fileService = fileService;
            this.facePathSystem = facePathSystem;
        }

        public async Task<Request> CreateRequestAsync(string requestType, IFormFile fromImageFile, IFormFile toImageFile = null)
        {
            var requestRepository = storage.GetRepository<IRequestRepository>();
            var requestImageRepository = storage.GetRepository<IRequestImageRepository>();

            Request request;
            switch (requestType)
            {
                case nameof(SwapRequest):
                    {
                        if (toImageFile == null)
                        { 
                            throw new NullReferenceException("If RequestType is Swap, then ToImage cant be a null.");
                        }

                        request = new SwapRequest() { Discriminator = nameof(SwapRequest) };
                        await requestRepository.CreateAsync(request);

                        RequestImage fromImage = new RequestImage() { Request = request };
                        await requestImageRepository.CreateAsync(fromImage);

                        RequestImage toImage = new RequestImage() { Request = request };
                        await requestImageRepository.CreateAsync(toImage);

                        await storage.SaveAsync();

                        fromImage.ImageName = fromImage.ImageId + Path.GetExtension(fromImageFile.FileName);
                        requestImageRepository.Edit(fromImage);

                        toImage.ImageName = toImage.ImageId + Path.GetExtension(toImageFile.FileName);
                        requestImageRepository.Edit(toImage);

                        await fileService.SaveFileAsync(fromImageFile.OpenReadStream(), Path.Combine(fileService.GlobalRequestImagesPath, fromImage.ImageName));
                        await fileService.SaveFileAsync(toImageFile.OpenReadStream(), Path.Combine(fileService.GlobalRequestImagesPath, toImage.ImageName));

                        break;
                    }

                case nameof(CutRequest):
                    {
                        request = new CutRequest() { Discriminator = nameof(CutRequest) };
                        await requestRepository.CreateAsync(request);

                        RequestImage fromImage = new RequestImage() { Request = request };
                        await requestImageRepository.CreateAsync(fromImage);
                        await storage.SaveAsync();

                        fromImage.ImageName = fromImage.ImageId + Path.GetExtension(fromImageFile.FileName);
                        requestImageRepository.Edit(fromImage);
                        await fileService.SaveFileAsync(fromImageFile.OpenReadStream(), Path.Combine(fileService.GlobalRequestImagesPath, fromImage.ImageName));

                        break;
                    }

                case nameof(DetectRequest):
                    {
                        request = new DetectRequest() { Discriminator = nameof(DetectRequest) };
                        await requestRepository.CreateAsync(request);

                        RequestImage fromImage = new RequestImage() { Request = request };
                        await requestImageRepository.CreateAsync(fromImage);
                        await storage.SaveAsync();

                        fromImage.ImageName = fromImage.ImageId + Path.GetExtension(fromImageFile.FileName);
                        requestImageRepository.Edit(fromImage);
                        await fileService.SaveFileAsync(fromImageFile.OpenReadStream(), Path.Combine(fileService.GlobalRequestImagesPath, fromImage.ImageName));

                        break;
                    }

                default:
                    {
                        throw new KeyNotFoundException($"RequestType is {requestType} not exist.");
                    }
            };

            await storage.SaveAsync();

            return request;
        }

        public async Task<Response> CreateResponseAsync(Request request)
        {
            var requestRepository = storage.GetRepository<IRequestRepository>();
            var responseRepository = storage.GetRepository<IResponseRepository>();
            var responseImageRepository = storage.GetRepository<IResponseImageRepository>();

            Response response;
            switch (request.Discriminator)
            {
                case nameof(SwapRequest):
                    {
                        response = new SwapResponse() { Discriminator = nameof(SwapResponse) };
                        await responseRepository.CreateAsync(response);

                        string fromImagePath = Path.Combine(fileService.GlobalRequestImagesPath, request.Images.First().ImageName);
                        string toImagePath = Path.Combine(fileService.GlobalRequestImagesPath, request.Images.Skip(1).First().ImageName);
                        Bitmap swapFaces = FaceReplacer.ReplaceFaces(fromImagePath, toImagePath);

                        ResponseImage responseImage = new ResponseImage() { Response = response };
                        await responseImageRepository.CreateAsync(responseImage);
                        await storage.SaveAsync();

                        responseImage.ImageName = $"{responseImage.ImageId}.jpg";
                        responseImageRepository.Edit(responseImage);

                        fileService.SaveFile(swapFaces, Path.Combine(fileService.GlobalResponseImagesPath, responseImage.ImageName));

                        break;
                    }

                case nameof(CutRequest):
                    {
                        response = new CutResponse() { Discriminator = nameof(CutResponse) };
                        await responseRepository.CreateAsync(response);

                        FaceRecognizer faceRecognizer = new FaceRecognizer(request.Images.First().ImageName, new RgbPixel(0, 0, 255), facePathSystem);
                        foreach (byte[] faceBuffer in faceRecognizer.GetAllFacesImages())
                        {
                            ResponseImage responseImage = new ResponseImage() { Response = response };
                            await responseImageRepository.CreateAsync(responseImage);
                            await storage.SaveAsync();

                            responseImage.ImageName = $"{responseImage.ImageId}.jpg";
                            responseImageRepository.Edit(responseImage);

                            await fileService.SaveFileAsync(faceBuffer, Path.Combine(fileService.GlobalResponseImagesPath, responseImage.ImageName));
                        }

                        break;
                    }

                case nameof(DetectRequest):
                    {
                        response = new DetectResponse() { Discriminator = nameof(DetectResponse) };
                        await responseRepository.CreateAsync(response);

                        FaceRecognizer faceRecognizer = new FaceRecognizer(request.Images.First().ImageName, new RgbPixel(0, 0, 255), facePathSystem);
                        byte[] faceBuffer = faceRecognizer.GetOutlinedFacesImage();

                        ResponseImage responseImage = new ResponseImage() { Response = response };
                        await responseImageRepository.CreateAsync(responseImage);
                        await storage.SaveAsync();

                        responseImage.ImageName = $"{responseImage.ImageId}.jpg";
                        responseImageRepository.Edit(responseImage);

                        await fileService.SaveFileAsync(faceBuffer, Path.Combine(fileService.GlobalResponseImagesPath, responseImage.ImageName));

                        break;
                    }

                default:
                    {
                        throw new KeyNotFoundException($"RequestType is {request.Discriminator} not exist.");
                    }
            };

            request.Response = response;
            requestRepository.Edit(request);

            await storage.SaveAsync();

            return response;
        }
    }
}
