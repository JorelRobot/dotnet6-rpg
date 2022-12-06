using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(),
            new Character { Id = 1, Name = "Sam" }
        };

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            var response = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            response.Data = await _context.Characters
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToListAsync();

            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> response = new ServiceResponse<List<GetCharacterDTO>>();

            try
            {
                Character character = characters.First(c => c.Id == id);

                characters.Remove(character);
                response.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters.ToListAsync();

            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharactersById(int id)
        {
            var response = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

            try
            {
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                // character.Name = updatedCharacter.Name;
                // character.HitPoints = updatedCharacter.HitPoints;
                // character.Strength = updatedCharacter.Strength;
                // character.Defense = updatedCharacter.Defense;
                // character.Intelligence = updatedCharacter.Intelligence;
                // character.Class = updatedCharacter.Class;

                _mapper.Map(updatedCharacter, character);

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}