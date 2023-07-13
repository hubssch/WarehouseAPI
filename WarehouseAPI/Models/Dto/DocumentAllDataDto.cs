using System.Security.Permissions;

namespace WarehouseAPI.Models.Dto
{
    public class DocumentAllDataDto
    {
        public int DocID { get; set; }
        public string Signature { get; set; }
        public string DocType { get; set; }
        public ContractorDto Contract { get; set; }
        public DateTime Date { get; set; }
        public List<ArticleDto> Articles { get; set; }

        public DocumentAllDataDto(int docID, string signature, string docType, ContractorDto contract, DateTime date, List<ArticleDto> articles)
        {
            DocID = docID;
            Signature = signature;
            DocType = docType;
            Contract = contract;
            Date = date;
            Articles = articles;
        }

        public DocumentAllDataDto() : this(0, "", "", new ContractorDto(), DateTime.Now, new List<ArticleDto>() { }) { }
    }
}
