using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace TaurusEdeucation.Database.Lector
{
  [TableName("LectorDatabaseTable")]
  [PrimaryKey("Id", AutoIncrement = true)]
  [ExplicitColumns]
  public class LectorDatabaseModel
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

    [Column("ELEMENTARY")]
    public bool elementary { get; set; }

    [Column("HIGH")]
    public bool high { get; set; }

    [Column("COLLEGE")]
    public bool college { get; set; }

    [Column("LESSONS")]
    public string lessons { get; set; }
  }
}