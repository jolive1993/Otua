namespace Outa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int Id { get; set; }
        [Display(Name = "Message")]
        public string Content { get; set; }
        [StringLength(32)]
        public string Subject { get; set; }
        [StringLength(128)]
        public string Author { get; set; }
        [Display(Name = "User")]
        [StringLength(256)]
        public string AuthorUn { get; set; }
        [StringLength(128)]
        public string Recp { get; set; }
        [StringLength(256)]
        public string RecpUn { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Request Id")]
        public int RequestID { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        public int? ParentID { get; set; }
    }
}
