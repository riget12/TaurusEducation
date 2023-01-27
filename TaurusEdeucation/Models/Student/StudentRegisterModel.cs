using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TaurusEdeucation.Models.DropDown;

namespace TaurusEdeucation.Models
{
    public class StudentRegisterModel
    {
        //jméno
        [Required(ErrorMessage = "Jméno je potřeba vyplnit.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Použijte pouze malá nebo velká písmena .")]
        [StringLength(30, ErrorMessage = "Jméno může být délky maximálně 30 znaků.")]
        [DataType(DataType.Text)]
        [DisplayName("Jméno")]
        public string FirstName { get; set; }

        //příjmení
        [Required(ErrorMessage = "Příjmení je potřeba vyplnit.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Použijte pouze malá nebo velká písmena .")]
        [StringLength(30, ErrorMessage = "Příjmení může být délky maximálně 30 znaků.")]
        [DataType(DataType.Text)]
        [DisplayName("Příjmení")]
        public string SurName { get; set; }

        //email
        [Required(ErrorMessage = "E-mail je potřeba vyplnit.")]
        [EmailAddress(ErrorMessage = "Nesprávný formát e-mailu.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        //telefon
        [Required(ErrorMessage = "Telefonní číslo je potřeba vyplnit.")]
        [StringLength(20, ErrorMessage = "Jméno může být délky maximálně 20 znaků.")]
        [DataType(DataType.Text)]
        [DisplayName("Telefon")]
        public string Phone { get; set; }

        //učitel
        [Required(ErrorMessage = "Je potřeba vybrat učitele.")]
        [DataType(DataType.Text)]
        public string Lector { get; set; }

        //úrovně studií
        [DisplayName("Úroveň školení")]
        public string Level { get; set; }

        //předměty
        [DisplayName("Předměty")]
        public string Lesson { get; set; }

        public Levels LevelList = new Levels();

        public Status Status = new Status();

        //seznam učitelů
        public List<SelectListItem> LectorsList = new List<SelectListItem>();

        public string Kraj { get; set; }

        public string Okres { get; set; }
    }
}