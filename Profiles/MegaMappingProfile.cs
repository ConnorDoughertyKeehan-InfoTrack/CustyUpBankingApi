using AutoMapper;
using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Models.UpBanking;
using System.Globalization;
using CustyUpBankingApi.Services;
using CustyUpBankingApi.Models.Requests;
using CustyUpBankingApi.Extensions;

namespace CustyUpBankingApi.Profiles
{
    public class MegaMappingProfile : Profile
    {
        public MegaMappingProfile()
        {
            CreateMap<UpTransaction, SpendTransaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Assuming Id is auto-generated
            .ForMember(dest => dest.DateSpend, opt => opt.MapFrom(src => src.Attributes.CreatedAt))
            .ForMember(dest => dest.DaySpend, opt => opt.MapFrom(src => (src.Attributes.CreatedAt).DayOfWeek.ToString()))
            .ForMember(dest => dest.Category, opt => opt.Ignore()) // Map as needed if Categories is an enum or another object
            .ForMember(dest => dest.TransactionDescription, opt => opt.MapFrom(src => src.Attributes.Description))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => Math.Abs(Convert.ToDecimal(src.Attributes.Amount.Value)))) //Turn negative into positive
            .ForMember(dest => dest.WeekNumber, opt => opt.MapFrom(src => src.Attributes.CreatedAt.GetWeekNumber()))
            .ForMember(dest => dest.UpTransactionId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Attributes.TransactionType));

		}
    }
}
