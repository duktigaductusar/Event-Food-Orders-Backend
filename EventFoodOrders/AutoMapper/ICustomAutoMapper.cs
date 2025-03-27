using AutoMapper;

namespace EventFoodOrders.AutoMapper;

/// <summary>
/// Retrieves a pre-configured IMapper.
/// </summary>
public interface ICustomAutoMapper
{
    IMapper Mapper { get; }
}