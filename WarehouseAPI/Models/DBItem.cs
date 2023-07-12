using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAPI.Models
{
    [Table("Items")]
    public class DbItem
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Column("ID_Document")]
        public int DocID { get; set; }

        [Column("ID_Article")]
        public int ArticleID { get; set; }

        public int Amount { get; set; }
    }
}
