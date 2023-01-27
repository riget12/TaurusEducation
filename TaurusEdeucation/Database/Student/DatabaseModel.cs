using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace TaurusEdeucation.Database.Student
{
  [TableName("StudentDatabaseTable")]
  [PrimaryKey("Id", AutoIncrement = true)]
  [ExplicitColumns]
  public class StudentDatabaseModel
  {
    [Column("ID")]
    [PrimaryKeyColumn(AutoIncrement = true)]
    public int Id { get; set; }

    [Column("NAME")]
    public string name { get; set; }

    [Column("EMAIL")]
    public string email { get; set; }

    [Column("LOCATION")]
    public string location { get; set; }

    [Column("LEVEL")]
    public string level { get; set; }

    [Column("PHONE")]
    public string phone { get; set; }
  }
}