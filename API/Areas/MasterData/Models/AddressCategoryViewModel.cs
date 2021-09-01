using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Entity.MasterData;

namespace API.Areas.MasterData.Models
{
    public class AddressCategoryViewModel
    {
        public int Id { get; set; }

        public string name { get; set; }

        public int discountId { get; set; }
        public decimal? rate { get; set; }

        public IEnumerable<AddressCategory> addressCategories { get; set; }
        //public IEnumerable<DiscountRate> discountRates { get; set; }
    }
}
