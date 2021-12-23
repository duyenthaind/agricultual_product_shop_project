namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dh_user()
        {
            dh_cart = new HashSet<dh_cart>();
            dh_order = new HashSet<dh_order>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(12)]
        public string phone { get; set; }

        [StringLength(255)]
        public string address { get; set; }

        public int? role { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dh_cart> dh_cart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dh_order> dh_order { get; set; }
    }
}
