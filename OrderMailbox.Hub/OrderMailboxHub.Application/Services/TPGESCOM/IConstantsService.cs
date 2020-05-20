using OrderMailboxHub.Application.Dtos.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.TPGESCOM
{
    public interface IConstantsService
    {
        Task<List<ConstantsDto>> GetAllConstants(string countryId);
    }
}
