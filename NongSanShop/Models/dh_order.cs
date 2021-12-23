namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dh_order()
        {
            dh_order_product = new HashSet<dh_order_product>();
        }

        public int id { get; set; }

        public int? user_id { get; set; }

        [StringLength(255)]
        public string address { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(10)]
        public string code_name { get; set; }

        public int? status { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dh_order_product> dh_order_product { get; set; }

        public virtual dh_user dh_user { get; set; }
    }
}
