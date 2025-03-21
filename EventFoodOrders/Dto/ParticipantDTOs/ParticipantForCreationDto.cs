namespace EventFoodOrders.Dto.ParticipantDTOs;

public class ParticipantForCreationDto
{
    public required Guid UserId;

    public ParticipantForCreationDto(Guid userId)
    {
        UserId = userId;
    }
}
