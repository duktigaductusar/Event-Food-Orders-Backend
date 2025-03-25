using AutoMapper;

// Gets a mapper with a configuration that includes the profiles with mappings for this project's DTOs.
namespace EventFoodOrders.AutoMapper;

public class CustomAutoMapper : ICustomAutoMapper
{
    private readonly Mapper _mapper = new(AutoMapperConfiguration.GetConfiguration());

    public IMapper Mapper { get { return _mapper; } }
}
