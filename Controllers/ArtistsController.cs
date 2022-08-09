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
    public class ArtistsController : Controller
    {
        private readonly CollectionModel _context;

        public ArtistsController(CollectionModel context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Artists.ToListAsync());
            }
            
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if(id == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                var artist = await _context.Artists
                .FirstOrDefaultAsync(m => m.Id == id);
                if(artist == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                return View(artist);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }


        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] Artist artist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(artist);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(artist);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                var artist = await _context.Artists.FindAsync(id);
                if (artist == null)
                {
                    return RedirectToAction("Index", "Error");
                }
                return View(artist);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] Artist artist)
        {
            try
            {

                if (id != artist.Id)
                {
                    return RedirectToAction("Index", "Error");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(artist);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArtistExists(artist.Id))
                        {
                            return RedirectToAction("Index", "Error");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(artist);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                var artist = await _context.Artists
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (artist == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                return View(artist);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }

        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var artist = await _context.Artists.FindAsync(id);

                using (CollectionModel db = new CollectionModel()) //Tar bort alla konstverk som är kopplade till konstnären om det finns några 
                {
                    var artworkFetch =
                       from arts in db.Artworks
                       where arts.ArtistId == id
                       select arts;


                    foreach (var selectedArtworks in artworkFetch)
                    {
                        db.Artworks.Remove(selectedArtworks);
                        db.SaveChanges();
                    }

                }

                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Error");
            }


        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }
    }
}
