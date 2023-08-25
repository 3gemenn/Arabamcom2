using Arabamcom2.DTOs;
using Arabamcom2.IService;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Newtonsoft.Json;
using Arabamcom2.FluentValidation;
using FluentValidation;

namespace Arabamcom2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly IAdvertService _advertService;
       
        public HomeController(IAdvertService advertService)
        {
            _advertService = advertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvertById([FromQuery]IdDto dto)
        {
            //var validator = new IdValidator();
            //var validationResult = await validator.ValidateAsync(dto);

            //if (!validationResult.IsValid)
            //{
            //    return BadRequest("Validation Error");
            //}

           
            var result = await _advertService.GetAdvertById(dto);
            if (result.StatusCode != 200)
            {
                return BadRequest("Error");
            }
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdvert(int pageSize, int pageNumber)
        {
            var result = await _advertService.GetAllAdvert(pageSize, pageNumber);
            if (result.StatusCode != 200)
            {
                return BadRequest("Error");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAdvert(AdvertDto dto)
        {
            var validator = new AdvertValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest("Validation Error");
            }

            try
            {
                var result = await _advertService.InsertAdvert(dto);
                return Ok(result);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(IdDto id)
        {
            var validator = new IdValidator();
            var validationResult = await validator.ValidateAsync(id);

            if (!validationResult.IsValid)
            {
                return BadRequest("Validation Error");
            }

            try
            {
                bool deleted = await _advertService.DeleteAdvert(id);

                if (deleted)
                {
                    return Ok("Advert deleted successfully.");
                }
                else
                {
                    return NotFound("Advert not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }


        [HttpPut("adverts/{id}")]
        public async Task<IActionResult> UpdateAdvert(string id, AdvertDto dto)
        {

            var validator = new AdvertValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest("Validation Error");
            }

            try
            {
                if (dto == null || dto.Id != id)
                {
                    return BadRequest("Invalid request data.");
                }

                var updatedAdvertResult = await _advertService.UpdateAdvert(dto);

                if (updatedAdvertResult.StatusCode == 200)
                {
                    return Ok("Advert updated successfully.");
                }
                else
                {
                    return NotFound("Advert not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


    }
}
