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
        [StringLength(36)]
        public string Title { get; set; }

        public string Content { get; set; }

        [StringLength(128)]
        public string Author { get; set; }
        [StringLength(256)]
        public string AuthorUn { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string Tags { get; set; }
        public int Status { get; set; }
        public string Img { get; set; }
    }
}
