using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Model
{
    public class UtilizadorRole
    {
        public int UtilizadorId { get; set; }
        public Utilizador Utilizador { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}