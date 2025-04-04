using System.Collections.ObjectModel;
using EventFoodOrders.Dto.ParticipantDTOs;

namespace EventFoodOrders.Dto.EventDTOs
{
    public record EventForResponseWithUsersDto : EventForResponseWithDetailsDto
    {
        public Collection<ParticipantWithUserDto> Participants { get; set; } = [];
    }
}
