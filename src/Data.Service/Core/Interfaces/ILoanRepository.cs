using Data.Service.Core.Entities;
using System.Collections.Generic;

namespace Data.Service.Core.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Dictionary<string, decimal> GetTotalLoanRequestAmountPerYear();
    }
}
