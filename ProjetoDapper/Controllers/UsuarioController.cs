using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjetoDapper.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace ProjetoDapper.Controllers
{
    public class UsuarioController : Controller
    {
        public static TimeSpan tempo;
        private readonly IDbConnection _dbConnection;

        public UsuarioController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {    
            if (tempo != null){
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                tempo.Hours, tempo.Minutes, tempo.Seconds,
                tempo.Milliseconds / 10);
                ViewBag.Tempo = elapsedTime;
            }
            

            var ListaUsuario = _dbConnection.Query<Usuario>("select * from Usuario");
            return View(ListaUsuario);
        }

        public IActionResult Create()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();         
            
            
            if (ModelState.IsValid)
            {
                for (int i = 0; i < 3000; i++)
                {
                    usuario.Nome = usuario.Nome + i.ToString();
                    await _dbConnection.ExecuteAsync("insert into Usuario(nome, email) values(@nome,@email)", 
                    new {nome = usuario.Nome, email = usuario.Email});
                }
                
                stopwatch.Stop();
                tempo = stopwatch.Elapsed;           
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

            var usuario = _dbConnection.Query<Usuario>("select * from Usuario where id = " + id).Single();
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbConnection.Execute("update Usuario set nome = @nome, email = @email where id = @id", 
                        new{nome = usuario.Nome, email = usuario.Email, id = usuario.Id});                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(usuario.Id))
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
            return View(usuario);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _dbConnection.Query<Usuario>("select * from usuario where id =" + id).Single();

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dbConnection.Execute("delete from usuario where id = " + id);
         
            return RedirectToAction("Index");
        }

        private bool LivroExists(int id)
        {
            return _dbConnection.Query<bool>("select * from usuario where id = " + id).FirstOrDefault();
        }

    }
}