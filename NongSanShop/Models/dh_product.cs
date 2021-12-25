namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dh_product()
        {
            dh_cart = new HashSet<dh_cart>();
            dh_order_product = new HashSet<dh_order_product>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [StringLength(100)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column(TypeName = "numeric")]
        public long? price { get; set; }

        public int? quantity { get; set; }

        public int? category_id { get; set; }

        public string avatar { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dh_cart> dh_cart { get; set; }

        public virtual dh_category dh_category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dh_order_product> dh_order_product { get; set; }
    }
}
