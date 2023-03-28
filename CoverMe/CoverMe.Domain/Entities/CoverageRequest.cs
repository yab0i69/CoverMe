using System.ComponentModel.DataAnnotations;

namespace CoverMe.Domain.Entities
{
    public class CoverageRequest
    {
        [Key]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Date { get; set; }
        public bool Approval { get; set; }
    }
}
