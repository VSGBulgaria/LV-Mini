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

        [Required]
        public decimal NewMoney { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public bool IsLoanRequest { get; set; }

        [Required]
        public decimal ExpectedFundingAtClosing { get; set; }

        [Required]
        public DateTime ProposedCloseDate { get; set; }

        [Required]
        public DateTime DateLoanRequestReceived { get; set; }

        [Required]
        public DateTime DecisionDate { get; set; }
    }
}
