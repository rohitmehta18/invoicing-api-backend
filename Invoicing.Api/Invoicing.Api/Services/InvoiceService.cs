
using Invoicing.Api.DTOs;
using Invoicing.Api.Repositories;
using System.Text;

namespace Invoicing.Api.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _repo;
        private readonly EmailService _email;

        public InvoiceService(InvoiceRepository repo, EmailService email)
        {
            _repo = repo;
            _email = email;
        }

        // 1️⃣ Add Invoice + send HTML email with totals
        public async Task<int> AddInvoiceAsync(AddInvoiceRequest request)
        {
            if (request.Items == null || request.Items.Count == 0)
                throw new Exception("Invoice must contain at least 1 item.");

            // calculate totals (same logic as SQL SP)
            decimal subtotal = 0;
            decimal totalDiscount = 0;

            foreach (var i in request.Items)
            {
                subtotal += (i.Quantity * i.Rate);
                totalDiscount += i.DiscountAmount + (i.Quantity * i.Rate * i.DiscountPercent / 100);
            }

            decimal totalAmount = subtotal - totalDiscount;

            // save invoice to DB
            int invoiceId = await _repo.AddInvoiceAsync(request);

            // build HTML items table dynamically
            StringBuilder itemsHtml = new StringBuilder();
            foreach (var i in request.Items)
            {
                itemsHtml.Append($@"
                    <tr>
                        <td>{i.ProductName}</td>
                        <td>{i.Quantity}</td>
                        <td>{i.Rate}</td>
                        <td>{i.DiscountPercent}</td>
                        <td>{i.DiscountAmount}</td>
                    </tr>");
            }

            // full HTML invoice template
            string html = $@"
<html>
<head>
<style>
    body {{
        font-family: Arial, sans-serif;
        color: #333;
        line-height: 1.6;
    }}
    .container {{
        max-width: 600px;
        margin: auto;
        padding: 20px;
        border: 1px solid #eee;
        border-radius: 8px;
        background: #fafafa;
    }}
    .header {{
        text-align: center;
        margin-bottom: 20px;
    }}
    .table {{
        width: 100%;
        border-collapse: collapse;
        margin-top: 15px;
    }}
    .table th, .table td {{
        border: 1px solid #ddd;
        padding: 8px;
        text-align: center;
    }}
    .footer {{
        margin-top: 30px;
        font-size: 12px;
        text-align: center;
        color: #777;
    }}
</style>
</head>

<body>
<div class='container'>
    <div class='header'>
        <h2>Invoice Confirmation</h2>
        <p>Your invoice has been successfully created.</p>
    </div>

    <h3>Invoice Details</h3>
    <p><strong>Invoice Number:</strong> INV-{invoiceId}</p>
    <p><strong>Date:</strong> {request.InvoiceDate:yyyy-MM-dd}</p>
    <p><strong>Customer ID:</strong> {request.CustomerId}</p>

    <h3>Billing Address</h3>
    <p>
        {request.BillingAddressLine1}<br/>
        {request.BillingCity}, {request.BillingState}<br/>
        {request.BillingPostalCode}
    </p>

    <h3>Shipping Address</h3>
    <p>
        {request.ShippingAddressLine1}<br/>
        {request.ShippingCity}, {request.ShippingState}<br/>
        {request.ShippingPostalCode}
    </p>

    <h3>Order Items</h3>
    <table class='table'>
        <tr>
            <th>Product</th>
            <th>Qty</th>
            <th>Rate</th>
            <th>Discount(%)</th>
            <th>Discount Amt</th>
        </tr>
        {itemsHtml}
    </table>

    <h3>Summary</h3>
    <p><strong>Subtotal:</strong> ₹{subtotal}</p>
    <p><strong>Total Discount:</strong> ₹{totalDiscount}</p>
    <p><strong>Total Amount:</strong> ₹{totalAmount}</p>

    <div class='footer'>
        <p>Thank you for your purchase!</p>
        <p>This is an auto-generated email. Please do not reply.</p>
    </div>
</div>
</body>
</html>";

            // send email
            await _email.SendInvoiceEmail(
                toEmail: "rohit.mehta.s84@kalvium.community",
                subject: $"Invoice Created (INV-{invoiceId})",
                htmlBody: html
            );

            return invoiceId;
        }

        // 2️⃣ Edit
        public async Task EditInvoiceAsync(int invoiceId, EditInvoiceRequest request)
        {
            if (request.Items == null || request.Items.Count == 0)
                throw new Exception("Invoice must contain at least 1 item.");

            await _repo.EditInvoiceAsync(invoiceId, request);
        }

        // 3️⃣ Void
        public async Task VoidInvoiceAsync(int invoiceId)
        {
            await _repo.VoidInvoiceAsync(invoiceId);
        }

        // 4️⃣ Search
        public async Task<List<InvoiceResponse>> SearchInvoicesAsync(SearchInvoiceRequest request)
        {
            return await _repo.SearchInvoicesAsync(request);
        }
    }
}
