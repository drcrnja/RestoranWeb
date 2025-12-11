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

        public async Task<IActionResult> Index()
        {
            var dto = await _service.GetAllAsync();
            return View(dto);
        }

        public async Task<IActionResult> Create()
        {
            var stolovi = await _uow.Stolovi.GetAllAsync();
            ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RezervacijaDto dto)
        {
            if (!ModelState.IsValid)
            {
                var stolovi = await _uow.Stolovi.GetAllAsync();
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
                ModelState.AddModelError("", ex.Message);
                var stolovi = await _uow.Stolovi.GetAllAsync();
                ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            var stolovi = await _uow.Stolovi.GetAllAsync();
            ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RezervacijaDto dto)
        {
            if (!ModelState.IsValid)
            {
                var stolovi = await _uow.Stolovi.GetAllAsync();
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
                ViewBag.Stolovi = new SelectList(stolovi, "BrojStola", "BrojStola");
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
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