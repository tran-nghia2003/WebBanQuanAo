namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductImage")]
    public partial class ProductImage
    {
        [Key]
        [StringLength(250)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Img_1 { get; set; }

        [StringLength(250)]
        public string Img_2 { get; set; }
    }
}
