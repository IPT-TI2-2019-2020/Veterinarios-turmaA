using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models {
   public class Veterinarios {

      // create table Veterinarios(
      //   ID integer Primary Key autoincrement,
      //   Nome varchar(60) not null,
      //   .....
      // )

     public int ID { get; set; }

      public string Nome{ get; set; }

      public string NumCedulaProf { get; set; }

      public string Fotografia { get; set; }



   }
}
