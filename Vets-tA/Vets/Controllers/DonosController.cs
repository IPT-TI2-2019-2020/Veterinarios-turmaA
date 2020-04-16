using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Data;
using Vets.Models;

namespace Vets.Controllers {
   public class DonosController : Controller {
      private readonly VetsDB _db; // em SQL, <=> use VetsDB;

      public DonosController(VetsDB context) {
         _db = context;
      }

      // GET: Donos
      public async Task<IActionResult> Index() {

         // _db.Donos.ToListAsync() <=> em SQL, a
         // LINQ
         // SELECT * FROM Donos;

         return View(await _db.Donos.ToListAsync());
      }

      // GET: Donos/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         var donos = await _db.Donos
             .FirstOrDefaultAsync(m => m.ID == id);
         if (donos == null) {
            return NotFound();
         }

         return View(donos);
      }

      // GET: Donos/Create
      public IActionResult Create() {
         return View();
      }

      // POST: Donos/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(Donos dono)
      //  public IActionResult Index(string visor, string bt, string primeiroOperando, string operador, string limpaVisor) {
      {
         if (ModelState.IsValid) {
            _db.Add(dono); // INSERT INTO Donos VALUE (...);
            await _db.SaveChangesAsync(); // COMMIT
            return RedirectToAction(nameof(Index));
         }
         return View(dono);
         //  ViewBag.Operador = operador;
      }

      // GET: Donos/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var donos = await _db.Donos.FindAsync(id);
         if (donos == null) {
            return NotFound();
         }
         return View(donos);
      }

      // POST: Donos/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,NIF")] Donos donos) {
         if (id != donos.ID) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _db.Update(donos);
               await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!DonosExists(donos.ID)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(donos);
      }

      // GET: Donos/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var donos = await _db.Donos
             .FirstOrDefaultAsync(m => m.ID == id);
         if (donos == null) {
            return NotFound();
         }

         return View(donos);
      }

      // POST: Donos/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var donos = await _db.Donos.FindAsync(id);
         _db.Donos.Remove(donos);
         await _db.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool DonosExists(int id) {
         return _db.Donos.Any(e => e.ID == id);
      }
   }
}
