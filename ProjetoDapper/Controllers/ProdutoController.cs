using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDapper.Models;


namespace ProjetoDapper.Controllers
{
    public class ProdutoController: Controller
    {
        public static string tempo;
        private readonly Contexto _context;

        public ProdutoController(Contexto contexto)
        {
            _context = contexto;
        }

        
        public IActionResult Index()
        {
            if (tempo != string.Empty) ViewBag.Tempo = tempo;

            return View(_context.Produto.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Produto produto)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            if (ModelState.IsValid)
            {
                for (int i = 0; i < 200; i++)
                {
                    produto.Nome = produto.Nome + i.ToString();
                    _context.Add(produto);
                    await _context.SaveChangesAsync();
                }
                            
                stopwatch.Stop();
                tempo = stopwatch.Elapsed.ToString();           
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = _context.Produto.SingleOrDefault(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Produto livro)
        {
            if (id != livro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(livro);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = _context.Produto
                .SingleOrDefault(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Produto.SingleOrDefaultAsync(m => m.Id == id);
            _context.Produto.Remove(livro);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LivroExists(int id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }


    }
}