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

      [Key] // PK
      public int ID { get; set; }

      [Required(ErrorMessage ="O Nome é de preenchimento obrigatório.")]
      [StringLength (40, ErrorMessage ="O {0} não pode ter mais de {1} carateres.")]
      public string Nome { get; set; }

    //  [Required(ErrorMessage ="O NIF é de preenchimento obrigatório.")]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [StringLength (9, MinimumLength =9,ErrorMessage ="O {0} deve ter exatamente {1} caracteres.")]
      [RegularExpression("[1356][0-9]{8}", ErrorMessage ="Deve escrever exatamente 9 algarismos, começando por 1, 3, 5 ou 6.")] // <=> filtro
      public string NIF { get; set; }

      //**************************************************
      // SELECT *
      // FROM Animais a, Donos d
      // WHERE a.DonoFK = d.ID AND
      //       d.ID = ??
      public ICollection<Animais> ListaAnimais { get; set; }


   }
}
