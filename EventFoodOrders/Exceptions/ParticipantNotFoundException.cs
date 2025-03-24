namespace EventFoodOrders.Exceptions;

public class ParticipantNotFoundException : CustomException
{
    public ParticipantNotFoundException() :
        base(message: "Participant not found.")
    { }

    public ParticipantNotFoundException(Guid participantId) :
        base(message: $"Participant with id {participantId} not found.")
    { }

    public ParticipantNotFoundException(string participantId) :
        base(message: $"Participant with id {participantId} not found.")
    { }

    public ParticipantNotFoundException(Guid participantId, Exception innerException) :
        base(message: $"Participant with id {participantId} not found.", innerException)
    { }

    public ParticipantNotFoundException(string participantId, Exception innerException) :
        base(message: $"Participant with id {participantId} not found.", innerException)
    { }
}
