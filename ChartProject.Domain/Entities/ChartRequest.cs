using ChartProject.Domain.Entities;
using System;

namespace CharProject.Domain.Entities
{
    public class ChartRequest : IEntity
    {
        public string? DbConnection { get; set; }
        public string? SqlQuery { get; set; }
        public string? ChartType { get; set; }

        // Stored Procedure veya View seçimi için ek alanlar
        public string? SelectedProcedure { get; set; }  // Eğer bir stored procedure seçildiyse
        public string? SelectedView { get; set; }       // Eğer bir view seçildiyse

        // Yeni Eklenen Alanlar: StartDate ve EndDate
        public DateTime? StartDate { get; set; }  // Nullable date, çünkü boş olabilir
        public DateTime? EndDate { get; set; }    // Nullable date, çünkü boş olabilir
        public decimal? Amount { get; set; }            // Satış miktarı filtresi için miktar

        public string? SelectedFunction { get; set; }
        
        // Eklenen yeni parametreler
        public int? ProductId { get; set; }      // Ürün ID'si
  

    }
}
