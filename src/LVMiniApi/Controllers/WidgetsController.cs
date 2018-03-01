using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        /// <returns></returns>
        [HttpGet]
        [Route("loanperformance")]
        public IActionResult LoanPortfolioPerformance()
        {
            var result = _loanRepository.GetTotalLoanRequestAmountPerYear();

            return Ok(result);
        }
    }
}