using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PWEB_TP.Models
{
    public class Mensagem
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int MensagemID { get; set; }

        [Display(Name = "Titulo")]
        public String Titulo { get; set; }

        [Display(Name = "Conteúdo")]
        [DataType(DataType.MultilineText)]
        public String Conteudo { get; set; }

        [Display(Name = "Data de Envio")]
        public DateTime DataDeCriacao { get; set; }

        [Display(Name = "Enviado Por")]
        public string Enviado_Id { get; set; }
        [ForeignKey("Enviado_Id")]
        public virtual ApplicationUser Enviado { get; set; }

        [Display(Name = "Recebida Por")]
        public string Recebido_Id { get; set; }
        [ForeignKey("Recebido_Id")]
        public virtual ApplicationUser Recebido { get; set; }
    }
}