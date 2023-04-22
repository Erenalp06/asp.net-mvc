using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Entities
{
    [Table("Projects")]
    [Serializable]
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ProjectImage { get; set; }

        [Required]
        [StringLength(50)]
        public string Fullname { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [Required]        
        public string ProjectURL { get; set; }

    }
}
