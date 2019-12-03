using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Starter.Data.Entities;
using Starter.Data.Repositories;

namespace Starter.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatRepository _repository;

        public CatController(ICatRepository repository)
        {
            _repository = repository;
        }

        // GET /cat
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET /cat/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetById(id));
        }

        // GET /cat/5
        [HttpGet("GetBySecondaryId/{id}")]
        public async Task<IActionResult> GetBySecondaryId(Guid id)
        {
            return Ok(await _repository.GetBySecondaryId(id));
        }

        // POST /cat
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cat entity)
        {
            await _repository.Create(entity);

            return Ok();
        }

        // PUT /cat/{id}
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Cat entity)
        {
            await _repository.Update(entity);

            return Ok();
        }

        // DELETE /cat/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.Delete(id);

            return Ok();
        }
    }
}