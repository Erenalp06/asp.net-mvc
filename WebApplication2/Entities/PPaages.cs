using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Entities
{
    [Table("PPagess")]
    [Serializable]
    public class PPaages
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Fullname { get; set; }


        [Required]
        [StringLength(150)]
        public string CVText { get; set; }

        [Required]
        public string CVPath { get; set; }

        [Required]
        [StringLength(150)]
        public string PortfolioText { get; set; }

        [Required]
        public string PortfolioPath { get; set; }

        [Required]
        [StringLength(150)]
        public string Contact { get; set; }

        [Required]
        [StringLength(150)]
        public string AboutMe { get; set; }

        [Required]
        public string PageImage { get; set; }


        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [Required]
        public string PageURL { get; set; }

    }
}
