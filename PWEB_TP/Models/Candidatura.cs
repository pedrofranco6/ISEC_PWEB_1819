namespace PWEB_TP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    public class Candidatura
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int CandidaturaId { get; set; }

        [Display(Name = "Ramo")]
        public Ramo Ramo { get; set; }

        [Display(Name = "Disciplinas")]
        [StringLength(500)]
        public string Disciplinas { get; set; }

        [Display(Name = "Prioridade")]
        public string Importancia { get; set; }

        [Display(Name = "Orientador DEIS")]
        public string Orientador_FK { get; set; }
        public virtual ApplicationUser Orientador { get; set; }

        [Display(Name = "Estado")]
        public Estado Estado { get; set; }

        [Display(Name = "Proposta")]
        public int EstagioId { get; set; }
        [ForeignKey("EstagioId")]
        public virtual Estagio Estagio { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string Observacoes { get; set; }

        [Display(Name = "Utilizador")]
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
