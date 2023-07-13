using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAPI.Models
{
    [Table("Document")]
    public class DbDocument
    {
        [Key]
        [Column("ID_Document")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocID { get; set; }

        [Required]
        [MaxLength(32)]
        public string? Signature { get; set; }

        [Column("Operation")]
        public char Operation { get; set; }

        [Column("ID_Contractor")]
        public long ContractID { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }
    }
}
