using AutoMapper;
using MagicvVilla_VilaAPI.Data;
using MagicvVilla_VilaAPI.Models;
using MagicvVilla_VilaAPI.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicvVilla_VilaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController:ControllerBase

    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        //private readonly ILogger<VillaAPIController> _logger;    
        public VillaAPIController(ApplicationDbContext db,IMapper mapper) 
        {
           _db=db;
            _mapper=mapper;
        }
        [HttpGet]   

        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            //_logger.LogInformation("Getting all villas ");

            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }
        [HttpGet("id",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200,Type=typeof(VillaDTO)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if(id==0)
            {
               // _logger.LogInformation("Get villa Error with Id" + id);
                return BadRequest();

            }
            var villa= await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createDTO) 
        
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if ( await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == createDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CustomErro", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            Villa model = _mapper.Map<Villa>(createDTO);
            //Villa model = new()
            //{
            //    Amenity = createDTO.Amenity,
            //    Details = createDTO.Details,
               
            //    ImageUrl = createDTO.ImageUrl,
            //    Name = createDTO.Name,
            //    Occupancy = createDTO.Occupancy,
            //    Rate = createDTO.Rate,
            //    Sqft = createDTO.Sqft,
            //};
          await _db.Villas.AddAsync(model);
            await  _db.SaveChangesAsync();
         


            return CreatedAtRoute("GetVilla",new { id =model.Id },model);
        }

        [HttpDelete("id", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u=>u.Id==id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
           await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("id", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null|| id != updateDTO.Id) 
            {
                return BadRequest();
            }
            //var villa= VillaStore.villaList.FirstOrDefault(v => v.Id==id);
            //villa.Name= villaDTO.Name;
            //villa.Sqft= villaDTO.Sqft;
            //villa.Occupancy= villaDTO.Occupancy;
            Villa model = _mapper.Map<Villa>(updateDTO);
            //Villa model = new()
            //{
            //    Amenity = updateDTO.Amenity,
            //    Details = updateDTO.Details,
            //    Id = updateDTO.Id,
            //    ImageUrl = updateDTO.ImageUrl,
            //    Name = updateDTO.Name,
            //    Occupancy = updateDTO.Occupancy,
            //    Rate = updateDTO.Rate,
            //    Sqft = updateDTO.Sqft,
            //};
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent() ;
        }
        [HttpPatch("id", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);


          //VillaUpdateDTO villaDTO = new()
          //  {
          //      Amenity = villa.Amenity,
          //      Details = villa.Details,
          //      Id = villa.Id,
          //      ImageUrl = villa.ImageUrl,
          //      Name = villa.Name,
          //      Occupancy = villa.Occupancy,
          //      Rate = villa.Rate,
          //      Sqft = villa.Sqft,
          //  };

            if (villa==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);
            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //};
            _db.Villas.Update(model);
           await _db.SaveChangesAsync();  

            if (!ModelState.IsValid) 
            {
             return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
