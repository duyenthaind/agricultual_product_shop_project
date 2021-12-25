namespace NongSanShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dh_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dh_category()
        {
            dh_product = new HashSet<dh_product>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [StringLength(255)]
        public string name { get; set; }

        public string description { get; set; }

        public long? created { get; set; }

        public long? updated { get; set; }

        public string avatar { set; get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dh_product> dh_product { get; set; }
    }
}
