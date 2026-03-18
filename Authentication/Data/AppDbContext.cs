using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projetoAPI.Model;

namespace projetoAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<Utilizador> Utilizadores { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Permissao> Permissoes { get; set; }

		public DbSet<UtilizadorRole> UtilizadorRoles { get; set; }
		public DbSet<RolePermissao> RolePermissoes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UtilizadorRole>()
				.HasKey(ur => new { ur.UtilizadorId, ur.RoleId });

			modelBuilder.Entity<RolePermissao>()
				.HasKey(rp => new { rp.RoleId, rp.PermissaoId });
		}
	}
}