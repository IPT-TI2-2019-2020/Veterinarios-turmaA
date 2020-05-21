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
   public class VeterinariosController : Controller {
      private readonly VetsDB _context;

      public VeterinariosController(VetsDB context) {
         _context = context;
      }



      // GET: Veterinarios
      public async Task<IActionResult> Index() {
         return View(await _context.Veterinarios.ToListAsync());
      }




      // GET: Veterinarios/Details/5
      /// <summary>
      /// Mostra os dados de um veterinário, acedendo aos dados relativos a ele,
      /// associados às consultas, aos seus animais e respetivos donos
      /// </summary>
      /// <param name="id">identificador do veterinário a apresentar os detalhes</param>
      /// <returns></returns>
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return RedirectToAction("Index");
         }

         // acesso aos dados será feito em modo 'lazy loading'
         // adicionar o termo 'virtual' aos atributos que exprimem os relacionamentos
         // adicionar a package: Install-Package Microsoft.EntityFrameworkCore.Proxies
         // 'ligar' o Lazy Loading
         var veterinario = await _context.Veterinarios.FirstOrDefaultAsync(m => m.ID == id);

         if (veterinario == null) {
            return RedirectToAction("Index");
         }

         return View(veterinario);
      }


      /// <summary>
      /// Mostra os dados de um veterinário, acedendo aos dados relativos a ele,
      /// associados às consultas, aos seus animais e respetivos donos
      /// </summary>
      /// <param name="id">identificador do veterinário a apresentar os detalhes</param>
      /// <returns></returns>
      public async Task<IActionResult> Details2(int? id) {
         if (id == null) {
            return RedirectToAction("Index");
         }

         // acesso aos dados será feito em modo 'Eager Loading'
         // acesso aos dados em modo 'antecipado'
         // na prática, far-se-á esta consulta
         // SELECT *
         // FROM Consultas c, Animais a, Donos d, Veterinarios v
         // WHERE c.VeterinarioFK=v.ID AND
         //       c.AnimalFK=a.ID AND
         //       a.DonoFK=d.ID AND
         //       v.ID=id
         var veterinario = await _context.Veterinarios
                                         .Include(v=>v.ListaConsultas)
                                         .ThenInclude(a=>a.Animal)
                                         .ThenInclude(d=>d.Dono)         
                                         .FirstOrDefaultAsync(m => m.ID == id);

         if (veterinario == null) {
            return RedirectToAction("Index");
         }

         return View(veterinario);
      }


      // GET: Veterinarios/Create
      public IActionResult Create() {
         return View();
      }

      // POST: Veterinarios/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("ID,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios) {
         if (ModelState.IsValid) {
            _context.Add(veterinarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         return View(veterinarios);
      }

      // GET: Veterinarios/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await _context.Veterinarios.FindAsync(id);
         if (veterinarios == null) {
            return NotFound();
         }
         return View(veterinarios);
      }

      // POST: Veterinarios/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios) {
         if (id != veterinarios.ID) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _context.Update(veterinarios);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!VeterinariosExists(veterinarios.ID)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(veterinarios);
      }

      // GET: Veterinarios/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await _context.Veterinarios
             .FirstOrDefaultAsync(m => m.ID == id);
         if (veterinarios == null) {
            return NotFound();
         }

         return View(veterinarios);
      }

      // POST: Veterinarios/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var veterinarios = await _context.Veterinarios.FindAsync(id);
         _context.Veterinarios.Remove(veterinarios);
         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool VeterinariosExists(int id) {
         return _context.Veterinarios.Any(e => e.ID == id);
      }
   }
}
