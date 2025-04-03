using AutoMapper;

namespace EventFoodOrders.AutoMapper;

internal static class AutoMapperConfiguration
{
    internal static MapperConfiguration GetConfiguration()
    {
        return new MapperConfiguration(config =>
        {
            config.AddProfile<AutoMapperParticipantProfile>();
            config.AddProfile<AutoMapperEventProfile>();
        });
    }
}
