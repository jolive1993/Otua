namespace Outa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Offer")]
    public partial class Offer
    {
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int Id { get; set; }
        [Display(Name = "Message & Notes")]
        public string o_Content { get; set; }
        [StringLength(128)]
        public string o_Author { get; set; }
        [Display(Name = "User")]
        [StringLength(256)]
        public string o_AuthorUn { get; set; }
        [Display(Name = "Date")]
        public DateTime o_Date { get; set; }
        [Display(Name = "Offer")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? o_Price { get; set; }
        [Display(Name = "Request Id")]
        public int o_Parent { get; set; }
        [Display(Name = "Status")]
        public int o_Status { get; set; }
        public int ReadStatus { get; set; }
    }
}
