using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciamentoBiblioteca.Controllers;
using GerenciamentoBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Context
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {

        }

        public DbSet<Livro> Livros {get; set;}
        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Emprestimo> Emprestimos {get; set;}
    }
}