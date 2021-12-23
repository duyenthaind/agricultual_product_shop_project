namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_order_product
    {
        public int id { get; set; }

        public int? order_id { get; set; }

        public int? product_id { get; set; }

        public long? price { get; set; }

        public int? quantity { get; set; }

        public virtual dh_order dh_order { get; set; }

        public virtual dh_product dh_product { get; set; }
    }
}
