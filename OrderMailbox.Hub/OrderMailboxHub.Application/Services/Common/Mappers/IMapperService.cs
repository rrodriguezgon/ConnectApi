using OrderMailboxHub.Application.Services.Common.Dtos;

namespace OrderMailboxHub.Application.Services.Common.Mappers
{
    public interface IMapperService<TDto>
      where TDto : BaseDto, new()
    {
        TDto Map(string objectAsJson);
    }
}
