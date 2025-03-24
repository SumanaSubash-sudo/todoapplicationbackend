using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListapi.Models
{
    public class TodoFulllist
    {

        [Key]
        public int ListId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = "";
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "";
      
        public int CategoryId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
       

    }
}
