using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Authentication.Models;
using Authentication.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Authentication.Data
{
	public class AppDbContext : IdentityDbContext<Utilizador>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<Utilizador> Utilizadores { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Permissao> Permissoes { get; set; }

		public DbSet<UtilizadorRole> UtilizadorRole { get; set; }
		public DbSet<RolePermissao> RolePermissoe { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UtilizadorRole>()
				.HasKey(ur => new { ur.UtilizadorId, ur.RoleId });

			modelBuilder.Entity<RolePermissao>()
				.HasKey(rp => new { rp.RoleId, rp.PermissaoId });

			modelBuilder.Entity<UtilizadorRole>()
				.ToTable("UtilizadorRole");
		}
	}
}