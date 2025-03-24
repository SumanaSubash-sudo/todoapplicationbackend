using System.ComponentModel.DataAnnotations;

namespace ToDoListapi.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";

        public ICollection<Category> Categories { get; } = new List<Category>();
    }

    public class LoginModel
    {
       // public Guid Id { get; set; }
      //  public string FirstName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
       // public bool Result { get; set; }=false;
       // public string message { get; set; }="";
    }
}
