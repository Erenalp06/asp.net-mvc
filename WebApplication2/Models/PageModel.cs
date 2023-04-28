using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace WebApplication2.Models
{
    public class PageModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "CV text is required")]
        [StringLength(150, ErrorMessage = "CV text can be max 150 characters.")]
        public string CVText { get; set; }

        [Required(ErrorMessage = "CV file is required")]
        public IFormFile CVPath { get; set; }

        [Required(ErrorMessage = "You must add some skills")]
        public Dictionary<string, string> MyDictionary { get; set; } = new Dictionary<string, string>();

        [Required(ErrorMessage = "Portfolio text is required")]
        [StringLength(150, ErrorMessage = "Portfolio text can be max 150 characters.")]
        public string PortfolioText { get; set; }

        [Required(ErrorMessage = "PortfolioPath file text is required")]
        public string PortfolioPath { get; set; }

        [Required(ErrorMessage = "Contact Information is required")]
        [StringLength(150, ErrorMessage = "Contact Information can be max 150 characters.")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "You have to give information about yourself!")]
        [StringLength(150, ErrorMessage = "Information about yourself can be max 150 characters.")]
        public string AboutMe { get; set; }

        [Required(ErrorMessage = "Page Image is required")]
        [Display(Name = "Resim")]
        [DataType(DataType.Upload)]        
        public IFormFile PageImage { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description can be max 100 characters.")]
        public string Description { get; set; }

        [Required]
        public string PageURL { get; set; }



        


    }
}
