using WarehouseAPI.Data;
using WarehouseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Models.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;

namespace WarehouseAPI.Controllers
{
    [Route("api/doc")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly WarehouseAppDbContext _db;

        public DocumentController(WarehouseAppDbContext db)
        {
            _db = db;
        }

		[Authorize(Roles = "Admin,User")]
		[HttpGet]
        public ActionResult<IEnumerable<DocumentDto>> GetDocuments()
        {
            var docs_list =
                from doc in _db.Documents
                join contract in _db.Contracts
                on doc.ContractID equals contract.ContractID
                select new
                {
                    DocID = doc.DocID,
                    Signature = doc.Signature,
                    DocType = doc.Operation == 'W' ? "WZ" : "PZ",
                    ContractData = String.Format("{0}, {1}, {2} {3}, NIP: {4}", contract.Name, contract.Street, contract.Code, contract.Post, contract.NIP),
                    Date = doc.Date
                };

            IEnumerable<DocumentDto> docsDto = docs_list.Select(d => new DocumentDto(d.DocID, d.Signature, d.DocType, d.ContractData, d.Date));
                
            return docsDto.ToList();
        }

		[Authorize(Roles = "Admin,User")]
		[HttpGet("{id}")]
        public ActionResult<DocumentDto> GetDocument(int id)
        {
            var doc_list = from docs in _db.Documents
                           join contract in _db.Contracts
                           on docs.ContractID equals contract.ContractID
                           select new
                           {
                               DocID = docs.DocID,
                               Signature = docs.Signature,
                               DocType = docs.Operation == 'W' ? "WZ" : "PZ",
                               ContractData = String.Format("{0}, {1}, {2} {3}, NIP: {4}", contract.Name, contract.Street, contract.Code, contract.Post, contract.NIP),
                               Date = docs.Date
                           };
            var doc = doc_list.FirstOrDefault(d => d.DocID == id);

            if (doc == null) return NotFound();

            DocumentDto docDto = new DocumentDto(doc.DocID, doc.Signature, doc.Signature, doc.ContractData, doc.Date);

            return docDto;
        }

        [Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult<DocumentDto> CreateDocument(DocumentDataDto dto)
        {
            var doc = new DbDocument
            {
                Signature = dto.Signature,
                Operation = dto.DocType == "WZ" ? 'W' : 'P',
                Date = dto.Date,
                ContractID = dto.ContractID
            };

            /* todo articles */

            _db.Documents.Add(doc);
            _db.SaveChanges();

            dto.DocID = doc.DocID;

            return CreatedAtAction(nameof(CreateDocument), dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateDocument(int id, DocumentDataDto dto)
        {
            if (id != dto.DocID) return BadRequest();

            var doc = _db.Documents.FirstOrDefault(d => d.DocID == id);

            if (doc == null) return NotFound();

            /* TODO */

            _db.SaveChanges();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult RemoveDocument(int id)
        {
            var doc = _db.Documents.FirstOrDefault(d => d.DocID == id);

            if (doc == null) return NotFound();

            _db.Documents.Remove(doc);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
