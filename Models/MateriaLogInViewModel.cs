using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pruebas2.Models
{
    public class MateriaLogInViewModel
    {
        [Key]
        public string ID { get; set; }
    }
}