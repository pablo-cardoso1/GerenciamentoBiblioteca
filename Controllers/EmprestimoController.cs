using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GerenciamentoBiblioteca.Context;
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Controllers
{
    public class EmprestimoController : Controller
    {
        private readonly BibliotecaContext _context;

        public EmprestimoController(BibliotecaContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var emprestimos = _context.Emprestimos.ToList();
            return View(emprestimos);
        }

        
        public IActionResult Emprestar()
        {
            ViewBag.Livros = _context.Livros.ToList(); 
            ViewBag.Usuarios = _context.Usuarios.ToList(); 
            return View();
        }

        [HttpPost]
        public IActionResult Emprestar(Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                var livro = _context.Livros.Find(emprestimo.IdLivro);

                if (livro != null && livro.Quantidade > 0)
                {                    
                    emprestimo.DataEmprestimo = DateTime.Now;
                    _context.Emprestimos.Add(emprestimo);
                    
                    livro.Quantidade--;
                   
                    _context.Livros.Update(livro);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index)); 
                }
                ModelState.AddModelError("", "Livro não encontrado ou quantidade insuficiente.");
            }

            ViewBag.Livros = _context.Livros.ToList();
            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult Devolver(int id)
        {
            var emprestimo = _context.Emprestimos.Find(id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            
            emprestimo.DataDevolucao = DateTime.Now;

            
            var livro = _context.Livros.Find(emprestimo.IdLivro);
            if (livro != null)
            {
                livro.Quantidade++; 
            }

            _context.SaveChanges(); 

            return RedirectToAction("Index");
        }




    }
}

