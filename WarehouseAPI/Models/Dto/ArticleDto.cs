namespace WarehouseAPI.Models.Dto
{
    public class ArticleDto
    {
        public int ArticleID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Amount { get; set; }

        public ArticleDto() : this(0, "", "", 0) { }

        public ArticleDto(int articleID, string name, string description, int amount)
        {
            ArticleID = articleID;
            Name = name;
            Description = description;
            Amount = amount;
        }

        public ArticleDto(DbArticle article)
        {
            ArticleID = article.ArticleID;
            Name = article.Name;
            Description = article.Description;
            Amount = article.Amount;
        }
    }
}
