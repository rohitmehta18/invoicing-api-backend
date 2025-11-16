using Microsoft.AspNetCore.Mvc;
using Invoicing.Api.Services;
using Invoicing.Api.DTOs;

namespace Invoicing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _service;

        public InvoiceController(InvoiceService service)
        {
            _service = service;
        }

       
        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] AddInvoiceRequest request)
        {
            try
            {
                var id = await _service.AddInvoiceAsync(request);
                return Ok(new { InvoiceId = id, Message = "Invoice created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        
        [HttpPut("{invoiceId}")]
        public async Task<IActionResult> EditInvoice(int invoiceId, [FromBody] EditInvoiceRequest request)
        {
            try
            {
                await _service.EditInvoiceAsync(invoiceId, request);
                return Ok(new { Message = "Invoice updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        
        [HttpPut("{invoiceId}/void")]
        public async Task<IActionResult> VoidInvoice(int invoiceId)
        {
            try
            {
                await _service.VoidInvoiceAsync(invoiceId);
                return Ok(new { Message = "Invoice voided." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

      
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchInvoiceRequest req)
        {
            try
            {
                var result = await _service.SearchInvoicesAsync(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
