using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoesController : ControllerBase
    {
        private readonly ShoesRepository _shoesRepository;

        public ShoesController(ShoesRepository shoesRepository)
        {
            _shoesRepository = shoesRepository;
        }

        [HttpGet]
        public IActionResult GetAllShoes()
        {
            var shoes = _shoesRepository.SelectAll();
            return Ok(shoes);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetShoeById(int id)
        //{
        //    var shoe = _shoesRepository.SelectByPK(id);
        //    if (shoe == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(shoe);
        //}

        [HttpDelete("{id}")]
        public IActionResult DeleteShoe(int id)
        {
            var isDel = _shoesRepository.Delete(id);
            if (!isDel)
                return NotFound();
            return Ok(new { message = "Shoe deleted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> InsertShoe([FromForm] AddShoeModel shoes)
        {
            if (shoes == null)
            {
                //Console.WriteLine(city.CityID);
                return BadRequest();
            }
            bool isInserted =await _shoesRepository.Insert(shoes);

            if (isInserted)
            {
                return Ok(new { Message = "Shoes Inserted Succesfully" });
            }

            return StatusCode(500, "An Error Occured While Inserting Shoes");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoes(int id, [FromForm] AddShoeModel shoes)
        {   
            Console.WriteLine(id);
            Console.WriteLine(shoes.ShoeId);
            Console.WriteLine(shoes.Image);
            if (shoes == null || id != shoes.ShoeId)
                return BadRequest();

            var isUpdate =await _shoesRepository.Update(shoes);
            if (!isUpdate)
                return NotFound();
            return Ok(new { message = "User Updated successfully" });
        }

        [HttpGet("/ByCategory/{id}")]
        public IActionResult GetAllShoesByCategory(int id)
        {
            var shoes = _shoesRepository.GetShoesByCategoryId(id);
            return Ok(shoes);
        }

        [HttpGet("SearchByName")]
        public IActionResult SearchShoesByName(string shoeName)
        {
            if (string.IsNullOrEmpty(shoeName))
            {
                return BadRequest("Shoe name must be provided.");
            }

            try
            {
                IEnumerable<ShoesModel> shoes = _shoesRepository.SearchShoesByName(shoeName);
    
                if (shoes == null || !shoes.Any())
                {
                    return NotFound("No shoes found with the provided name.");
                }

                return Ok(shoes);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown for simplicity)
                return StatusCode(500, "An error occurred while searching for shoes.");
            }
        }

        [HttpGet("/ShoeDetail/{id}")]
        public IActionResult ShoDetail(int id)
        {
            var shoe = _shoesRepository.SelectByID(id);
            if(shoe == null)
            {
                NotFound("No shoes found with the provided Id.");
            }
            return Ok(shoe);
        }

        [HttpGet("/ShoeByPK/{id}")]
        public IActionResult ShoeByPK(int id)
        {
            var shoe = _shoesRepository.SelectByPK(id);
            if (shoe == null)
            {
                NotFound("No shoes found with the provided Id.");
            }
            return Ok(shoe);
        }
    }
}