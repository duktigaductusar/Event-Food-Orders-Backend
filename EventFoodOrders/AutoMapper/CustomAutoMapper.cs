using AutoMapper;

namespace EventFoodOrders.AutoMapper
{
    public class CustomAutoMapper
    {
        private readonly Mapper _mapper = new(AutoMapperConfiguration.GetConfiguration());

        public IMapper Mapper {  get { return _mapper; } }
    }
}
