using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRespository _categoryRepository;

        public CategoriesController(CategoryRespository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var c = _categoryRepository.SelectAll();
            return Ok(c);
        }

        [HttpPost]
        public IActionResult InsertCategory([FromBody] CategoriesModel cat)
        {
            if (cat == null)
            {
                //Console.WriteLine(city.CityID);
                return BadRequest();
            }
            bool isInserted = _categoryRepository.Insert(cat);

            if (isInserted)
            {
                return Ok(new { Message = "cat Inserted Succesfully" });
            }

            return StatusCode(500, "An Error Occured While Inserting cat");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoriesModel cat)
        {
            if (cat == null || id != cat.CategoryId)
                return BadRequest();

            var isUpdate = _categoryRepository.Update(cat);
            if (!isUpdate)
                return NotFound();
            return Ok(new { message = "cat Updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var isDel = _categoryRepository.Delete(id);
            if (!isDel)
                return NotFound();
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
