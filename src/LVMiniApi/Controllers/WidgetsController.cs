using Data.Service.Core.Interfaces;
using Data.Service.Core.MappingClasses;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LVMiniApi.Controllers
{
    /// <summary>
    /// Controller for the LVMini widgets.
    /// </summary>
    [Route("api/widgets")]
    [Produces("application/json")]
    public class WidgetsController : Controller
    {
        private readonly ILoanRepository _loanRepository;

        /// <inheritdoc />
        public WidgetsController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }
        /// <summary>
        /// Gets the total loan amount for the past 3 years for each year.
        /// </summary>
        /// <returns>Http OK with data. Return not found if no data was extracted.</returns>
        [HttpGet]
        [Route("loanperformance")]
        public IActionResult LoanPortfolioPerformance()
        {
            var result = _loanRepository.LoanRequestAmountPerYearInquire();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets all loans of the past year which are closed.
        /// </summary>
        /// <returns>Http OK and a collection of data.</returns>
        [HttpGet]
        [Route("budgetvsactual")]
        public IActionResult BudgetVersusActual()
        {
            List<YearlyBudgetInfo> result = _loanRepository.AllLoansGroupedByProductGroupsInquire();
            if (result == null)
            {
                return NotFound();
            }


            Dictionary<string, YearlyBudgetInfoDto> dataToReturn = new Dictionary<string, YearlyBudgetInfoDto>();
            foreach (YearlyBudgetInfo info in result)
            {
                dataToReturn.Add(info.ProductGroupTitle, new YearlyBudgetInfoDto() { ActualBudget = info.ActualBudget, YearlyBudget = info.YearlyBudget });
            }

            return Ok(dataToReturn);
        }
    }
}