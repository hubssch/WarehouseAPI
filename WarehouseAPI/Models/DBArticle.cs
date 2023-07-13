using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAPI.Models
{
    [Table("Articles")]
    public class DbArticle
    {
        [Key]
        [Column("ID_Article")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleID { get; set; }

        [Required]
        [MaxLength(128)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int Amount { get; set; }
    }
}
