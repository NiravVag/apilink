using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.CancelBooking;
using DTO.Common;
using DTO.Eaqf;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI
{
    public class EaqfEventUpdateManager : ApiCommonData, IEaqfEventUpdateManager
    {
        private readonly EAQFSettings _eaqfSettings = null;
        private readonly IHelper _helper = null;
        private readonly ICancelBookingRepository _cancelBookingRepository = null;
        private readonly BookingMap _bookingmap = null;
        public EaqfEventUpdateManager(IOptions<EAQFSettings> eaqfSettings, IHelper helper,
            ICancelBookingRepository cancelBookingRepository)
        {
            _eaqfSettings = eaqfSettings.Value;
            _helper = helper;
            _cancelBookingRepository = cancelBookingRepository;
            _bookingmap = new BookingMap();
        }

        public async Task<bool> UpdateRescheduleStatusToEAQF(EAQFEventUpdate request, EAQFBookingEventRequestType eAQFBookingEvent)
        {

            string token = "";
            var baseUrl = _eaqfSettings.BaseUrl;
            var oauthRequestUrl = _eaqfSettings.OAuthRequestUrl;

            var oAuthRequestObject = new EAQFOauthTokenRequest()
            {
                client_id = _eaqfSettings.ClientId,
                client_secret = _eaqfSettings.SecretKey,
                grant_type = _eaqfSettings.GrantType
            };

            try
            {
                HttpResponseMessage tokenResponse = await _helper.SendRequestToPartnerAPI(Utilities.Method.PostForm, oauthRequestUrl, oAuthRequestObject, baseUrl, "");

                if (tokenResponse.StatusCode == HttpStatusCode.OK)
                {
                    var tokenInfo = tokenResponse.Content.ReadAsStringAsync();
                    JObject tokenInfoJson = JObject.Parse(tokenInfo.Result);
                    if (tokenInfoJson != null && tokenInfoJson.GetValue("access_token") != null)
                    {
                        token = tokenInfoJson.GetValue("access_token").ToString();
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        var rescheduleUpdateRequestUrl = _eaqfSettings.BookingEventRequestUrl;

                        object bookingEventUpdate = null;

                        if (eAQFBookingEvent == EAQFBookingEventRequestType.DateChange)
                        {
                            var serviceFromDate = request.ServiceFromDate.ToNullableDateTime();
                            var fromDate = serviceFromDate.HasValue ? serviceFromDate.Value.ToString(StandardDateFormat) : string.Empty;

                            var serviceToDate = request.ServiceToDate.ToNullableDateTime();
                            var toDate = serviceToDate.HasValue ? serviceToDate.Value.ToString(StandardDateFormat) : string.Empty;

                            //Fetch the REschedule reasons for the booking
                            var reasonList = await GetRescheduleReason(request.BookingId);
                            //Filter the reasons to fetch the current reason
                            var reason = reasonList.ResponseList.Where(x => x.Id == request.ReasonTypeId).Select(x => x.Reason).FirstOrDefault();
                            bookingEventUpdate = new EAQFRescheduleEventUpdate()
                            {
                                ReasonChange = reason,
                                ServiceDateFrom = fromDate,
                                ServiceDateTo = toDate,
                                Classification = "DateChange"
                            };
                        }
                        else if (eAQFBookingEvent == EAQFBookingEventRequestType.AddStatus)
                        {
                            bookingEventUpdate = new EAQFBookingEventUpdate()
                            {
                                Classification = "AddStatus",
                                StatusDate = DateTime.Now.ToString(),
                                StatusId = request.StatusId
                            };
                        }
                        else if (eAQFBookingEvent == EAQFBookingEventRequestType.CancelStatus)
                        {
                            var reason = await _cancelBookingRepository.GetBookingCancelReasonsById(request.ReasonTypeId);

                            bookingEventUpdate = new EAQFBookingEventCancelUpdate()
                            {
                                Classification = "Cancel",
                                Reason = reason?.Reason
                            };
                        }

                        rescheduleUpdateRequestUrl = string.Format(rescheduleUpdateRequestUrl, request.BookingId);

                        HttpResponseMessage rescheduleUpdateResponse = await _helper.SendRequestToPartnerAPI(Utilities.Method.Post, rescheduleUpdateRequestUrl, bookingEventUpdate, baseUrl, token);
                        if (rescheduleUpdateResponse.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<BookingCancelRescheduleResponse> GetRescheduleReason(int bookingId)
        {
            try
            {
                var response = new BookingCancelRescheduleResponse();
                var getBookingDetail = await _cancelBookingRepository.GetCancelBookingDetails(bookingId);
                if (getBookingDetail == null)
                    return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.CannotGetBookingDetail };
                var data = await _cancelBookingRepository.GetBookingRescheduleReasons(getBookingDetail.CustomerId);
                if (data == null || data.Count == 0)
                    return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.NotFound };
                response.ResponseList = data.Select(_bookingmap.GetBookingRescheduleReasons).ToArray();
                response.Result = BookingCancelRescheduleReasonsResult.Success;
                return response;
            }
            catch (Exception)
            {
                return new BookingCancelRescheduleResponse() { Result = BookingCancelRescheduleReasonsResult.NotFound };
            }
        }
    }
}
