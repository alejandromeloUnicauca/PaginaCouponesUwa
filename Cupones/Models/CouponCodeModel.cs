using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cupones.Models
{
    [Table("CouponCode")]
    public class CouponCodeModel
    {
        [Key]
        public int idCouponCode { get; set; }
        [Column("code")]
        public string code{ get; set; }

        public int idCoupon { get; set; }
        [ForeignKey("idCoupon")]
        public virtual CouponModel coupon { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser AspNetUser { get; set; }
    }
}