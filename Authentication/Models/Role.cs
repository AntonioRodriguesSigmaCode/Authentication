using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public ICollection<Utilizador> Utilizadores { get; set; }
        public ICollection<Permissao> Permissoes { get; set; }
    }
}