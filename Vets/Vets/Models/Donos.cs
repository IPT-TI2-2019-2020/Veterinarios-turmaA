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

      // lista de anotadores possíveis
      // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1


      /// <summary>
      /// Chave primária (PK)
      /// </summary>
      [Key]
      public int ID { get; set; }

      /// <summary>
      /// Nome do Dono
      /// </summary>
      [Required(ErrorMessage = "O Nome é de preenchimento obrigatório.")]
      [StringLength(40, ErrorMessage = "O {0} não pode ter mais de {1} carateres.")]
      [RegularExpression("[A-ZÂÓÍÉ][a-záéíóúàèìòùâêîôûãôûäëïöüçñ]+(( | d[oa](s)? | (d)?e |-|'| d')[A-ZÂÓÍÉ][a-záéíóúàèìòùâêîôûãôûäëïöüçñ]+){1,3}",
         ErrorMessage = "Só são aceites letras.<br />A primeira letra de cada nome é uma Maiúscula seguida de minúsculas.<br />Deve escrever entre 2 e 4 nomes.")]
      public string Nome { get; set; }

      /// <summary>
      /// Número de Identificação Fiscal, do Dono
      /// </summary>
      //  [Required(ErrorMessage ="O NIF é de preenchimento obrigatório.")]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [StringLength(9, MinimumLength = 9, ErrorMessage = "O {0} deve ter exatamente {1} caracteres.")]
      [RegularExpression("[1356][0-9]{8}", ErrorMessage = "Deve escrever exatamente 9 algarismos, começando por 1, 3, 5 ou 6.")] // <=> filtro
      public string NIF { get; set; }

      /// <summary>
      /// Sexo do(a) Dono(a)
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [StringLength (1)]
      [RegularExpression("[mfMF]")]
      public string Sexo { get; set; }


      //**************************************************
      // SELECT *
      // FROM Animais a, Donos d
      // WHERE a.DonoFK = d.ID AND
      //       d.ID = ??
      /// <summary>
      /// Lista dos Animais associados a um Dono
      /// </summary>
      public virtual ICollection<Animais> ListaAnimais { get; set; }


   }
}
