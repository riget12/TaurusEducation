using System.ComponentModel.DataAnnotations;

namespace TaurusEdeucation.Models
{
  public class MemberLoginModel
  {
    [Required, Display(Name = "Váš e-mail")]
    public string Username { get; set; }

    [Required, Display(Name = "Heslo"), DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Pamatovat si mě")]
    public bool RememberMe { get; set; }

  }
}