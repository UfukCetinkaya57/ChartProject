 using CharProject.Application.Features.ChartRequests.Commands;
using CharProject.Application.Features.ChartRequests.Queries;
using CharProject.Domain.Entities;
using ChartProject.Application.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CharProject.Infrastructure.Services
{
    public class ChartDataService : IChartDataService
    {
        public async Task<GetChartDataResponse> GetChartData(ChartRequest chartRequest)
        {
            var labels = new List<string>();
            var data = new List<decimal>();
            List<DateTime> dates = new List<DateTime>();  // Tarihler

            using (SqlConnection connection = new SqlConnection($"{chartRequest.DbConnection};TrustServerCertificate=True"))
            {
                await connection.OpenAsync();

                // Eğer view seçilmişse, view sorgusu çalıştırılacak
                if (!string.IsNullOrEmpty(chartRequest.SelectedView))
                {
                    string sqlQuery = $"SELECT * FROM {chartRequest.SelectedView}";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Type fieldType = reader.GetFieldType(i);

                                    // Sütunun türüne göre dinamik olarak işlem yap
                                    if (fieldType == typeof(int))
                                    {
                                        labels.Add(reader.GetInt32(i).ToString());
                                    }
                                    else if (fieldType == typeof(string))
                                    {
                                        labels.Add(reader.GetString(i));
                                    }
                                    else if (fieldType == typeof(decimal))
                                    {
                                        data.Add(reader.GetDecimal(i));
                                    }
                                    else if (fieldType == typeof(DateTime))
                                    {
                                        dates.Add(reader.GetDateTime(i));
                                    }

                                }
                            }
                        }
                    }
                }

                // Eğer procedure seçilmişse, procedure çağrısı çalıştırılacak
                else if (!string.IsNullOrEmpty(chartRequest.SelectedProcedure))
                {
                    string sqlQuery = $"EXEC {chartRequest.SelectedProcedure}";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        if (chartRequest.SelectedProcedure == "GetSalesDataByDateRange")
                        {
                            command.CommandText = "EXEC GetSalesDataByDateRange @StartDate, @EndDate";

                            command.Parameters.AddWithValue("@StartDate", chartRequest.StartDate ?? DateTime.MinValue);
                            command.Parameters.AddWithValue("@EndDate", chartRequest.EndDate ?? DateTime.MaxValue);
                        }
                        else if (chartRequest.SelectedProcedure == "GetSalesAboveAmount")
                        {
                            command.CommandText = "EXEC GetSalesAboveAmount @Amount";
                            command.Parameters.AddWithValue("@Amount", chartRequest.Amount ?? 0);
                        }

                        else if (chartRequest.SelectedProcedure == "GetSalesCountByDateRange")
                        {
                            command.CommandText = "EXEC GetSalesCountByDateRange @StartDate, @EndDate";

                            command.Parameters.AddWithValue("@StartDate", chartRequest.StartDate ?? DateTime.MinValue);
                            command.Parameters.AddWithValue("@EndDate", chartRequest.EndDate ?? DateTime.MaxValue);
                        }
                        else if (chartRequest.SelectedProcedure == "GetSalesByProductId")
                        {
                            command.CommandText = "EXEC GetSalesByProductId @ProductId";

                            command.Parameters.AddWithValue("@ProductId", chartRequest.ProductId ?? 0);
                        }
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (chartRequest.SelectedProcedure == "GetSalesCountByDateRange")
                                {
                                    labels.Add("Total Number of Sales");
                                    data.Add(reader.GetInt32(0)); // İlk sütun toplam satış
                                    break;
                                }
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var fieldType = reader.GetFieldType(i);

                                    if (fieldType == typeof(int))
                                    {
                                        labels.Add(reader.GetInt32(i).ToString());
                                    }
                                    else if (fieldType == typeof(string))
                                    {
                                        labels.Add(reader.GetString(i));
                                    }
                                    else if (fieldType == typeof(decimal))
                                    {
                                        data.Add(reader.GetDecimal(i));
                                    }
                                }
                            }
                        }
                    }
                }
                // Eğer function seçilmişse, function çağrısı çalıştırılacak
                else if (!string.IsNullOrEmpty(chartRequest.SelectedFunction))
                {
                    string sqlQuery = $"SELECT * FROM {chartRequest.SelectedFunction}()"; // Function çağrısı
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        if (chartRequest.SelectedFunction == "GetSalesWithinDateRange")
                        {
                            command.CommandText = " SELECT * FROM dbo.GetSalesWithinDateRange (@StartDate, @EndDate)";

                            command.Parameters.AddWithValue("@StartDate", chartRequest.StartDate ?? DateTime.MinValue);
                            command.Parameters.AddWithValue("@EndDate", chartRequest.EndDate ?? DateTime.MaxValue);
                        }
                        else if (chartRequest.SelectedFunction == "GetSalesByProduct")
                        {
                            command.CommandText = "SELECT * FROM dbo.GetSalesByProduct(@ProductId)";
                            command.Parameters.AddWithValue("@ProductId", chartRequest.ProductId);
                        }
                        else if (chartRequest.SelectedFunction == "GetAverageSalesAmount")
                        {
                            command.CommandText = "SELECT dbo.GetAverageSalesAmount()";
                        }
                        else if (chartRequest.SelectedFunction == "GetTotalSalesAmount")
                        {
                            command.CommandText = "SELECT dbo.GetTotalSalesAmount()";
                        }

                        //else if (chartRequest.ChartRequest.SelectedFunction == "GetTopSellingProduct")
                        //{
                        //    command.Parameters.AddWithValue("@ProductId", chartRequest.ChartRequest.ProductId ?? 0);
                        //}
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (chartRequest.SelectedFunction == "GetAverageSalesAmount")
                                {
                                    labels.Add("Average Sales Amount");
                                    data.Add(reader.GetDecimal(0)); // İlk sütun toplam satış
                                    break;
                                }
                                else if (chartRequest.SelectedFunction == "GetTotalSalesAmount")
                                {
                                    // Bu durumda, verinin dönen bir sayı olduğunu kabul ederek bir label ekliyoruz
                                    labels.Add("Total Sales Amount");
                                    data.Add(reader.GetDecimal(0)); // İlk sütun toplam satış
                                    break;
                                }
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var fieldType = reader.GetFieldType(i);

                                    if (fieldType == typeof(int))
                                    {
                                        labels.Add(reader.GetInt32(i).ToString());
                                    }
                                    else if (fieldType == typeof(string))
                                    {
                                        labels.Add(reader.GetString(i));
                                    }
                                    else if (fieldType == typeof(decimal))
                                    {
                                        data.Add(reader.GetDecimal(i));
                                    }
                                }
                            }
                        }

                    }
                }

            }

            return new GetChartDataResponse { Labels = labels, Data = data };
        }

        public async Task<GetProceduresViewsAndFunctionsResponse> GetProceduresViewsAndFunctions(string dbConnection)
        {
            var procedures = new List<string>();
            var views = new List<string>();
            var functions = new List<string>();

            using (SqlConnection connection = new SqlConnection($"{dbConnection};TrustServerCertificate=True"))
            {
                await connection.OpenAsync();

                // Stored Procedures Listeleme
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.procedures where name NOT LIKE 'sp_%'", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            procedures.Add(reader.GetString(0));
                        }
                    }
                }

                // Views Listeleme
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.views ", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            views.Add(reader.GetString(0));
                        }
                    }
                }

                // Functions Listeleme
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.objects WHERE (type = 'FN' OR type = 'TF' OR type = 'IF') AND name NOT LIKE 'fn_%'", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            functions.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return new GetProceduresViewsAndFunctionsResponse
            {
                Procedures = procedures,
                Views = views,
                Functions = functions
            };
        }
    }
}
