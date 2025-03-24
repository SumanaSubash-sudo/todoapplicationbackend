using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListapi.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }="";

        public Guid? UserId { get; set; }

        public ICollection<TodoFulllist> Todofulllist { get; } = new List<TodoFulllist>();

    }


}
