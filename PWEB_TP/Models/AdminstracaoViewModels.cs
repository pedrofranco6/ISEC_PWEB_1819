using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWEB_TP.Models
{
    public class AdminstracaoViewModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Nome Completo")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

    }


    public class ComissaoDeEstagiosViewModel
    {
        public List<AdminstracaoViewModel> Comissao { get; set; }
        public List<AdminstracaoViewModel> NaoComissao { get; set; }
        public string escolhido { get; set; }
    }


    public class DisciplinasViewModel
    {
        public string Nome { get; set; }

      //  [Range(0, 20, ErrorMessage = "Seleciona um número entre 0 e 20.")]
        [Column]
        public String Classificacao { get; set; }
    }

    public class CreateCandidaturaViewModel
    {
        public Candidatura Candidatura { get; set; }
        public List<DisciplinasViewModel> Disciplinas { get; set; }
    }
}