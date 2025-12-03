using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restoran.web.Data;
using Restoran.web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restoran.web.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly RestoranContext _context;
        public RezervacijaController(RestoranContext context) => _context = context;

        // READ
        public async Task<IActionResult> Index() =>
            View(await _context.Rezervacije
                .Include(r => r.Gost)
                .Include(r => r.Sto)
                .ToListAsync());

        // CREATE
        public async Task<IActionResult> Create()
        {
            await PopuniStoloveCreate(null);   // MORA DA POSTOJI
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime Datum, TimeSpan Vreme,
                                                int BrojOsoba, int IDStola,
                                                string ImeGosta, string PrezimeGosta)
        {
            bool zauzet = await _context.Rezervacije
                .AnyAsync(r => r.IDStola == IDStola && r.Datum == Datum);

            if (zauzet)
            {
                var brojStola = await _context.Stolovi
                    .Where(s => s.IDStola == IDStola)
                    .Select(s => s.BrojStola)
                    .FirstOrDefaultAsync();

                ModelState.AddModelError("", $"Sto br. {brojStola} je već rezervisan za {Datum:d}.");
                await PopuniStoloveCreate(Datum);
                return View();
            }

            var gost = await _context.Gosti
                .FirstOrDefaultAsync(g => g.ImeGosta == ImeGosta &&
                                          g.PrezimeGosta == PrezimeGosta);

            if (gost == null)
            {
                gost = new Gost { ImeGosta = ImeGosta, PrezimeGosta = PrezimeGosta };
                _context.Gosti.Add(gost);
                await _context.SaveChangesAsync();
            }

            var rez = new Rezervacija
            {
                Datum = Datum,
                Vreme = Vreme,
                BrojOsoba = BrojOsoba,
                IDStola = IDStola,
                IDGosta = gost.IDGosta
            };

            _context.Rezervacije.Add(rez);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var rez = await _context.Rezervacije.FindAsync(id);
            if (rez == null) return NotFound();

            ViewBag.Gosti = new SelectList(await _context.Gosti.ToListAsync(),
                                           "IDGosta", "ImeGosta", rez.IDGosta);
            ViewBag.Stolovi = await PopuniStoloveEdit(rez.Datum, rez.IDStola); // MORA DA POSTOJI
            return View(rez);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime Datum, TimeSpan Vreme,
                                              int BrojOsoba, int IDStola,
                                              string ImeGosta, string PrezimeGosta)
        {
            var rez = await _context.Rezervacije.FindAsync(id);
            if (rez == null) return NotFound();

            bool zauzet = await _context.Rezervacije
                .AnyAsync(r => r.IDStola == IDStola &&
                               r.Datum == Datum &&
                               r.IDRezervacije != id);

            if (zauzet)
            {
                var brojStola = await _context.Stolovi
                    .Where(s => s.IDStola == IDStola)
                    .Select(s => s.BrojStola)
                    .FirstOrDefaultAsync();

                ModelState.AddModelError("", $"Sto br. {brojStola} je već rezervisan za {Datum:d}.");
                ViewBag.Gosti = new SelectList(await _context.Gosti.ToListAsync(),
                                               "IDGosta", "ImeGosta", rez.IDGosta);
                ViewBag.Stolovi = await PopuniStoloveEdit(Datum, IDStola);
                return View(rez);
            }

            var gost = await _context.Gosti
                .FirstOrDefaultAsync(g => g.ImeGosta == ImeGosta &&
                                          g.PrezimeGosta == PrezimeGosta);

            if (gost == null)
            {
                gost = new Gost { ImeGosta = ImeGosta, PrezimeGosta = PrezimeGosta };
                _context.Gosti.Add(gost);
                await _context.SaveChangesAsync();
            }

            rez.Datum = Datum;
            rez.Vreme = Vreme;
            rez.BrojOsoba = BrojOsoba;
            rez.IDStola = IDStola;
            rez.IDGosta = gost.IDGosta;

            _context.Update(rez);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var rez = await _context.Rezervacije
                .Include(r => r.Gost)
                .Include(r => r.Sto)
                .FirstOrDefaultAsync(m => m.IDRezervacije == id);
            return rez == null ? NotFound() : View(rez);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rez = await _context.Rezervacije.FindAsync(id);
            if (rez != null) _context.Rezervacije.Remove(rez);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // AJAX: osveži stolove (za Create)
        public async Task<IActionResult> OsveziStolove(DateTime datum)
        {
            await PopuniStoloveCreate(datum);
            return PartialView("_StoloviPartial", ViewBag.Stolovi);
        }

        // AJAX: osveži stolove (za Edit)
        public async Task<IActionResult> OsveziStoloveEdit(DateTime datum, int id)
        {
            ViewBag.Stolovi = await PopuniStoloveEdit(datum, id);
            return PartialView("_StoloviPartialEdit", ViewBag.Stolovi);
        }

        // pomoćna za CREATE (anonimni objekti)
        private async Task PopuniStoloveCreate(DateTime? zaDatum)
        {
            var stolovi = await _context.Stolovi
                .Select(s => new
                {
                    s.IDStola,
                    Broj = s.BrojStola,
                    Zauzet = zaDatum.HasValue &&
                              _context.Rezervacije
                                  .Any(r => r.IDStola == s.IDStola &&
                                            r.Datum == zaDatum.Value)
                })
                .ToListAsync();

            ViewBag.Stolovi = stolovi;
        }

        // pomoćna za EDIT (SelectListItem + Disabled)
        private async Task<List<SelectListItem>> PopuniStoloveEdit(DateTime zaDatum, int izabraniSto)
        {
            var stolovi = await _context.Stolovi
                .Select(s => new
                {
                    s.IDStola,
                    Broj = s.BrojStola,
                    Zauzet = _context.Rezervacije
                                .Any(r => r.IDStola == s.IDStola &&
                                          r.Datum == zaDatum &&
                                          r.IDRezervacije != izabraniSto)
                })
                .ToListAsync();

            return stolovi.Select(x => new SelectListItem
            {
                Value = x.IDStola.ToString(),
                Text = $"Sto {x.Broj}",
                Selected = x.IDStola == izabraniSto,
                Disabled = x.Zauzet   // onemogući crvene
            }).ToList();
        }

        private bool RezervacijaExists(int id)
            => _context.Rezervacije.Any(e => e.IDRezervacije == id);
    }
}