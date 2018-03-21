using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("Loan", Schema = "IbClue")]
    public class Loan
    {
        [Key]
        public int IDLoan { get; set; }

        [Required]
        public int IDAccount { get; set; }

        public int? IdLoanSource { get; set; }

        public decimal? NewMoney { get; set; }

        public DateTime? LoanDate { get; set; }

        public bool? IsLoanRequest { get; set; }

        public decimal? ExpectedFundingAtClosing { get; set; }

        public DateTime? ProposedCloseDate { get; set; }

        public DateTime? DateLoanRequestReceived { get; set; }

        public DateTime? DecisionDate { get; set; }

        public decimal? LoanAmount { get; set; }

        public int? DaysPastDue { get; set; }
    }
}
