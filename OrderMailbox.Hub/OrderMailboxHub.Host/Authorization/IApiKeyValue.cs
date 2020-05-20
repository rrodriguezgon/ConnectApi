using System.Collections.Generic;

namespace OrderMailboxHub.Host.Authorization
{
    public interface IApiKeyValue
    {
        string EncryptionKey { get; set; }

        string ObjectIdentifier { get; set; }

        string CountryId { get; set; }

        string ToEncryptString();

        string ToString();
    }
}