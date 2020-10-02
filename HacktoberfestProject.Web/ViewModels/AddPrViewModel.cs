using System.ComponentModel.DataAnnotations;

namespace HacktoberfestProject.Web.ViewModels
{
    public class AddPrViewModel
    {
        public string UserName { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public string Repository { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a non-zero value!")]
        public int PrNumber { get; set; }

        [Required]
        [Url]
        public string PrUrl { get; set; }
    }
}
