using AutoMapper;
using Infrastructure.SmsGate.DataTypes;
using JetBrains.Annotations;
using SmsGate.Application.Nikita;

namespace SmsGate.Application.AutoMapper
{
    [UsedImplicitly]
    public class NikitaShortMessageQueueDTOProfile : Profile
    {
        public NikitaShortMessageQueueDTOProfile()
        {
            CreateMap<SendShortMessageCommand, NikitaShortMessageQueueDTO>();
        }
    }
}
