using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ToDoListapi.Database;
using ToDoListapi.Models;

namespace ToDoListapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodolistController : ControllerBase
    {
        private readonly Todolist _toDoList;

        public TodolistController(Todolist toDoList)
        {
            this._toDoList = toDoList;
        }


        [HttpGet]
        [Route("getCategory")]
        public async Task<IActionResult> GetCategory()
        {

            
            var categories = await _toDoList.Category.ToListAsync();

           
            if (categories == null || !categories.Any())
            {
                return Ok(new List<object>());
            }

           
            var categoryWithCountList = new List<object>();

            foreach (var category in categories)
            {
             
                var count = await _toDoList.Todofulllist.CountAsync(todo => todo.CategoryId == category.CategoryId);

              
                categoryWithCountList.Add(new
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName, 
                    TodoCount = count
                });
            }

            return Ok(categoryWithCountList);
           


        }


        [HttpPost]
        [Route("addNewCategory")]
        public async Task<IActionResult> addNewCategory([FromBody] Category category)
        {
           
            if (category == null)
            {
                return BadRequest("Category name is required.");
            }
            await _toDoList.Category.AddAsync(category);
            await _toDoList.SaveChangesAsync();
            return Ok(category);

        }

        [HttpGet]
        [Route("getList")]
        public async Task<IActionResult> GetFullList(int categoryId)
        {
        
            var todoList = await _toDoList.Todofulllist
                .Where(e => e.CategoryId == categoryId)
                .OrderBy(e => e.Status)
                .ThenBy(e => e.ModifiedDate)
                .ToListAsync();

            var category = await _toDoList.Category.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            var catoryname = category.CategoryName;

            if (!todoList.Any())
            {
                //return NotFound("No todo items found for this category.");
                return Ok(new List<object>
                {
                    new { CategoryName = category.CategoryName }
                });
            }
            var todoListWithCategoryName = todoList.Select(todo => new
            {
                todo.ListId,
                todo.Name,
                todo.Status,
                todo.ModifiedDate,
                CategoryName = category.CategoryName // Include the CategoryName here
            }).ToList();

            return Ok(todoListWithCategoryName);
        }


        [HttpPost]
        [Route("addNewList")]
        public async Task<IActionResult> addNewList([FromBody] TodoFulllist _list)
        {
            if (_list == null)
            {
                return BadRequest("Description is required.");
            }
            //_list.ModifiedDate = DateTime.Now;
            await _toDoList.Todofulllist.AddAsync(_list);
            await _toDoList.SaveChangesAsync();
            return Ok(_list);

        }


        [HttpPut("{listId}")]
        public async Task<IActionResult> updateListStatus(int listId, [FromBody] TodoFulllist todoItem)
        {
            if (listId != todoItem.ListId)
            {
                return BadRequest("Todo item ID mismatch.");
            }

            var existingTodo = await _toDoList.Todofulllist.FindAsync(listId);
            if (existingTodo == null)
            {
                return NotFound("Todo item not found.");
            }
            existingTodo.Status = todoItem.Status;
           // existingTodo.ModifiedDate = DateTime.Now;
            try
            {
                await _toDoList.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(listId))
                {
                    return NotFound("Todo item not found.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();

        }

        private bool TodoItemExists(int id)
        {
            return _toDoList.Todofulllist.Any(e => e.ListId == id);
        }

        private bool TodoCategoryExists(int categoryId)
        {
            return _toDoList.Todofulllist.Any(e => e.CategoryId == categoryId);
        }


        [HttpDelete]
        [Route("deleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _toDoList.Category.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound($"Category with ID {categoryId} not found.");
            }

            // Check if there are any Todo items associated with this category
            var todoItems = await _toDoList.Todofulllist.Where(t => t.CategoryId == categoryId).ToListAsync();
            if (todoItems.Any())
            {
                return BadRequest($"Cannot delete category with ID {categoryId} because it has associated Todo items.");
            }

            _toDoList.Category.Remove(category);
            await _toDoList.SaveChangesAsync();
            return Ok(new { message = $"Category with ID {categoryId} has been deleted." });
            
        }

        [HttpDelete]
        [Route("deleteTodolists/{listId}")]
        public async Task<IActionResult> DeleteTodoItem(int listId)
        {
            var todoItem = await _toDoList.Todofulllist.FindAsync(listId);
            if (todoItem == null)
            {
                return NotFound($"Todo item with ID {listId} not found.");
            }

            _toDoList.Todofulllist.Remove(todoItem);
            await _toDoList.SaveChangesAsync();
            return Ok(new { message = $"Todo item with ID {listId} has been deleted." });
        }


    }





}
