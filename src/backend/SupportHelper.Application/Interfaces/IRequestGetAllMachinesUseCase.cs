using SupportHelper.Application.DTOs;
using SupportHelper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestGetAllMachinesUseCase
    {
        Task<ResponsePageableDto<HashSet<Machine>>> ExecuteAsync(int pageNumber, int pageSize);
    }
}
