using WarehouseAPI.Data;
using WarehouseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using WarehouseAPI.Models.Dto;

namespace WarehouseAPI.Controllers
{
    [Route("api/contractor")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly WarehouseAppDbContext _db;

        public ContractorController(WarehouseAppDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public ActionResult<IEnumerable<ContractorDto>> GetContractors()
        {
            var cont_list = _db.Contracts.ToList();
            var contDtos = cont_list.Select(c =>
                new ContractorDto(c)
            );
            return contDtos.ToList();
        }

		[Authorize(Roles = "Admin,User")]
		[HttpGet("{id}")]
        public ActionResult<ContractorDto> GetContractor(int id)
        {
            var cont = _db.Contracts.FirstOrDefault(c => c.ContractID == id);

            if (cont == null) return NotFound();

            return new ContractorDto(cont);
        }

        [Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult<DocumentDto> CreateContractor(ContractorDto dto)
        {
            var cont = new DbContract
            {
                Code = dto.Code,
                Name = dto.Name,
                NIP = dto.NIP,
                Post = dto.Post,
                Street = dto.Street
            };


            _db.Contracts.Add(cont);
            _db.SaveChanges();

            dto.ContractID = cont.ContractID;

            return CreatedAtAction(nameof(CreateContractor), dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateContract(int id, ContractorDto dto)
        {
            if (id != dto.ContractID) return BadRequest();

            var cont = _db.Contracts.FirstOrDefault(c => c.ContractID == id);

            if (cont == null) return NotFound();

            cont.Street = dto.Street;
            cont.NIP = dto.NIP;
            cont.Post = dto.Post;
            cont.Name = dto.Name;
            cont.Code = dto.Code;

            _db.SaveChanges();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult RemoveContract(int id)
        {
            var item = _db.Contracts.FirstOrDefault(c => c.ContractID == id);

            if (item == null) return NotFound();

            _db.Contracts.Remove(item);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
