﻿namespace EventFoodOrders.Dto.EventDTOs;

public record EventForUpdateDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset? Date { get; init; }
    public DateTimeOffset? Deadline { get; init; }
}
