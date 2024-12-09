namespace EventFoodOrders.Dto;

public class EventDTO
{
    public Guid eventId { get; set; }
    public String EventName { get; set; }
    public DateTime EventDate { get; set; }
    public String Description { get; set; }
    public bool Action { get; set; }

}
