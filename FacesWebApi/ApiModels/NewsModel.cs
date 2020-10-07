using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FacesWebApi.ApiModels
{
    public class NewsModel
    {
        [Required(ErrorMessage = "You didn't enter a title.")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "You didn't enter a body.")]
        public string Body { get; set; }

        [Required(ErrorMessage = "You didn't choose a image.")]
        public IFormFile Image { get; set; }
    }
}
