namespace EventFoodOrders.Dto.ParticipantDTOs
{
    public class ParticipantWithUserDto
    {
        public Guid EventId { get; set; }
        public bool WantsMeal { get; set; }
        public string Allergies { get; set; }
        public string Preferences { get; set; }
        public string ResponseType { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
