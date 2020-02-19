using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWEB_TP.Models
{
    public enum Ramo { Todos = 0, DA = 1, SI = 2, RAS = 3 };

    public class Disciplina
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int DisciplinaId { get; set; }

        [Required]
        [Display(Name = "Ramo")]
        public Ramo Ramo { get; set; }

        [Required]
        [Display(Name = "Disciplina")]
        [StringLength(100)]
        public string Nome { get; set; }

    }
}