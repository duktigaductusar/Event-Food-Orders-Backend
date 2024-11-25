namespace EventFoodOrders.Dto;

public class EventDTO
{
    private Guid eventId;
    private String EventName { get; set; }
    private DateTime EventDate { get; set; }

    public Guid getEventId()
    {
        return eventId;
    }

    public void setEventId(Guid eventId)
    {
        this.eventId = eventId;
    }

    public String getEventName()
    {
        return EventName;
    }

    public void setEventName(String eventName)
    {
        this.EventName = eventName;
    }

    public DateTime getEventDate()
    {
        return EventDate;

    }

    public void setEventDate(DateTime eventDate)
    {
        this.EventDate = eventDate;
    }
}
