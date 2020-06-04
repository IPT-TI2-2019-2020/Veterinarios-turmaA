using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Data;
using Vets.Models;

namespace Vets.Controllers {

   [Authorize] // fecha o acesso a qualquer recurso da classe a Utilizadores não Autenticadados
   public class VeterinariosController : Controller {

      private readonly VetsDB _context;

      private readonly IWebHostEnvironment _ambiente;

      public VeterinariosController(VetsDB context,
         IWebHostEnvironment ambiente
         ) {
         _context = context;
         _ambiente = ambiente; // estou a injetar os dados do Servidor Web no meu método
      }



      // GET: Veterinarios
      [AllowAnonymous]  // anula o efeito do [Authorize]
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
                                         .Include(v => v.ListaConsultas)
                                         .ThenInclude(a => a.Animal)
                                         .ThenInclude(d => d.Dono)
                                         .FirstOrDefaultAsync(m => m.ID == id);

         if (veterinario == null) {
            return RedirectToAction("Index");
         }

         return View(veterinario);
      }


      // GET: Veterinarios/Create
      /// <summary>
      /// Apresenta o formulário de Criação de um novo Veterinário
      /// </summary>
      /// <returns></returns>
      [Authorize]
      public IActionResult Create() {
         return View();
      }

      // POST: Veterinarios/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize]
      public async Task<IActionResult> Create([Bind("Nome,NumCedulaProf,Fotografia")] Veterinarios veterinario, IFormFile fotoVet) {

         //****************************************
         //   processar o ficheiro da Fotografia
         //****************************************

         // vars. auxiliares
         string caminhoCompleto = "";
         bool haImagem = false;

         // será que há ficheiro?
         if (fotoVet == null) {
            // não há ficheiro!
            // o que vai ser feito?
            //   - devolver o controlo para a View, informando que é necessário escolher uma fotografia
            //       ModelState.AddModelError("", "Não se esqueça de adicionar uma fotografia do Veterinário");
            //       return View(veterinario);
            //   - adicionar uma fotografia 'por defeito'
            veterinario.Fotografia = "noFoto.png";
         }
         else {
            // há ficheiro.
            // será que é uma imagem?
            if (fotoVet.ContentType == "image/jpeg" ||
               fotoVet.ContentType == "image/png") {
               // temos imagem. Ótimo!
               // temos de gerar um nome para o ficheiro
               Guid g;
               g = Guid.NewGuid();
               // identificar a Extensão do ficheiro
               string extensao = Path.GetExtension(fotoVet.FileName).ToLower();
               // nome do ficheiro
               string nome = g.ToString() + extensao;
               // preparar o ficheiro para ser guardado, mas não o vamos guardar já...
               // precisamos de identificar o caminho onde o ficheiro vai ser guardado
               caminhoCompleto = Path.Combine(_ambiente.WebRootPath, "Imagens\\Vets", nome);
               // associar o nome da fotografia ao Veterinário 
               veterinario.Fotografia = nome;
               // assinalar que existe imagem
               haImagem = true;
            }
            else {
               // há ficheiro, MAS não é uma imagem
               // o que vai ser feito?
               //   - devolver o controlo para a View, informando que é necessário escolher uma fotografia
               //       ModelState.AddModelError("", "Não se esqueça de adicionar uma fotografia do Veterinário");
               //       return View(veterinario);
               //   - adicionar uma fotografia 'por defeito'
               veterinario.Fotografia = "noFoto.png";
            }

         }

         if (ModelState.IsValid) {
            // adiciona o Veterinário ao Modelo 
            _context.Add(veterinario);
            // consolida, na BD, as alterações
            await _context.SaveChangesAsync();
            // o registo foi guardado
            // o ficheiro vai agora ser guardado no disco rígido
            if (haImagem) {
               using var stream = new FileStream(caminhoCompleto, FileMode.Create);
               await fotoVet.CopyToAsync(stream);
            }
            // redireciona o utilizador para a View Index
            return RedirectToAction(nameof(Index));
         }

         // se o modelo não for válido, devolve o controlo à view do Create
         return View(veterinario);
      }

      // GET: Veterinarios/Edit/5
      [Authorize]
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
      [Authorize]
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
