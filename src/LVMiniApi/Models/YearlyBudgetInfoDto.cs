namespace LVMiniApi.Models
{
    /// <summary>
    /// A model for mapping the data from the database.
    /// </summary>
    public class YearlyBudgetInfoDto
    {
        /// <summary>
        /// The yearly budget.
        /// </summary>
        public decimal YearlyBudget { get; set; }

        /// <summary>
        /// the actual budget.
        /// </summary>
        public decimal ActualBudget { get; set; }
    }
}
