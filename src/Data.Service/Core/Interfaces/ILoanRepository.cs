using Data.Service.Core.MappingClasses;
using System.Collections.Generic;

namespace Data.Service.Core.Interfaces
{
    public interface ILoanRepository
    {
        Dictionary<string, decimal> LoanRequestAmountPerYearInquire();
        List<YearlyBudgetInfo> AllLoansGroupedByProductGroupsInquire();
    }
}
