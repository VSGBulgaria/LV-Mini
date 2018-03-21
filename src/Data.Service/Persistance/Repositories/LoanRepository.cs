using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Data.Service.Core.MappingClasses;
using System;

namespace Data.Service.Persistance.Repositories
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        public LoanRepository(LvMiniDbContext context) : base(context)
        {
        }

        public Dictionary<string, decimal> LoanRequestAmountPerYearInquire()
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            using (SqlConnection connection = Context.Database.GetDbConnection() as SqlConnection)
            {
                if (connection != null)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "IbClue.GetLoanAmountPerYear";

                        connection.Open();

                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(reader[0].ToString(), reader.GetDecimal(1));
                            }
                        }

                        connection.Close();
                        command.Dispose();
                    }
                }
            }

            return result;
        }

        public List<YearlyBudgetInfo> AllLoansGroupedByProductGroupsInquire()
        {
            List<YearlyBudgetInfo> resultCollection = new List<YearlyBudgetInfo>();

            var connection = Context.Database.GetDbConnection();

            using (SqlConnection dbConnection = new SqlConnection(connection.ConnectionString))
            using (SqlCommand dbCommand = new SqlCommand("usp_GetBudgetVSActualWidgetInfo", dbConnection))
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbConnection.Open();

                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    resultCollection.Add(new YearlyBudgetInfo(reader.GetString(0), Convert.ToDecimal(reader[1]), Convert.ToDecimal(reader[2])));
                }

                dbConnection.Close();
            }

            return resultCollection;
        }
    }
}
