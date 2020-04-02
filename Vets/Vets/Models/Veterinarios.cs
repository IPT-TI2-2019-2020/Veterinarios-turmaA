using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models {
   public class Veterinarios {


      public Veterinarios() {
         ListaConsultas = new HashSet<Consultas>();
      }



      // create table Veterinarios(
      //   ID integer Primary Key autoincrement,
      //   Nome varchar(60) not null,
      //   .....
      // )

      [Key]
      public int ID { get; set; }

      public string Nome { get; set; }

      public string NumCedulaProf { get; set; }

      public string Fotografia { get; set; }

      //*********************************************
      public ICollection<Consultas> ListaConsultas { get; set; }

   }
}
