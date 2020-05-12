using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoDapper.Models
{
    public class Produto
    {

        public int Id { get; set; }        

        public string Nome { get; set; }    

        public decimal Preco { get; set; }


    }
}