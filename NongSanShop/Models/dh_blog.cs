namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_blog
    {
        public int id { get; set; }

        [Required(ErrorMessage ="Không được để trống")]
        [StringLength(100)]
        public string thumbnail { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public string content { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }
    }
}
