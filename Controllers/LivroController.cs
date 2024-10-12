using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GerenciamentoBiblioteca.Context;
using GerenciamentoBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoBiblioteca.Controllers{
    
    public class LivroController : Controller
    {
        private readonly BibliotecaContext _context;

        public LivroController(BibliotecaContext context)
        {
            _context = context;
        }   
        

        public IActionResult Index()
        {
            var livros = _context.Livros.ToList();
            return View(livros);
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Livro livro)
        {
            if (ModelState.IsValid)
            {
                _context.Livros.Add(livro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(livro);
        }

        public IActionResult Editar(int id)
        {
            var livro = _context.Livros.Find(id);

            if(livro == null)
                return NotFound();
            
            return View(livro);
        }

        [HttpPost]
        public IActionResult Editar(Livro livro)
        {
            var livroBanco = _context.Livros.Find(livro.Id);

            livroBanco.Titulo = livro.Titulo;
            livroBanco.Autor = livro.Autor;
            livroBanco.AnoPublicacao = livro.AnoPublicacao;
            livroBanco.Quantidade = livro.Quantidade;

            _context.Livros.Update(livroBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detalhes(int id)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null)
                return RedirectToAction(nameof(Index));
            
            return View(livro);
        }

        public IActionResult Remover(int id)
        {
            var livro = _context.Livros.Find(id);

            if (livro == null)
                return RedirectToAction(nameof(Index));

            return View(livro);
        }

        [HttpPost]
        public IActionResult Remover(Livro livro)
        {
            var livroBanco = _context.Livros.Find(livro.Id);

            _context.Livros.Remove(livroBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}