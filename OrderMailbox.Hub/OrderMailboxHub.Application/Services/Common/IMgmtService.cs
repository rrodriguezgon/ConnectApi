namespace OrderMailboxHub.Application.Services.Common
{
    public interface IMgmtService<TDto>
     : IRestfulApiClientService<TDto>
     where TDto : class, new()
    { }
}
