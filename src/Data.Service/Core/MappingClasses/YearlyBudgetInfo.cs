using Data.Service.Core.Interfaces;

namespace Data.Service.Core.MappingClasses
{
    public class YearlyBudgetInfo
    {
        public YearlyBudgetInfo(string productGroupTitle, decimal yearlyBudget, decimal actualBudget)
        {
            ProductGroupTitle = productGroupTitle;
            YearlyBudget = yearlyBudget;
            ActualBudget = actualBudget;
        }

        public string ProductGroupTitle { get; set; }
        public decimal YearlyBudget { get; set; }
        public decimal ActualBudget { get; set; }
    }
}