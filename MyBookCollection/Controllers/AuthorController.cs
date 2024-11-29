using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookCollection.Models.Data;
using MyBookCollection.Models.ViewModels;
using MyBookCollection.Services;
using System.Security.Claims;

namespace MyBookCollection.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async  Task<IActionResult> AuthorList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                
                var authors = await _authorService.GetAllAuthorsAsync(userId);
                return View(authors);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong! Try again.";
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewAuthor(Author author)
        {
            if (author == null || !ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Something went Wrong! Try again.";
                return RedirectToAction("AuthorList");
            }
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingAuthor = await _authorService.GetAllAuthorsAsync(userId);
                if (!existingAuthor.Any(x=>x.Name == author.Name)) 
                {
                    await _authorService.AddAuthorAsync(author, userId);
                    TempData["SuccessMessage"] = "Author name is added successfully!";
                    return RedirectToAction("AuthorList");
                }
                TempData["ErrorMessage"] = "Name is already added! Try another.";
            }
            return RedirectToAction("AuthorList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuthor(int id, Author author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingAuthor = await _authorService.GetAuthorByIdAsync(id);
                var duplicateAuthor = await _authorService.GetAllAuthorsAsync(userId);
                var result = duplicateAuthor.SkipWhile(x => x.Id == id).Any(a=>a.Name == author.Name);
                if (existingAuthor != null && !result)
                {
                    existingAuthor.Name = author.Name;
                    await _authorService.UpdateAuthorAsync(existingAuthor);
                    TempData["SuccessMessage"] = "Author name is updated successfully!";
                    return RedirectToAction("AuthorList");
                }
                TempData["ErrorMessage"] = "Name is already added! Try another.";
                return RedirectToAction("AuthorList");
            }
            TempData["ErrorMessage"] = "Something went Wrong! Try again.";
            return RedirectToAction("AuthorList");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            try
            {
                await _authorService.DeleteAuthorAsync(id);
                TempData["SuccessMessage"] = "Author name is deleted successfully!";
                return RedirectToAction(nameof(AuthorList));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went Wrong! Try again.";
                return RedirectToAction("AuthorList");
            } 
        }
    }
}
