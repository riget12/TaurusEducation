using System.ComponentModel.DataAnnotations;

namespace TaurusEdeucation.Models
{
  public class MemberWorkPlaceUpdate
  {
    [DataType(DataType.Text)]
    public string WorkArea { get; set; }

    [DataType(DataType.Text)]
    public string WorkPlace { get; set; }

    [DataType(DataType.Text)]
    public string MemberId { get; set; }
  }
}