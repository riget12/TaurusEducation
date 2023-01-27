using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TaurusEdeucation.Models.DropDown;
using TaurusEdeucation.Validations;
using Umbraco.Web;

namespace TaurusEdeucation.Models
{
    public class MemberProfileEditModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Jméno je potřeba vyplnit.")]
        [RegularExpression(@"^\p{L}+$", ErrorMessage = "Použijte pouze malá nebo velká písmena .")]
        [StringLength(30, ErrorMessage = "Jméno může být délky maximálně 30 znaků.")]
        [DataType(DataType.Text)]
        [DisplayName("Jméno")]
        public string FirstName { get; set; }

        //příjmení
        [Required(ErrorMessage = "Příjmení je potřeba vyplnit.")]
        [RegularExpression(@"^\p{L}+$", ErrorMessage = "Použijte pouze malá nebo velká písmena .")]
        [StringLength(30, ErrorMessage = "Příjmení může být délky maximálně 30 znaků.")]
        [DataType(DataType.Text)]
        [DisplayName("Příjmení")]
        public string SurName { get; set; }

        //ulice
        [Required(ErrorMessage = "Ulice je potřeba vyplnit.")]
        [DataType(DataType.Text)]
        [DisplayName("Ulice a č.p.")]
        public string Street { get; set; }

        //město
        [Required(ErrorMessage = "Město je potřeba vyplnit.")]
        [DataType(DataType.Text)]
        [DisplayName("Město")]
        public string City { get; set; }

        //telefonní číslo
        [Required(ErrorMessage = "Telefonní číslo je potřeba vyplnit.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Nesprávný tvar telefonního čísla.")]
        [Phone]
        [DisplayName("Telefon")]
        public string Phone { get; set; }

        //e-mail
        [Required(ErrorMessage = "E-mail je potřeba vyplnit.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný tvar e-mailové adresy.")]
        [EmailAddress(ErrorMessage = "Nesprávný formát e-mailu.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        //úrovně studií
        [ValidateAnyElementOfArray(ErrorMessage = "Vyberte úroveň školení.")]
        [DisplayName("Úroveň školení")]
        public string[] Levels { get; set; }

        //předměty
        [ValidateAnyElementOfArray(ErrorMessage = "Musíte vybrat alespoň jeden předmět.")]
        [DisplayName("Předměty")]
        public string[] Lessons { get; set; }

        public Levels LevelList = new Levels();

        //kraj, kde se učitel nachází
        public string Kraj { get; set; }

        //místo kde se učitel nachází
        public string Okres { get; set; }

        //Zda-li se lektor zobrazuje
        public bool IsHidden { get; set; }

        public string Resume { get; set; }

        public string Photo { get; set; }
    }
}