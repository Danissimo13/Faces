using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FacesWebApi.ApiModels
{
    public class RequestModel
    {
        [Required(ErrorMessage = "You didnt enter a request type.")]
        public string RequestType { get; set; }

        [Required(ErrorMessage = "You didnt load an image.")]
        public IFormFile FromImage { get; set; }

        public IFormFile ToImage { get; set; }
    }
}
