using AutoMapper;
using CsApi.Models.Domain;
using CsApi.Models.Dto;

namespace CsApi.Mappers;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<User, SentUserDto>();
        CreateMap<User, UserToNotifyDto>();
        CreateMap<UpdateUserDto, User>();
    }
}