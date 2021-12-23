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

        public string thumbnail { get; set; }

        public string content { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }
    }
}
