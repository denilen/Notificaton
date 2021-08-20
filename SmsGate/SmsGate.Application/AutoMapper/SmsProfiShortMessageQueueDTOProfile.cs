using AutoMapper;
using Infrastructure.SmsGate.DataTypes;
using JetBrains.Annotations;
using SmsGate.Application.SmsProfi;

namespace SmsGate.Application.AutoMapper
{
    [UsedImplicitly]
    public class SmsProfiShortMessageQueueDTOProfile : Profile
    {
        public SmsProfiShortMessageQueueDTOProfile()
        {
            CreateMap<SendShortMessageCommand, SmsProfiShortMessageQueueDTO>();
        }
    }
}
