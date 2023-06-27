using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class SplitBooking
    {
        public int BookingId { get; set; }
        public bool IsEmailRequired { get; set; }
        public string SplitBookingComments { get; set; }
        public SaveInsepectionRequest BookingData { get; set; }

		public IEnumerable<ProductInfo> SplitBookingProductList { get; set; }
    }

	public class ProductInfo
	{
		public int PoDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
		public string ProductDescription { get; set; }
		public string PoName { get; set; }
		public string Aql { get; set; }
		public int BookingQuantity { get; set; }
		public string UnitName { get; set; }
		public bool Selected { get; set; }
		public string ColorCode { get; set; }
		public string ColorName { get; set; }
	}
}
