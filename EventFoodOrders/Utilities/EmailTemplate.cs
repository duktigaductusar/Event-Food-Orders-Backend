namespace EventFoodOrders.Utilities;

public class EmailTemplate(string subject, string body)
{
    public string Subject { get; set; } = subject;
    public string Body { get; set; } = body;
}