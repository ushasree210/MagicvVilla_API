﻿using AutoMapper;
using MagicvVilla_VilaAPI.Models;
using MagicvVilla_VilaAPI.Models.Dto;

namespace MagicvVilla_VilaAPI
{
    public class MappingConfig : Profile
    {


        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}