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


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// relação entre utiliador e roles (Muito para Muitos)
			modelBuilder.Entity<Utilizador>()
				.HasMany(e => e.Roles)
				.WithMany(e => e.Utilizadores)
				.UsingEntity(e => e.ToTable("UtilizadorRoles"));

			// relação entre role e permissao (Um para Muitos)
			modelBuilder.Entity<Role>()
				.HasMany(e => e.Permissoes)
				.WithOne(e => e.Role)
				.HasForeignKey(e => e.RoleId);
		}
	}
}