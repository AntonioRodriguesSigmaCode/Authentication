using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Model
{
    public class Permissao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public ICollection<RolePermissao> RolePermissoes { get; set; }
    }
}