namespace OrderMailboxHub.Application.Services.Common
{
    public interface IRestfulApiClientService<TDto>
        where TDto : class, new()
    { }
}
