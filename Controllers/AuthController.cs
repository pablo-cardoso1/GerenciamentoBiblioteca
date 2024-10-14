using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using GerenciamentoBiblioteca.Models;
using System.Security.Claims;
using GerenciamentoBiblioteca.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GerenciamentoBiblioteca.Controllers
{
    public class AuthController : Controller
    {
        private readonly BibliotecaContext _context;

        public AuthController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous] // Permite o acesso anônimo à página de login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous] // Permite o acesso anônimo ao método de login
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email); // Busque apenas pelo email

                // Verifique a senha usando VerifyPassword
                if (usuario != null && VerifyPassword(model.Senha, usuario.Senha))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.Name, usuario.Nome),
                        new Claim(ClaimTypes.Role, usuario.Tipo)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties { IsPersistent = true }); // Cookie persistente

                    return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
                }

                ModelState.AddModelError("", "Email ou senha inválidos.");
            }

            return View(model); // Retorna a view com os erros se não conseguir autenticar
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        // Função de verificação de senha (ajuste para incluir lógica de hashing se necessário)
        private bool VerifyPassword(string inputSenha, string storedSenha)
        {
            // Para uma implementação de senha segura, compare o hash da senha de entrada com o armazenado
            return inputSenha == storedSenha; // Simples comparação de string por enquanto
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Exibir o formulário de cadastro
        [HttpGet]
        [AllowAnonymous] // Permite o acesso anônimo
        public IActionResult Cadastro()
        {
            return View(new CadastroModel());
        }

        // Processar o envio do cadastro        
        [HttpPost]
        [AllowAnonymous] // Permite o acesso anônimo ao método de cadastro
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(CadastroModel model)
        {
            // Log os dados recebidos
            Console.WriteLine($"Nome: {model.Nome}, Email: {model.Email}, Telefone: {model.Telefone}, Senha: {model.Senha}");
            if (ModelState.IsValid)
            {
                // Crie um novo usuário a partir do modelo
                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Telefone = model.Telefone,
                    Senha = model.Senha, // Hasheie a senha antes de armazenar
                    Tipo = "User" // Define o tipo como "User" por padrão
                };

                // Verifique se o email já está em uso
                var emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
                if (emailExistente)
                {
                    ModelState.AddModelError("Email", "Este email já está cadastrado.");
                    return View(model);
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth"); // Redireciona para a página de login
            }
            else
            {
                // Log se o ModelState não é válido
                Console.WriteLine("ModelState não é válido.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(model); // Retorna a view com os erros se não conseguir cadastrar
        }

    }
}

