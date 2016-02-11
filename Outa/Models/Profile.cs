namespace Outa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Profile")]
    public partial class Profile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }

        [StringLength(128)]
        public string UserID { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }
        public string Img { get; set; }

        public string Description { get; set; }

        public decimal? Rating { get; set; }
    }
}
