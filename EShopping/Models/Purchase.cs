using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopping.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        //[Required]
        [Display(Name = "機關名稱")]
        [StringLength(50)]
        public string name { get; set; }

        //[Required]
        [Display(Name = "標案案號及名稱")]
        [StringLength(50)]
        public string project { get; set; }

        //[Required]
        [Display(Name = "傳輸次數")]
        [StringLength(50)]
        public string counter { get; set; }

        //[Required]
        [Display(Name = "招標方式")]
        [StringLength(50)]
        public string method { get; set; }

        //[Required]
        [Display(Name = "採購性質")]
        [StringLength(50)]
        public string category { get; set; }

        //[Required]
        [Display(Name = "公告日期")]
        public DateTime announce_date { get; set; }

        //[Required]
        [Display(Name = "截止投標")]
        public DateTime submit_deadline { get; set; }

        //[Required]
        [Display(Name = "預算金額")]
        public int? budget { get; set; }
    }
}