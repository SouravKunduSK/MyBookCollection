﻿using Microsoft.AspNetCore.Mvc;
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
            catch (Exception ex)
            {
                ViewBag.Error = "Something went wrong! Try again.";
                return View();
            }
        }

       /* public async Task<IActionResult> Create()
        {
            return View();
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewAuthor(Author author)
        {
            if (author == null || !ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Something went Wrong! Try again.";
                return RedirectToAction("AuthorList");
            }
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingAuthor = await _authorService.GetAllAuthorsAsync(userId);
                if (!existingAuthor.Any(x=>x.Name == author.Name)) 
                {
                    await _authorService.AddAuthorAsync(author, userId);
                    ViewBag.Message = "Author name is added successfully!";
                    return RedirectToAction("AuthorList");
                }
                ViewBag.ErrorMessage = "Name is already added! Try another.";
            }
            return RedirectToAction("AuthorList");
        }


       /* public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id, userId);
            if (author == null) return NotFound();
            return View(author);
        }*/

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
                    ViewBag.Message = "Author name is updated successfully!";
                    return RedirectToAction("AuthorList");
                }
                ViewBag.ErrorMessage = "Name is already added! Try another.";
                return RedirectToAction("AuthorList");
            }
            ViewBag.ErrorMessage = "Something went Wrong! Try again.";
            return RedirectToAction("AuthorList");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _authorService.DeleteAuthorAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null) return NotFound();
            return View(author);
        }

        public async Task<IActionResult> AddOrEditPartial(int? id)
        {
            if (id == null)
            {
                // Add new author (empty form)
                return PartialView("_AddOrEdit", new AuthorVM());
            }
            else
            {
                // Edit existing author
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var author = await _authorService.GetAuthorByIdAsync(id.Value);
                if (author == null)
                {
                    return NotFound();
                }

                var authorVm = new AuthorVM
                {
                    Id = author.Id,
                    Name = author.Name
                };
                return PartialView("_AddOrEdit", authorVm);
            }
        }
        public async Task<IActionResult> DeletePartial(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return PartialView("_Delete", author);
        }


    }
}
