using API.Data.Entity;
using System.ComponentModel.DataAnnotations;
using API.Data.Entity.MasterData;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entity.Master
{
    [Table("PostOffice", Schema = "MasterData")]
    public class PostOffice : Base
    {
        public int? districtId { get; set; }
        public District district { get; set; }

        [MaxLength(10)]
        public string postalCode { get; set; }
        [MaxLength(100)]
        public string postalName { get; set; }
        [MaxLength(20)]
        public string postalShortName { get; set; }
        [MaxLength(100)]
        public string postalNameBn { get; set; }

        
    }
}
