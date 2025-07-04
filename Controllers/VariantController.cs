using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Status;
using Beauty_Works.Models.DTO.Variant;
using Beauty_Works.Repositories.Implementation;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly IVariantRepository variantRepo;

        public VariantController(IVariantRepository variantRepo)
        {
            this.variantRepo = variantRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(CreateVariantRequestDto request)
        {
            // Map Domain to Dto
            var variant = new Variant
            {
                Name = request.Name
            };

            await variantRepo.CreateAsync(variant);

            var response = new VariantDto
            {
                ID = variant.ID,
                Name = variant.Name
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVariant()
        {
            var variants = await variantRepo.GetAllAsync();

            // Map Domain to Dto
            var response = new List<VariantDto>();
            foreach (var variant in variants)
            {
                response.Add(new VariantDto
                {
                    ID = variant.ID,
                    Name = variant.Name
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{variantID:int}")]
        public async Task<IActionResult> GetVariantByID([FromRoute] int variantID)
        {
            var existingVariant = await variantRepo.GetByID(variantID);

            if (existingVariant == null)
            {
                return NotFound();
            }

            // Map Domain to Dto
            var response = new VariantDto
            {
                ID = existingVariant.ID,
                Name = existingVariant.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{variantID:int}")]
        public async Task<IActionResult> UpdateVariant([FromRoute] int variantID, [FromBody] UpdateVariantRequestDto request)
        {
            var variant = new Variant
            {
                ID = variantID,
                Name = request.Name
            };

            variant = await variantRepo.UpdateAsync(variant);

            if (variant == null)
            {
                return NotFound();
            }

            // Map Domain to dto
            var response = new VariantDto
            {
                ID = variantID,
                Name = variant.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{variantID:int}")]
        public async Task<IActionResult> DeleteVariant([FromRoute] int variantID)
        {
            var variant = await variantRepo.DeleteAsync(variantID);

            if (variant == null)
            {
                return NotFound();
            }

            var response = new VariantDto
            {
                ID = variant.ID,
                Name = variant.Name
            };

            return Ok(response);
        }
    }
}
