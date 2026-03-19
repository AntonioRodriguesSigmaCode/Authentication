using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projetoAPI.Model
{
    public class Permissao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}