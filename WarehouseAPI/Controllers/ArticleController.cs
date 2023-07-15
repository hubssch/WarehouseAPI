using WarehouseAPI.Data;
using WarehouseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Models.Dto;

namespace WarehouseAPI.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly WarehouseAppDbContext _db;

        public ArticleController(WarehouseAppDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public ActionResult<IEnumerable<ArticleDto>> GetArticles()
        {
            var a_list = _db.Articles.ToList();
            var aDtos = a_list.Select(a =>
                new ArticleDto(a)
            );
            return aDtos.ToList();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public ActionResult<ArticleDto> GetArticle(int id)
        {
            var a = _db.Articles.FirstOrDefault(a => a.ArticleID == id);

            if (a == null) return NotFound();

            return new ArticleDto(a);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ArticleDto> CreateArticle(ArticleDto dto)
        {
            var a = new DbArticle
            {
                Name = dto.Name,
                Description = dto.Description,
                Amount = dto.Amount
            };


            _db.Articles.Add(a);
            _db.SaveChanges();

            dto.ArticleID = a.ArticleID;

            return CreatedAtAction(nameof(CreateArticle), dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateArticle(int id, ArticleDto dto)
        {
            if (id != dto.ArticleID) return BadRequest();

            var a = _db.Articles.FirstOrDefault(a => a.ArticleID == id);

            if (a == null) return NotFound();

            a.Name = dto.Name;
            a.Description = dto.Description;
            a.Amount = dto.Amount;

            _db.SaveChanges();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult RemoveArticle(int id)
        {
            var a = _db.Articles.FirstOrDefault(a => a.ArticleID == id);

            if (a == null) return NotFound();

            _db.Articles.Remove(a);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
