namespace EventFoodOrders.Exceptions;

public class ParticipantNotFoundException : Exception
{
    public ParticipantNotFoundException() :
        base("Participant not found.")
    { }

    public ParticipantNotFoundException(Guid participantId) :
        base($"Participant with id {participantId} not found.")
    { }

    public ParticipantNotFoundException(string participantId) :
        base($"Participant with id {participantId} not found.")
    { }

    public ParticipantNotFoundException(Guid participantId, Exception innerException) :
        base($"Participant with id {participantId} not found.", innerException)
    { }

    public ParticipantNotFoundException(string participantId, Exception innerException) :
        base($"Participant with id {participantId} not found.", innerException)
    { }
}
