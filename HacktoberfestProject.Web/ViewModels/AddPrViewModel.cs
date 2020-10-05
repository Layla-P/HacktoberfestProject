using System.ComponentModel.DataAnnotations;

namespace HacktoberfestProject.Web.ViewModels
{
    public class AddPrViewModel
    {
        public string UserName { get; set; }

        [Display(Name = "Repository Owner")]
        [Required]
        public string Owner { get; set; }

        [Display(Name = "Repository Name")]
        [Required]
        public string Repository { get; set; }

        [Display(Name = "Pull Request Number")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a non-zero value!")]
        public int PrNumber { get; set; }

        [Display(Name = "Pull Request URL")]
        [Required]
        [Url]
        public string PrUrl { get; set; }
    }
}
