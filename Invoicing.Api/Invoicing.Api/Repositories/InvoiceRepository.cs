using System.Data;
using Microsoft.Data.SqlClient;
using Invoicing.Api.Database;
using Invoicing.Api.DTOs;
using Invoicing.Api.Models;

namespace Invoicing.Api.Repositories
{
    public class InvoiceRepository
    {
        private readonly DbHelper _db;

        public InvoiceRepository(DbHelper db)
        {
            _db = db;
        }

        // Helper → Convert List<InvoiceItemDto> to SQL DataTable
        private DataTable CreateItemTable(List<InvoiceItemDto> items)
        {
            var table = new DataTable();
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Rate", typeof(decimal));
            table.Columns.Add("DiscountPercent", typeof(decimal));
            table.Columns.Add("DiscountAmount", typeof(decimal));

            foreach (var item in items)
            {
                table.Rows.Add(
                    item.ProductName,
                    item.Quantity,
                    item.Rate,
                    item.DiscountPercent,
                    item.DiscountAmount
                );
            }

            return table;
        }

        // 1️⃣ Add Invoice
        public async Task<int> AddInvoiceAsync(AddInvoiceRequest request)
        {
            using var conn = _db.GetConnection();
            using var cmd = new SqlCommand("sp_AddInvoice", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // header params
            cmd.Parameters.AddWithValue("@CustomerId", request.CustomerId);
            cmd.Parameters.AddWithValue("@InvoiceDate", request.InvoiceDate);

            cmd.Parameters.AddWithValue("@BillingAddressLine1", request.BillingAddressLine1);
            cmd.Parameters.AddWithValue("@BillingAddressLine2",
                (object?)request.BillingAddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillingCity", request.BillingCity);
            cmd.Parameters.AddWithValue("@BillingState", request.BillingState);
            cmd.Parameters.AddWithValue("@BillingPostalCode",
                (object?)request.BillingPostalCode ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@ShippingAddressLine1", request.ShippingAddressLine1);
            cmd.Parameters.AddWithValue("@ShippingAddressLine2",
                (object?)request.ShippingAddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ShippingCity", request.ShippingCity);
            cmd.Parameters.AddWithValue("@ShippingState", request.ShippingState);
            cmd.Parameters.AddWithValue("@ShippingPostalCode",
                (object?)request.ShippingPostalCode ?? DBNull.Value);

            // items
            var itemsTable = CreateItemTable(request.Items);
            var itemsParam = cmd.Parameters.AddWithValue("@Items", itemsTable);
            itemsParam.SqlDbType = SqlDbType.Structured;
            itemsParam.TypeName = "AddInvoiceItemType";

            // output
            var outputParam = new SqlParameter("@NewInvoiceId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)outputParam.Value;
        }

        // 2️⃣ Edit Invoice
        public async Task EditInvoiceAsync(int invoiceId, EditInvoiceRequest request)
        {
            using var conn = _db.GetConnection();
            using var cmd = new SqlCommand("sp_EditInvoice", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
            cmd.Parameters.AddWithValue("@InvoiceDate", request.InvoiceDate);

            cmd.Parameters.AddWithValue("@BillingAddressLine1", request.BillingAddressLine1);
            cmd.Parameters.AddWithValue("@BillingAddressLine2",
                (object?)request.BillingAddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillingCity", request.BillingCity);
            cmd.Parameters.AddWithValue("@BillingState", request.BillingState);
            cmd.Parameters.AddWithValue("@BillingPostalCode",
                (object?)request.BillingPostalCode ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@ShippingAddressLine1", request.ShippingAddressLine1);
            cmd.Parameters.AddWithValue("@ShippingAddressLine2",
                (object?)request.ShippingAddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ShippingCity", request.ShippingCity);
            cmd.Parameters.AddWithValue("@ShippingState", request.ShippingState);
            cmd.Parameters.AddWithValue("@ShippingPostalCode",
                (object?)request.ShippingPostalCode ?? DBNull.Value);

            var itemsTable = CreateItemTable(request.Items);
            var itemsParam = cmd.Parameters.AddWithValue("@Items", itemsTable);
            itemsParam.SqlDbType = SqlDbType.Structured;
            itemsParam.TypeName = "AddInvoiceItemType";

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // 3️⃣ Void Invoice
        public async Task VoidInvoiceAsync(int invoiceId)
        {
            using var conn = _db.GetConnection();
            using var cmd = new SqlCommand("sp_VoidInvoice", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // 4️⃣ Search Invoices
        public async Task<List<InvoiceResponse>> SearchInvoicesAsync(SearchInvoiceRequest req)
        {
            var results = new List<InvoiceResponse>();

            using var conn = _db.GetConnection();
            using var cmd = new SqlCommand("sp_SearchInvoices", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CustomerId", (object?)req.CustomerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InvoiceNumber", (object?)req.InvoiceNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FromDate", (object?)req.FromDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ToDate", (object?)req.ToDate ?? DBNull.Value);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var invoice = new InvoiceResponse
                {
                    InvoiceId = reader.GetInt32(0),
                    InvoiceNumber = reader.GetString(1),
                    CustomerId = reader.GetInt32(2),
                    // CustomerName is ignored in DTO (we can add if needed)
                    InvoiceDate = reader.GetDateTime(4),
                    Subtotal = reader.GetDecimal(5),
                    TotalDiscount = reader.GetDecimal(6),
                    TotalAmount = reader.GetDecimal(7),
                    Status = reader.GetString(8)
                };

                results.Add(invoice);
            }

            return results;
        }
    }
}
