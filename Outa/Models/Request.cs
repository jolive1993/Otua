namespace Outa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int Id { get; set; }

        public string Content { get; set; }

        [StringLength(128)]
        public string Author { get; set; }

        [Required]
        public string Tags { get; set; }
    }
}
