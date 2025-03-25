namespace EventFoodOrders.Exceptions;

public class ParticipantNotFoundInEventException : CustomException
{
    public ParticipantNotFoundInEventException() :
        base(StatusCodes.Status400BadRequest, message: "Participant not found.")
    { }

    public ParticipantNotFoundInEventException(string message = "Participant not found.") :
        base(StatusCodes.Status400BadRequest, message: message)
    { }

    public ParticipantNotFoundInEventException(Guid participantId, Guid eventId) :
        this(message: $"Participant with id {participantId} not found in event with id {eventId}.")
    { }
}
