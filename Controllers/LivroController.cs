using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GerenciamentoBiblioteca.Context;
using GerenciamentoBiblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoBiblioteca.Controllers
{

    [Authorize]
    public class LivroController : Controller
    {
        private readonly BibliotecaContext _context;

        public LivroController(BibliotecaContext context)
        {
            _context = context;
        }


        public IActionResult Index(string searchTitulo, string searchAutor, int? searchAno)
        {
            var livros = _context.Livros.AsQueryable();

            // Filtros de busca
            if (!string.IsNullOrEmpty(searchTitulo))
            {
                livros = livros.Where(l => l.Titulo.Contains(searchTitulo));
            }

            if (!string.IsNullOrEmpty(searchAutor))
            {
                livros = livros.Where(l => l.Autor.Contains(searchAutor));
            }

            if (searchAno.HasValue)
            {
                livros = livros.Where(l => l.AnoPublicacao == searchAno);
            }

            return View(livros.ToList());
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
                // Adicione o livro ao banco de dados
                _context.Livros.Add(livro);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Se a validação falhar, retorne à visão com os erros
            return View(livro);
        }
        
        
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var livro = _context.Livros.Find(id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        
        [HttpPost]
        public IActionResult Editar(Livro livro)
        {
            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return View(livro); // Retorna a view com os erros de validação
            }

            var livroBanco = _context.Livros.Find(livro.Id);
            if (livroBanco == null)
            {
                return NotFound(); // Se não encontrar o livro, retorna NotFound
            }

            // Atualiza as propriedades do livro
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

        [Authorize(Roles = "Admin")]
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