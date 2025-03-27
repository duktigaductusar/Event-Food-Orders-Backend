namespace EventFoodOrders.Dto.UserDTOs
{
    public record UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
