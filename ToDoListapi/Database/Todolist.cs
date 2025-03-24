using Microsoft.EntityFrameworkCore;
using ToDoListapi.Models;

namespace ToDoListapi.Database
{
    public class Todolist : DbContext
    {

        public Todolist(DbContextOptions<Todolist> options) : base(options)
        {
        }

        public required DbSet<User> User { get; set; }

        public required DbSet<Category> Category { get; set; }

        public required DbSet<TodoFulllist> Todofulllist { get; set; }


    }
}