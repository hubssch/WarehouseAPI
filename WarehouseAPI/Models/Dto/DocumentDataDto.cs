using System.Security.Permissions;

namespace WarehouseAPI.Models.Dto
{
    public class DocumentDataDto
    {
        public int DocID { get; set; }
        public string Signature { get; set; }
        public string DocType { get; set; }
        public int ContractID { get; set; }
        public DateTime Date { get; set; }
        public List<int> Articles { get; set; }

        public DocumentDataDto(int docID, string signature, string docType, int contractId, DateTime date, List<int> articles)
        {
            DocID = docID;
            Signature = signature;
            DocType = docType;
            ContractID = contractId;
            Date = date;
            Articles = articles;
        }

        public DocumentDataDto() : this(0, "", "", 0, DateTime.Now, new List<int>() { }) { }
    }
}
