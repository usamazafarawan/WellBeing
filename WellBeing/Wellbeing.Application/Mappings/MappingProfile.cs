using AutoMapper;
using Wellbeing.Domain.Entities;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Features.Clients.Commands.CreateClients;
using Wellbeing.Application.Features.Clients.Commands.UpdateClients;
using Wellbeing.Application.Features.AspNetUsers.Commands.CreateAspNetUsers;
using Wellbeing.Application.Features.AspNetUsers.Commands.UpdateAspNetUsers;
using Wellbeing.Application.Features.WellbeingDimensions.Commands.CreateWellbeingDimension;
using Wellbeing.Application.Features.WellbeingDimensions.Commands.UpdateWellbeingDimension;
using Wellbeing.Application.Features.WellbeingSubDimensions.Commands.CreateWellbeingSubDimension;
using Wellbeing.Application.Features.WellbeingSubDimensions.Commands.UpdateWellbeingSubDimension;
using Wellbeing.Application.Features.Questions.Commands.CreateQuestion;
using Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;
using System.Text.Json;

namespace Wellbeing.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Clients, ClientsDto>()
            .ForMember(dest => dest.AspNetUsersCount, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .AfterMap((src, dest) =>
            {
                if (!string.IsNullOrEmpty(src.ClientSettings))
                {
                    using var doc = JsonDocument.Parse(src.ClientSettings);
                    dest.ClientSettings = doc.RootElement.Clone();
                }
                else
                {
                    dest.ClientSettings = JsonDocument.Parse("{}").RootElement.Clone();
                }
            });
        CreateMap<CreateClientsCommand, Clients>()
            .ForMember(dest => dest.ClientSettings, opt => opt.MapFrom(src => src.ClientSettings ?? "{}"));
        CreateMap<UpdateClientsCommand, Clients>()
            .ForMember(dest => dest.ClientSettings, opt => opt.MapFrom(src => src.ClientSettings ?? "{}"));

        CreateMap<AspNetUsers, AspNetUsersDto>()
            .ForMember(dest => dest.ClientsName, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientsId));
        CreateMap<CreateAspNetUsersCommand, AspNetUsers>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpperInvariant()))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => 0));
        CreateMap<UpdateAspNetUsersCommand, AspNetUsers>()
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpperInvariant()))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
            .ForMember(dest => dest.PasswordHash, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PasswordHash)))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

        CreateMap<WellbeingDimension, WellbeingDimensionDto>()
            .ForMember(dest => dest.ClientsName, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        CreateMap<CreateWellbeingDimensionCommand, WellbeingDimension>();
        CreateMap<UpdateWellbeingDimensionCommand, WellbeingDimension>();

        CreateMap<WellbeingSubDimension, WellbeingSubDimensionDto>()
            .ForMember(dest => dest.WellbeingDimensionName, opt => opt.Ignore())
            .ForMember(dest => dest.ClientsName, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        CreateMap<CreateWellbeingSubDimensionCommand, WellbeingSubDimension>();
        CreateMap<UpdateWellbeingSubDimensionCommand, WellbeingSubDimension>();

        CreateMap<Question, QuestionDto>()
            .ForMember(dest => dest.WellbeingDimensionName, opt => opt.Ignore())
            .ForMember(dest => dest.WellbeingSubDimensionName, opt => opt.Ignore())
            .ForMember(dest => dest.ClientsName, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        CreateMap<CreateQuestionCommand, Question>();
        CreateMap<UpdateQuestionCommand, Question>();
    }
}
