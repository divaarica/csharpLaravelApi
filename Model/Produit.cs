using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiProject.Model
{
    public class Produit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public int Prix { get; set; }


    }

    public class ProduitsResponse
    {
        public Produit[] Data { get; set; }
    }

    public class ProduitResponse
    {
        public Produit Data { get; set; }
    }
}
