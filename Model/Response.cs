using ApiProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Model
{
    public class ApiResponse
    {
        public Produit[] Data { get; set; }
    }

    public class ProduitResponse
    {
        public Produit Data { get; set; }
    }
}
