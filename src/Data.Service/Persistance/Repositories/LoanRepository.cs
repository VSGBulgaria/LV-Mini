using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Data.Service.Persistance.Repositories
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        public LoanRepository(LvMiniDbContext context) : base(context)
        {
        }

        public Dictionary<string, decimal> GetTotalLoanRequestAmountPerYear()
        {
            var result = Entities
                .Where(loan => loan.IsLoanRequest == true && loan.DateLoanRequestReceived != null)
                .Select(l => new
                {
                    l.DateLoanRequestReceived.Value.Year,
                    Money = l.NewMoney.Value
                })
                .GroupBy(p => p.Year, p => p.Money, (y, m) => new
                {
                    Year = y,
                    Money = m.Sum()
                })
                .OrderByDescending(t => t.Year)
                .Take(3)
                .ToDictionary(x => x.Year.ToString(), y => y.Money);

            return result;
        }
    }
}
