namespace EventFoodOrders.Exceptions;

public class ParticipantNotFoundException : CustomException
{
    public ParticipantNotFoundException() :
        base(StatusCodes.Status400BadRequest, message: "Participant not found.")
    { }

    public ParticipantNotFoundException(string message = "Participant not found.") :
        base(StatusCodes.Status400BadRequest, message: message)
    { }

    public ParticipantNotFoundException(Guid participantId) :
        this(message: $"Participant with id {participantId} not found.")
    { }
}
