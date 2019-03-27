using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cupones.Models
{
    [Table("Empresa")]
    public class EmpresaModel
    {
        public EmpresaModel()
        {

        }

        [Key]
        public int idEmpresa { get; set; }
        [Required]
        public string nit { get; set; }
        [Required]
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser AspNetUser { get; set; }
    }
}