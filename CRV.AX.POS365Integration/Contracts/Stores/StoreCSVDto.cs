using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Contracts.Stores
{
    public class StoreCSVDto
    {
        public string StoreNumber { get; set; }

        public string URL { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
