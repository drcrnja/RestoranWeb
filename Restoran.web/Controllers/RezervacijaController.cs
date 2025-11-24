using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restoran.web.Data;
using Restoran.web.Models;

namespace Restoran.web.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly RestoranContext _context;

        public RezervacijaController(RestoranContext context)
        {
            _context = context;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var restoranContext = _context.Rezervacije
                .Include(r => r.Gost)
                .Include(r => r.Sto);
            return View(await restoranContext.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Gost)
                .Include(r => r.Sto)
                .FirstOrDefaultAsync(m => m.IDRezervacije == id);

            if (rezervacija == null) return NotFound();

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public IActionResult Create()
        {
            ViewBag.Stolovi = new SelectList(_context.Stolovi, "IDStola", "BrojStola");
            return View();
        }

        // POST: Rezervacija/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IDRezervacije,Datum,Vreme,BrojOsoba,IDStola")] Rezervacija rezervacija,
            string ImeGosta,
            string PrezimeGosta)
        {
            if (ModelState.IsValid)
            {
                // pronađi ili dodaj gosta
                var gost = await _context.Gosti
                    .FirstOrDefaultAsync(g => g.ImeGosta == ImeGosta && g.PrezimeGosta == PrezimeGosta);

                if (gost == null)
                {
                    gost = new Gost { ImeGosta = ImeGosta, PrezimeGosta = PrezimeGosta };
                    _context.Gosti.Add(gost);
                    await _context.SaveChangesAsync();
                }

                rezervacija.IDGosta = gost.IDGosta;

                // generiši novi ID
                rezervacija.IDRezervacije = _context.Rezervacije.Any() ? _context.Rezervacije.Max(r => r.IDRezervacije) + 1 : 1;

                _context.Rezervacije.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ponovo popuni padajuću listu ako model nije validan
            ViewBag.Stolovi = new SelectList(_context.Stolovi, "IDStola", "BrojStola");
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null) return NotFound();

            ViewBag.Stolovi = new SelectList(_context.Stolovi, "IDStola", "BrojStola", rezervacija.IDStola);
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("IDRezervacije,Datum,Vreme,BrojOsoba,IDGosta,IDStola")] Rezervacija rezervacija)
        {
            if (id != rezervacija.IDRezervacije) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Rezervacije.Any(e => e.IDRezervacije == rezervacija.IDRezervacije))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stolovi = new SelectList(_context.Stolovi, "IDStola", "BrojStola", rezervacija.IDStola);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Gost)
                .Include(r => r.Sto)
                .FirstOrDefaultAsync(m => m.IDRezervacije == id);

            if (rezervacija == null) return NotFound();

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}