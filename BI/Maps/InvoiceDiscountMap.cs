using DTO.Common;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class InvoiceDiscountMap : ApiCommonData
    {

        public InvoiceDiscountSummaryItem MapInvoiceDiscountSummary(InvoiceDiscountSummaryRepoItem entity)
        {
            return new InvoiceDiscountSummaryItem
            {
                Id=entity.Id,
                Country=string.Join(", ",entity.Country),
                CreatedBy=entity.CreatedBy,
                CreatedOn=entity.CreatedOn.HasValue? entity.CreatedOn.Value.ToString(StandardDateFormat) : "",
                UpdatedBy=entity.UpdatedBy,
                UpdatedOn= entity.UpdatedOn.HasValue ? entity.UpdatedOn.Value.ToString(StandardDateFormat) : "",
                Customer=entity.CustomerName,
                DiscountType=entity.DiscountType,
                PeriodFrom=entity.PeriodFrom.HasValue ? entity.PeriodFrom.Value.ToString(StandardDateFormat) : "",
                PeriodTo = entity.PeriodTo.HasValue ? entity.PeriodTo.Value.ToString(StandardDateFormat) : "",
            };
        }

        public InvDisTranDetail MapInvoiceDiscountEntity(SaveInvoiceDiscount input, int userId,int? entityId)
        {
            return new InvDisTranDetail()
            {
                Active = true,
                SelectAllCountry = input.ApplyToNewCountry,
                CustomerId = input.CustomerId,
                DiscountType = input.DiscountType,
                PeriodTo = input.PeriodTo.ToNullableDateTime(),
                PeriodFrom = input.PeriodFrom.ToNullableDateTime(),
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                EntityId=entityId
            };
        }
        public void MapInvoiceDiscountEntity(InvDisTranDetail entity, SaveInvoiceDiscount input, int userId)
        {
            entity.Active = true;
            entity.SelectAllCountry = input.ApplyToNewCountry;
            entity.CustomerId = input.CustomerId;
            entity.DiscountType = input.DiscountType;
            entity.PeriodTo = input.PeriodTo.ToNullableDateTime();
            entity.PeriodFrom = input.PeriodFrom.ToNullableDateTime();
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
        }

        public void MapInvDisTranPeriod(InvDisTranPeriodInfo entity, InvoiceDiscountLimit input)
        {
            entity.Active = true;
            entity.LimitFrom = input.LimitFrom;
            entity.LimitTo = input.LimitTo;
            entity.NotificationSent = input.Notification;
        }

        public InvDisTranCountry MapInvoiceDiscountCountry(int id, int userId)
        {
            return new InvDisTranCountry()
            {
                CountryId = id,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            };
        }

        public InvDisTranPeriodInfo MapInvoiceDiscountPeriod(InvoiceDiscountLimit input, int userId)
        {
            return new InvDisTranPeriodInfo()
            {
                Active = true,
                LimitFrom = input.LimitFrom,
                LimitTo = input.LimitTo,
                NotificationSent = input.Notification,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            };
        }

        public SaveInvoiceDiscount EditInvoiceDiscountMap(InvDisTranDetail entity)
        {
            return new SaveInvoiceDiscount()
            {
                ApplyToNewCountry = entity.SelectAllCountry,
                CustomerId = entity.CustomerId,
                DiscountType = entity.DiscountType,
                Id = entity.Id,
                PeriodFrom = entity.PeriodFrom.GetCustomDate(),
                PeriodTo = entity.PeriodTo.GetCustomDate(),
            };
        }

        public InvoiceDiscountLimit EditInvoiceDiscountPeriodMap(InvDisTranPeriodInfo entity)
        {
            return new InvoiceDiscountLimit()
            {
                Id = entity.Id,
                LimitFrom = entity.LimitFrom,
                LimitTo = entity.LimitTo,
                Notification = entity.NotificationSent
            };
        }


    }
}
