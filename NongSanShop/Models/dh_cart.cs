namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_cart
    {
        public int id { get; set; }

        public int? user_id { get; set; }

        public int? quantity { get; set; }

        public long? price { get; set; }

        public int? product_id { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }

        public virtual dh_product dh_product { get; set; }

        public virtual dh_user dh_user { get; set; }
    }
}
