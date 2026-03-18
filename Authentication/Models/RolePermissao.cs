using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projetoAPI.Model
{
    public class RolePermissao
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int PermissaoId { get; set; }
        public Permissao Permissao { get; set; }
    }
}