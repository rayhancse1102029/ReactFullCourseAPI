using API.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entity.MasterData
{
    [Table("PaymentMode", Schema = "MasterData")]
    public class PaymentMode : Base
    {
       public string paymentModeName { get; set; }
    }
}
