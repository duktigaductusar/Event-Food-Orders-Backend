﻿using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class ServiceManager(IAuthService authService, IEventService eventService, IParticipantService participantService, IUserService userService) : IServiceManager
{
    public IAuthService AuthService { get; set; } = authService;
    public IEventService EventService { get; set; } = eventService;
    public IParticipantService ParticipantService { get; set; } = participantService;
    public IUserService UserService { get; set; } = userService;

}
