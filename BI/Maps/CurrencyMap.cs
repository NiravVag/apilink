using DTO.Common;
using DTO.ExchangeRate;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps
{
    public class CurrencyMap : ApiCommonData
    {
        public ExchangeRateType GetRateType(EmExchangeRateType entity)
        {
            if (entity == null)
                return null;

            return new ExchangeRateType
            {
                Id = entity.Id,
                Label = entity.TypeTransId.GetTranslation(entity.Label)
            };
        }
    }
}
