using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models {
   public class Donos {

      public Donos() {
         ListaAnimais = new HashSet<Animais>(); // estou a 'colocar' dados na lista dos animais, de cada 'dono'
      }

      [Key]
      public int ID { get; set; }

      public string Nome { get; set; }

      public string NIF { get; set; }

      //**************************************************
      // SELECT *
      // FROM Animais a, Donos d
      // WHERE a.DonoFK = d.ID AND
      //       d.ID = ??
      public ICollection<Animais> ListaAnimais { get; set; }


   }
}
