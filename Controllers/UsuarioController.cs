using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciamentoBiblioteca.Context;
using Microsoft.AspNetCore.Mvc;
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly BibliotecaContext _context;

        public UsuarioController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }
        
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if(usuario == null)
                return NotFound();
            
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(usuario.Id);

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Telefone = usuario.Telefone;           

            _context.Usuarios.Update(usuarioBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

         public IActionResult Detalhes(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return RedirectToAction(nameof(Index));
            
            return View(usuario);
        }

        public IActionResult Remover(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return RedirectToAction(nameof(Index));
            
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Remover(Usuario usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(usuario.Id);

            _context.Usuarios.Remove(usuarioBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}