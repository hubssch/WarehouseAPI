using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAPI.Models
{
    [Table("Contractors")]
    public class DbContract
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractID { get; set; }

        [Required]
        [MaxLength(128)]
        public string? Name { get; set; }

        public string? NIP { get; set; }

        [MaxLength(128)]
        public string? Street { get; set; }

        [MaxLength(128)]
        public string? Post { get; set; }

        public string? Code { get; set; }
    }
}
