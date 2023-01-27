using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.Models
{
  public class MemberProfileEditPasswordModel
  {
    //e-mail
    [Required(ErrorMessage = "E-mail je potřeba vyplnit.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný tvar e-mailové adresy.")]
    [EmailAddress(ErrorMessage = "Nesprávný formát e-mailu.")]
    public string email { get; set; }

    //heslo
    [Required(ErrorMessage = "Heslo je potřeba vyplnit."), DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Heslo musí být délky alespoň {2} znaků.", MinimumLength = 10)]
    [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Heslo musí obsahovat alespoň 3 ze 4: velké písmeno, malé písmeno, číslo a speciální znak (e.g. !@#$%^&*)")]
    public virtual string password { get; set; }

    [Required(ErrorMessage = "Heslo je potřeba vyplnit."), DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Heslo musí být délky alespoň {2} znaků.", MinimumLength = 10)]
    [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Heslo musí obsahovat alespoň 3 ze 4: velké písmeno, malé písmeno, číslo a speciální znak (e.g. !@#$%^&*)")]
    public virtual string newPassword { get; set; }

    //potvrzení hesla
    [Required(ErrorMessage = "Ověření hesla je potřeba vyplnit."), DataType(DataType.Password)]
    [Compare("newPassword", ErrorMessage = "Potvrzení hesla se neshoduje s heslem.")]
    public virtual string passwordCheck { get; set; }
  }
}