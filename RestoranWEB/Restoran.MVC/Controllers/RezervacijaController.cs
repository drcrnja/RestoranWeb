using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restoran.BLL.DTOs;
using Restoran.BLL.Services;
using Restoran.DAL.UnitOfWork;

namespace Restoran.MVC.Controllers
{
    
    public class RezervacijaController : Controller
    {
        private readonly IRezervacijaService _service;
        private readonly IUnitOfWork _uow;
        
        public RezervacijaController(IRezervacijaService service, IUnitOfWork uow)
        {
            
            _service = service;
            _uow = uow;
        }
        //prikaz rezervacija

        public async Task<IActionResult> Index()
        {
            var dto = await _service.GetAllAsync();
            return View(dto);
        }
        //ispis zauzetih stolova za dati datum
        private async Task<List<int>> GetZauzetiStoloviAsync(DateTime datum)
        {
            var rezervacije = await _uow.Rezervacije.GetAllAsync();
            return rezervacije
                .Where(r => r.Datum.Date == datum.Date)
                .Select(r => r.Sto.BrojStola)
                .Distinct()
                .ToList();
        }
        //kreiranje rezervacije
        public async Task<IActionResult> Create()
        {
            var stolovi = await _uow.Stolovi.GetAllAsync();
            var danas = DateTime.Today;
            //ispis zauzetih stolova za danasnji dan
            var zauzeti = await GetZauzetiStoloviAsync(danas);
            ViewBag.ZauzetiStolovi = zauzeti;
            //padajuci meni sa stolovima
            ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
            return View();
        }
        //kreiranje rezervacije post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RezervacijaDto dto)
        {
            
            if (!ModelState.IsValid)
            {
                var stolovi = await _uow.Stolovi.GetAllAsync();
                var zauzeti = await GetZauzetiStoloviAsync(dto.Datum);
                ViewBag.ZauzetiStolovi = zauzeti;
                ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
                return View(dto);
            }

            try
            {
                
                await _service.CreateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                //graska ako je sto zauzet
                ModelState.AddModelError("", ex.Message);
                var stolovi = await _uow.Stolovi.GetAllAsync();
                var zauzeti = await GetZauzetiStoloviAsync(dto.Datum);
                ViewBag.ZauzetiStolovi = zauzeti;
               
                ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
                return View(dto);
            }
        }
        //izmena 
        public async Task<IActionResult> Edit(int id)
        {
            //prikaz rezervacije koja treba da se izmeni
            var dto = await _service.GetByIdAsync(id);
            //ako ne postoji
            if (dto == null) return NotFound();
            var stolovi = await _uow.Stolovi.GetAllAsync();
            var zauzeti = await GetZauzetiStoloviAsync(dto.Datum);
            ViewBag.ZauzetiStolovi = zauzeti;
            ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
            return View(dto);
        }
        //izmena post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RezervacijaDto dto)
        {
            if (!ModelState.IsValid)
            {
                var stolovi = await _uow.Stolovi.GetAllAsync();
                var zauzeti = await GetZauzetiStoloviAsync(dto.Datum);
                ViewBag.ZauzetiStolovi = zauzeti;
                ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
                return View(dto);
            }

            try
            {
                await _service.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var stolovi = await _uow.Stolovi.GetAllAsync();
                var zauzeti = await GetZauzetiStoloviAsync(dto.Datum);
                ViewBag.ZauzetiStolovi = zauzeti;
                ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
                return View(dto);
            }
        }
        //brisanje

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }
        //prikaz zauzetih stolova u json formatu za uneti datum
        [HttpGet]
        public async Task<JsonResult> GetZauzetiStoloviJson(DateTime datum)
        {
            var rezervacije = await _uow.Rezervacije.GetAllAsync();
            var zauzeti = rezervacije
                .Where(r => r.Datum.Date == datum.Date)
                .Select(r => r.Sto.BrojStola)
                .Distinct()
                .ToList();
            return Json(zauzeti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}