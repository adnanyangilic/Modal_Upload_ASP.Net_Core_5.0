using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modal_Upload_ASP.Net_Core_5._0.Data;
using Modal_Upload_ASP.Net_Core_5._0.Models;
using static Modal_Upload_ASP.Net_Core_5._0.Helper;

namespace Modal_Upload_ASP.Net_Core_5._0.Controllers
{
    public class BlogAuthorsController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogAuthorsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: BlogAuthors
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogAuthors.ToListAsync());
        }


        [NoDirectAccess]
        public async Task<IActionResult> Create_Update(int id = 0)
        {
            if (id == 0)
                return View(new BlogAuthor());
            else
            {
                var blogAuthor = await _context.BlogAuthors.FindAsync(id);
                if (blogAuthor == null)
                {
                    return NotFound();
                }
                return View(blogAuthor);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_Update( IFormFile resim, [Bind("BlogAuthorId, BlogAuthorName")] BlogAuthor blogAuthor)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (blogAuthor.BlogAuthorId == 0)
                {
                    var filePath = Path.Combine(_env.WebRootPath, Path.GetFileName(resim.FileName));
                    resim.CopyTo(new FileStream(filePath, FileMode.Create));
                    ViewData["myfileLocation"] = Path.GetFileName(resim.FileName);
                    blogAuthor.Imagepath = ViewData["myfileLocation"].ToString();

                    _context.Add(blogAuthor);
                    await _context.SaveChangesAsync();



                }
                //Update
                else
                {
                    try
                    {

                        if (resim != null)
                        {


                            var filePath = Path.Combine(_env.WebRootPath, Path.GetFileName(resim.FileName));

                            resim.CopyTo(new FileStream(filePath, FileMode.Create));
                            ViewData["myfileLocation"] = Path.GetFileName(resim.FileName);
                            blogAuthor.Imagepath = ViewData["myfileLocation"].ToString();

                            _context.Update(blogAuthor);
                            await _context.SaveChangesAsync();




                        }
                        else
                        {
                            _context.BlogAuthors.Attach(blogAuthor);
                            _context.Entry(blogAuthor).State = EntityState.Modified;
                            _context.Entry(blogAuthor).Property(x => x.Imagepath).IsModified = false;
                            _context.SaveChanges();

                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BlogAuthorExists(blogAuthor.BlogAuthorId))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.BlogAuthors.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", blogAuthor) });
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogAuthor = await _context.BlogAuthors
                .FirstOrDefaultAsync(m => m.BlogAuthorId == id);
            if (blogAuthor == null)
            {
                return NotFound();
            }

            return View(blogAuthor);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var blogAuthor = await _context.BlogAuthors.FindAsync(id);


            var filePathToDel = Path.Combine(_env.WebRootPath, "/", blogAuthor.Imagepath);

            _context.BlogAuthors.Remove(blogAuthor);
            await _context.SaveChangesAsync();



            if (System.IO.File.Exists(filePathToDel))
                System.IO.File.Delete(filePathToDel);



            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.BlogAuthors.ToList()) });
        }

        private bool BlogAuthorExists(int id)
        {
            return _context.BlogAuthors.Any(e => e.BlogAuthorId == id);
        }


    }
}
