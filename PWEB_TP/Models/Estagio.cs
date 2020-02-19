using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PWEB_TP.Controllers;

namespace PWEB_TP.Models
{
    public enum Estado { Pendente = 0, Aprovado = 1, Recusado = 2, Entrevista = 3, Atribuido  = 4, NãoAtribuido = 5, Defesa = 6, Concluido = 7};
    public class Estagio
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int EstagioId { get; set; }

        [Required]
        [Display(Name = "Nome do Estágio")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Ramo")]
        public Ramo Ramo { get; set; }

        [Required]
        [Display(Name = "Enquadramento")]
        [DataType(DataType.MultilineText)]
        public string Enquandramento { get; set; }

        [Required]
        [Display(Name = "Objetivos")]
        [DataType(DataType.MultilineText)]
        public string Objetivos { get; set; }

        [Required]
        [Display(Name = "Condições de Acesso")]
        public string CondicoesDeAcesso { get; set; }

        [Required]
        [Display(Name = "Localização")]
        public string Local { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataDeCriacao { get; set; }

        [Required]
        [Display(Name = "Contacto")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Número de telefone inválido.")]
        public int Contacto { get; set; }

        [Display(Name = "Estado")]
        public Estado Estado { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string Observacoes { get; set; }

        [Display(Name = "Avaliação da Empresa/Docente(s)")]
        public int AvaliacaoED { get; set; }

        [Display(Name = "Aluno Atribuido")]
        public string AlunoId { get; set; }
        [ForeignKey("AlunoId")]
        public virtual ApplicationUser Aluno { get; set; }

        [Display(Name = "Avaliação do Aluno")]
        public int AvaliacaoA { get; set; }

        [Display(Name = "Data da Defesa")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> DataDaDefesa { get; set; }

        [Display(Name = "Utilizador")]
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}