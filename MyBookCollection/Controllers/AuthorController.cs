using Microsoft.AspNetCore.Mvc;
using MyBookCollection.Models.Data;
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
            catch (Exception ex)
            {
                ViewBag.Error = "Something went wrong! Try again.";
                return View();
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _authorService.AddAuthorAsync(author, userId);
                ViewBag.Message = "Author name is added successfully!";
                return RedirectToAction(nameof(AuthorList));
            }
            return View("AuthorList");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id, userId);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _authorService.UpdateAuthorAsync(author, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id, userId);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _authorService.DeleteAuthorAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id, userId);
            if (author == null) return NotFound();
            return View(author);
        }

    }
}
