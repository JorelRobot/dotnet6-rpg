using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharactersById(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Delete(int id)
        {
            var response = await _characterService.DeleteCharacter(id);

            if (response.Data == null){
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<List<ServiceResponse<GetCharacterDTO>>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> UpdateCharacter(UpdateCharacterDTO updatedCharacter) 
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);

            if (response.Data == null){
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}