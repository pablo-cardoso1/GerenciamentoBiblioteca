using System.Linq;
using GerenciamentoBiblioteca.Context;
using GerenciamentoBiblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoBiblioteca.Controllers
{
    [Authorize] // Aplica autorização a todas as ações neste controlador
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

        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem acessar
        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem adicionar
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

        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem editar
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem editar
        public IActionResult Editar(Usuario usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(usuario.Id);
            if (usuarioBanco == null)
                return NotFound();

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Telefone = usuario.Telefone;
            usuarioBanco.Senha = usuario.Senha;

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

        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem remover
        public IActionResult Remover(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return RedirectToAction(nameof(Index));

            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem confirmar a remoção
        public IActionResult Remover(Usuario usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(usuario.Id);
            if (usuarioBanco == null)
                return NotFound();

            _context.Usuarios.Remove(usuarioBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
