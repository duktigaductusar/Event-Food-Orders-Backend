﻿using EventFoodOrders.Dto.UserDTOs;

namespace EventFoodOrders.Services.Interfaces
{
    public interface IUserService
    {
        List<UserDto> GetUsers(string queryString);
        string GetNameWithId(Guid userId);
        List<string> GetNamesWithIds(List<Guid> userIds);
        void SendEmail(List<Guid> userIds, string message);
    }
}