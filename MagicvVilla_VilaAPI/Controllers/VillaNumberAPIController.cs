using AutoMapper;
using MagicvVilla_VilaAPI.Models;
using MagicvVilla_VilaAPI.Models.Dto;
using MagicvVilla_VilaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicvVilla_VilaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : Controller
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            this._response = new APIResponse();
            _dbVilla = dbVilla;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }


            return _response;

        }
        [HttpGet("{id:int}", Name ="GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if(await _dbVillaNumber.GetAsync(u=>u.VillaNo==createDTO.VillaNo)!=null)
                {
                    ModelState.AddModelError("CustomerError", "Villa Number Already Exists!");
                    return BadRequest(ModelState);
                }
                if(await _dbVilla.GetAsync(u => u.Id == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomerError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
                        
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}",Name ="DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if(villaNumber == null)
                {
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("{id:int}", Name ="UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if(updateDTO == null || id!=updateDTO.VillaNo)
                {
                    return BadRequest();
                }
                if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomerError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

                await _dbVillaNumber.UpdateAsync(model);
                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }

            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        //public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        //{
        //    if (patchDTO == null || id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var villa = await _dbVillaNumber.GetAsync(u => u.VillaNo == id, tracked: false);

        //    VillaNumberUpdateDTO villaDTO = _mapper.Map<VillaNumberUpdateDTO>(villa);


           
        //    if (villa == null)
        //    {
        //        return BadRequest();
        //    }
        //    patchDTO.ApplyTo(villaDTO, ModelState);

        //    Villa model = _mapper.Map<Villa>(villaDTO);
          
        //    await _dbVillaNumber.UpdateAsync(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return NoContent();
        //}

    }
}
