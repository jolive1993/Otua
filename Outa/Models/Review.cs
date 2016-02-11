namespace Outa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }

        public string Content { get; set; }

        public decimal? Rating { get; set; }

        public int? TrnsactionId { get; set; }
    }
}
