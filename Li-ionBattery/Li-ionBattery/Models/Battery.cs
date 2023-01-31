using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace Li_ionBattery.Models
{
    public class Battery
    {
        [Key]
        public int ID { get; set; }
        public string? BatteryId { get; set; }
        public decimal? Resistance1 { get; set; }
        public decimal? Voltage1 { get; set; }
        public decimal? Temperature1 { get; set; }
        public string? DateTime1 { get; set; }
        public decimal? Resistance2 { get; set; }
        public decimal? Voltage2 { get; set; }
        public decimal? Temperature2 { get; set; }
        public string? DateTime2 { get; set; }
        public DateTime? UplodedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
