using InquiryAPI.DomainObjects.Inquiries;
using InquiryAPI.Services.InquiriesService;
using InquiryAPI.Services.UserTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InquiryAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class InquiriesController : ControllerBase
    {
        private readonly IInquiriesService _inquiriesService;
        private readonly IUserTokenService _userTokenService;
        public InquiriesController(IInquiriesService inquiriesService, IUserTokenService userTokenService)
        {
            _inquiriesService = inquiriesService;
            _userTokenService = userTokenService;
        }

        public class InquirySaveDTO
        {
            public Guid? Id { get; set; }
            public InquiryType Type { get; set; }
            public string Message { get; set; }
        }

        public class InquiryDTO
        {
            public Guid Id { get; set; }
            public string Message { get; set; }
            public InquiryType Type { get; set; }
            public InquiryStatus Status { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }

        [HttpPost]
        public IActionResult CreateNew([FromBody] InquirySaveDTO inquirySaveDTO)
        {
            Inquiry inquiry = new(inquirySaveDTO.Type, inquirySaveDTO.Message, _userTokenService.UserId);
            inquiry = _inquiriesService.Save(inquiry);

            InquiryDTO inquiryDTO = new()
            {
                Id = inquiry.Id,
                Type = inquiry.Type,
                Message = inquiry.Message,
                Status = inquiry.Status,
                CreationDate = inquiry.CreationDate,
            };

            return Ok(inquiryDTO);
        }


        public class InquiryListItemDTO
        {
            public Guid Id { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public InquiryStatus Status { get; set; }
            public InquiryType Type { get; set; }
        }

        [HttpGet]
        public ActionResult<List<InquiryListItemDTO>> List()
        {
            List<InquiryListItemDTO> list = _inquiriesService
                .Search(new InquirySearchPredicate() { UserId = _userTokenService.UserId })
                .OrderByDescending(q => q.CreationDate)
                .Select(q => new InquiryListItemDTO()
                {
                    Id = q.Id,
                    CreationDate = q.CreationDate,
                    Status = q.Status,
                    Type = q.Type,
                })
                .ToList();

            return Ok(list);
        }

        public class EnumDto
        {
            public int Value { get; set; }
            public string Label { get; set; }
        }


        [HttpGet("inquiry-types")]
        public ActionResult<IEnumerable<EnumDto>> GetInquiryTypes()
        {
            List<EnumDto> types = Enum.GetValues(typeof(InquiryType))
                .Cast<InquiryType>()
                .Select(e => new EnumDto { Value = (int)e, Label = e.ToString() })
                .ToList();

            return Ok(types);
        }

        [HttpGet("inquiry-statuses")]
        public ActionResult<IEnumerable<EnumDto>> GetInquiryStatuses()
        {
            List<EnumDto> statuses = Enum.GetValues(typeof(InquiryStatus))
                .Cast<InquiryStatus>()
                .Select(e => new EnumDto { Value = (int)e, Label = e.ToString() })
                .ToList();

            return Ok(statuses);
        }
    }
}