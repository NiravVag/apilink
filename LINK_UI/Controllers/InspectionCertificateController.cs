using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.InspectionCertificate;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspectionCertificateController : ControllerBase
    {
        private readonly IInspectionCertificateManager _icManager = null;
        private readonly IInspectionBookingManager _inspManager = null;

        public InspectionCertificateController(IInspectionCertificateManager icManager,
            IInspectionBookingManager inspManager)
        {
            _icManager = icManager;
            _inspManager = inspManager;
        }

        [HttpPost("bookingSearch")]
        [Right("inspection-certificate")]
        public async Task<ICBookingResponse> SearchIC(ICBookingSearchRequest request)
        {
            if (request != null)
                return await _icManager.GetInspectionICData(request);
            else
                return new ICBookingResponse() { Result = ICBookingSearchResult.RequestNotCorrectFormat };

        }
        [HttpPost("save")]
        [Right("inspection-certificate")]
        public async Task<InspectionCertificateResponse> Save(InspectionCertificateRequest request)
        {
            if (request != null)
                return await _icManager.SaveIC(request);
            else
                return new InspectionCertificateResponse() { Result = InspectionCertificateResult.RequestNotCorrectFormat };
        }
        [HttpGet("edit/{id}")]
        [Right("inspection-certificate")]
        public async Task<EditInspectionCertificateResponse> Edit(int id)
        {
            if (id > 0)
                return await _icManager.EditICDetails(id);
            else
                return new EditInspectionCertificateResponse() { Result = InspectionCertificateResult.RequestNotCorrectFormat };
        }
        [HttpGet("cancel/{id}")]
        [Right("inspection-certificate")]
        public async Task<InspectionCertificateResponse> Cancel(int id)
        {
            if (id > 0)
                return await _icManager.CancelICDetails(id);
            else
                return new InspectionCertificateResponse() { Result = InspectionCertificateResult.RequestNotCorrectFormat };
        }

        [HttpGet("preview/{id}/{isDraft}")]
        [Right("inspection-certificate")]
        public async Task<IActionResult> Preview(int id, bool isDraft, [FromServices] IPDF previewService)
        {
            if (id > 0)
            {
                var icDetails = await _icManager.GetICPreviewDetails(id, isDraft);

                if (icDetails != null)
                {
                    var document = previewService.CreateICDocument(icDetails);
                    return File(document.Content, document.MimeType);
                }
            }
            return NotFound();
        }


        [HttpGet("icproducts/{id}")]
        [Right("inspection-certificate")]
        public List<ICSummaryProducts> GetICSummaryProducts(int id)
        {
            return _icManager.GetICSummaryProducts(id);
        }

        [HttpPost("icsummarysearch")]
        [Right("inspection-certificate")]
        public async Task<ICSummarySearchResponse> SearchICData(ICSummarySearchRequest request)
        {
            return await _icManager.GetICSummaryDetails(request);
        }

        [HttpGet("icstatuslist")]
        [Right("inspection-certificate")]
        public ICStatusResponse GetICStatusList()
        {
            return _icManager.GetICStatus();
        }
        [HttpGet("icTitleList")]
        [Right("inspection-certificate")]
        public async Task<ICTitleResponse> ICTitleList()
        {
            return await _icManager.GetICTitleList();
        }
        [HttpPost("bookingICProduct")]
        [Right("inspection-certificate")]
        public async Task<ICBookingProductResponse> BookingICProduct(ICBookingProductRequest request)
        {
            if (request != null)
                return await _icManager.BookingICProduct(request);
            else
                return new ICBookingProductResponse() { Result = ICBookingSearchResult.RequestNotCorrectFormat };

        }      
    }
}