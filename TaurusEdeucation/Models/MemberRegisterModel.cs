using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TaurusEdeucation.Models.DropDown;
using TaurusEdeucation.Models.SubModels;
using TaurusEdeucation.Validations;

namespace TaurusEdeucation.Models
{
    //model registračního formuláře
    public class MemberRegisterModel
    {
        //křestní jméno
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

        //heslo
        [Required(ErrorMessage = "Heslo je potřeba vyplnit."), DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Heslo musí být délky alespoň {2} znaků.", MinimumLength = 10)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Heslo musí obsahovat alespoň 3 ze 4: velké písmeno, malé písmeno, číslo a speciální znak (e.g. !@#$%^&*)")]
        [DisplayName("Heslo")]
        public virtual string Password { get; set; }

        //potvrzení hesla
        [Required(ErrorMessage = "Ověření hesla je potřeba vyplnit."), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Potvrzení hesla se neshoduje s heslem.")]
        [DisplayName("Heslo znovu")]
        public virtual string PasswordCheck { get; set; }

        //úrovně studií
        [ValidateAnyElementOfArray(ErrorMessage = "Vyberte úroveň školení.")]
        [DisplayName("Úroveň školení")]
        public string[] Levels { get; set; }

        //předměty
        [ValidateAnyElementOfArray(ErrorMessage = "Musíte vybrat alespoň jeden předmět.")]
        [DisplayName("Předměty")]
        public string[] Lessons { get; set; }

        public Levels LevelList = new Levels();

        public List<LevelViewModel> LevelViews = new List<LevelViewModel>();

        //kraj, kde se učitel nachází
        public string Kraj { get; set; }

        //místo kde se učitel nachází
        public string Okres { get; set; }

        //zobrazuje se
        public bool IsHidden { get; set; }

        public string Resume { get; set; }

        public Image Photo { get; set; }

        //souhlas s podmínkama
        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíte souhlasit s podmínkami.")]
        public virtual bool AgreedToTerms { get; set; }
    }
}