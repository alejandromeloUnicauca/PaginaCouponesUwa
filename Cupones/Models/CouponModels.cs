using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cupones.Models
{
    [Table("Coupon")]
    public class CouponModel
    {
        [Key]
        public int idCoupon { get; set; }
        [Display(Name="Name")]
        [Column("name")]

       
        public string name { get; set; }
        [Display(Name="Base Cost")]
        [Column("baseCost")]
        public long baseCost { get; set; }
        [Display(Name = "Sale Cost")]
        [Column("saleCost")]
        public long saleCost { get; set; }
        [Display(Name = "Original Cost")]
        [Column("originalCost")]
        public long originalCost { get; set; }
        [Display(Name = "Description")]
        [Column("description")]
        [StringLength(300)]
        public string description { get; set; }
        [Display(Name = "Stock")]
        [Column("stock")]
        public int stock { get; set; }
        [Display(Name = "Enable")]
        [Column("enable")]
        public bool enable { get; set; }

        [Display(Name = "Expiration Date")]
        [Column("expirationDate")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime expirationDate { get; set; }

        [Display(Name = "Path Imagen")]
        [Column("pathImg")]
        public string pathImg { get; set; }

        // Foreign key 
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser AspNetUser{ get; set; }

        public int idEmpresa { get; set; }

        [ForeignKey("idEmpresa")]
        public virtual EmpresaModel Empresa { get; set; }
    }

}