using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models {
   public class Animais {

      public Animais() {
         ListaConsultas = new HashSet<Consultas>();
      }


      [Key]
      public int ID { get; set; }

      public string Nome { get; set; }

      public string Especie { get; set; }

      public string Raca { get; set; }

      public double Peso { get; set; }

      public string Foto { get; set; }

      //***************************************
      // adição da FK
      /// CREATE TABLE Animais (
      ///   ID int primary key,
      ///   nome varchar (30) not null,
      ///   ....
      ///   donoFK int,
      ///   ForeignKey (donoFK) Reference Donos(ID)
      /// )
      //***************************************
      [ForeignKey("Dono")] // anotação: vai associar o atributo 'DonoFK' ao atributo 'Dono' com a característica de FK
      public int DonoFK { get; set; } // donoFK int
      public Donos Dono { get; set; } // Reference Donos(ID)


      //*************************************************
      public ICollection<Consultas> ListaConsultas { get; set; }



   }
}
