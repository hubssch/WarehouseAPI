using WarehouseAPI.Data;
using WarehouseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Models.Dto;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
		[HttpGet("all")]
		public ActionResult<IEnumerable<DocumentAllDataDto>> GetDocumentsAllData()
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
					ContractData = contract,
					Date = doc.Date
				};

            var docs_arts =
                from d in _db.Documents
                join i in _db.Items on d.DocID equals i.DocID
                join a in _db.Articles on i.ArticleID equals a.ArticleID
				select new
                {
                    DocID  = d.DocID,
                    ArtId  = a.ArticleID,
                    Amount = i.Amount,           // amount from the doc not warehouse inventory
                    ArtName = a.Name,
                    ArtDesc = a.Description
                };

            List<DocumentAllDataDto> docsDto = new List<DocumentAllDataDto>();
            foreach (var d in docs_list)
            {
                var curr_doc_arts = docs_arts.Where(curr_doc => curr_doc.DocID == d.DocID).ToList();
                var arts = curr_doc_arts.Select(cda => new ArticleDto(cda.ArtId, cda.ArtName, cda.ArtDesc, cda.Amount));
                var outDoc = new DocumentAllDataDto(
                    d.DocID,
                    d.Signature,
                    d.DocType,
                    new ContractorDto(d.ContractData),
                    d.Date,
                    arts.ToList()
                );
                docsDto.Add(outDoc);
            }

			return docsDto;
		}

		[Authorize(Roles = "Admin,User")]
		[HttpGet("{id}")]
        public ActionResult<DocumentDto> GetDocument(int id)
        {
            var doc_list = from docs in _db.Documents
                           join contract in _db.Contracts
                           on docs.ContractID equals contract.ContractID
                           where docs.DocID == id
                           select new
                           {
                               DocID = docs.DocID,
                               Signature = docs.Signature,
                               DocType = docs.Operation == 'W' ? "WZ" : "PZ",
                               ContractData = String.Format("{0}, {1}, {2} {3}, NIP: {4}", contract.Name, contract.Street, contract.Code, contract.Post, contract.NIP),
                               Date = docs.Date
                           };

            if (!doc_list.Any()) return NotFound();

            var doc = doc_list.First();

            DocumentDto docDto = new DocumentDto(doc.DocID, doc.Signature, doc.Signature, doc.ContractData, doc.Date);

            return docDto;
        }

		[Authorize(Roles = "Admin,User")]
		[HttpGet("all/{id}")]
		public ActionResult<DocumentAllDataDto> GetDocumentAllData(int id)
		{
			var single_doc_list =
				from doc in _db.Documents
				join contract in _db.Contracts
				on doc.ContractID equals contract.ContractID
                where doc.DocID == id
				select new
				{
					DocID = doc.DocID,
					Signature = doc.Signature,
					DocType = doc.Operation == 'W' ? "WZ" : "PZ",
					ContractData = contract,
					Date = doc.Date
				};

            if (!single_doc_list.Any()) return NotFound();

            var foundoc = single_doc_list.First();

			var docs_arts =
				from d in _db.Documents
				join i in _db.Items on d.DocID equals i.DocID
				join a in _db.Articles on i.ArticleID equals a.ArticleID
				select new
				{
					DocID = d.DocID,
					ArtId = a.ArticleID,
					Amount = i.Amount,           // amount from the doc not warehouse inventory
					ArtName = a.Name,
					ArtDesc = a.Description
				};

			List<DocumentAllDataDto> docsDto = new List<DocumentAllDataDto>();

    		var curr_doc_arts = docs_arts.Where(curr_doc => curr_doc.DocID == foundoc.DocID).ToList();
			var arts = curr_doc_arts.Select(cda => new ArticleDto(cda.ArtId, cda.ArtName, cda.ArtDesc, cda.Amount));
			var outDoc = new DocumentAllDataDto(
				foundoc.DocID,
				foundoc.Signature,
				foundoc.DocType,
				new ContractorDto(foundoc.ContractData),
				foundoc.Date,
				arts.ToList()
			);

			return outDoc;
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

			_db.Documents.Add(doc);
            _db.SaveChanges();

            dto.DocID = doc.DocID;

            return CreatedAtAction(nameof(CreateDocument), dto);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost("all")]
		public ActionResult<DocumentAllDataDto> CreateDocument(DocumentAllDataDto dto)
		{
			var doc = new DbDocument
			{
				Signature = dto.Signature,
				Operation = dto.DocType == "W" ? 'W' : 'P',
				Date = dto.Date,
				ContractID = dto.Contract.ContractID
			};

			var transaction = _db.Database.BeginTransaction();

			_db.Documents.Add(doc);
			_db.SaveChanges();

			int multiplier = dto.DocType == "W" ? -1 : 1; // dla WZ odejmujemy, dla PZ dodajemy

			dto.DocID = doc.DocID;

			foreach (ArticleDto item in dto.Articles)
			{
				DbItem dbitem = new DbItem { DocID = doc.DocID, ArticleID = item.ArticleID, Amount = item.Amount };
				_db.Add(dbitem);
				DbArticle dbarticle = _db.Articles.FirstOrDefault(a => a.ArticleID == item.ArticleID);
				dbarticle.Amount += multiplier * item.Amount;
				_db.Update(dbarticle);
				_db.SaveChanges();
			}

			transaction.Commit();

			return CreatedAtAction(nameof(CreateDocument), dto);
		}

		[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateDocument(int id, DocumentDataDto dto)
        {
            if (id != dto.DocID) return BadRequest();

            var doc = _db.Documents.FirstOrDefault(d => d.DocID == id);

            if (doc == null) return NotFound();

            doc.ContractID = dto.ContractID;
            doc.Date = dto.Date;
            doc.Signature = dto.Signature;
            doc.Operation = dto.DocType == "WZ" ? 'W' : 'P';

            _db.SaveChanges();

            return NoContent();
        }

		[Authorize(Roles = "Admin")]
		[HttpPut("all/{id}")]
		public IActionResult UpdateDocument(int id, DocumentAllDataDto dto)
		{
			if (id != dto.DocID) return BadRequest();

			var doc = _db.Documents.FirstOrDefault(d => d.DocID == id);

			if (doc == null) return NotFound();

			var transaction = _db.Database.BeginTransaction();

			doc.Signature = dto.Signature;
			doc.Date = dto.Date;
			doc.ContractID = dto.Contract.ContractID;
			doc.Operation = dto.DocType == "WZ" ? 'W' : 'P';

			_db.Update(doc);
			_db.SaveChanges();

			_db.Database.ExecuteSql($"DELETE FROM Items where ID_Document = {id}");

			foreach (ArticleDto item in dto.Articles)
			{
				DbItem dbitem = new DbItem { DocID = doc.DocID, ArticleID = item.ArticleID, Amount = item.Amount };
				_db.Add(dbitem);
				_db.SaveChanges();
			}

			transaction.Commit();

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
