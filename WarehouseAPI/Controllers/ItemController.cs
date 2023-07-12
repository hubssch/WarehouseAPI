using WarehouseAPI.Data;
using WarehouseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Data;

/* to jest chyba do usuniecia */

namespace WarehouseAPI.Controllers
{
    [Route("api/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly WarehouseAppDbContext _db;

        public ItemController(WarehouseAppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public object Get()
        {
            try
            {
                IEnumerable<DbDocument> cars = _db.Documents.ToList();
                return cars;
            }
            catch(Exception ex)
            {

            }
            return null;

        }
    }
}
