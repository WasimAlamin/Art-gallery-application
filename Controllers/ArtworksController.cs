using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaborationRep.Models;

namespace LaborationRep.Controllers
{
    public class ArtworksController : Controller
    {
        private readonly CollectionModel _context;

        public ArtworksController(CollectionModel context)
        {
            _context = context;
        }

        // GET: Artworks Visar alla konstverk som tillhör en viss konstnär
        public IActionResult Index(int? id)
        {
            try
            {
                using (CollectionModel db = new CollectionModel())
                {
                    Artist artist = db.Artists.Find(id);
                    artist.Art = (List<Artwork>)db.Artworks.Where(c => c.ArtistId == artist.Id).ToList();

                    return View(artist);
                }

            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); 
                return RedirectToAction("Index", "Error");
            }
           
          
        }

        // GET: Artworks/Detaljer/skickar med ID för valt konstverk
        public async Task<IActionResult> Details(int? id)
        {
            try
            { 
                if(id == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                var artwork = await _context.Artworks
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);

                if(artwork == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                return View(artwork);
            }
           catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error"); 
            }
            
        }

        // GET: Artworks/Create
        public IActionResult Create()
        {
          
                ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "LastName");
                return View();
            

        }

        // POST: Artworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtworkName,Year,Genre,Technique,Room,Location,Exhibition,ArtistId")] Artwork artwork)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(artwork);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = artwork.ArtistId }); //skickar med ArtistId för att konstverken ska visas, som tillhör den specifika konstnären, i Index
                }
                ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "LastName", artwork.ArtistId);
                return View(artwork);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }


        }

        // GET: Artworks/Edit/5 Uppdaterar ett valt konstverk
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if(id == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                var artwork = await _context.Artworks.FindAsync(id);
                if(artwork == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "LastName", artwork.ArtistId);
                return View(artwork);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        // POST: Artworks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtworkName,Year,Genre,Technique,Room,Location,Exhibition,Status,Period,ArtistId")] Artwork artwork)
        {
            try
            {

                if (id != artwork.Id)
                {
                    return RedirectToAction("Index", "Error");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(artwork);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArtworkExists(artwork.Id))
                        {
                            return RedirectToAction("Index", "Error");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", new { id = artwork.ArtistId });
                }
                ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "LastName", artwork.ArtistId);
                return View(artwork);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        // GET: Artworks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if(id == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                var artwork = await _context.Artworks
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
                if(artwork == null)
                {
                    return RedirectToAction("Index", "Error");
                }
                return View(artwork);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        // POST: Artworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var artwork = await _context.Artworks.FindAsync(id);
                _context.Artworks.Remove(artwork); 
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = artwork.ArtistId });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        private bool ArtworkExists(int id)
        {
            return _context.Artworks.Any(e => e.Id == id);
        }
    }
}
