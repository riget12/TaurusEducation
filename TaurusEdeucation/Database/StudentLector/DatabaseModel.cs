using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace TaurusEdeucation.Database.StudentLector
{
  [TableName("StudentLectorDatabaseTable")]
  [PrimaryKey("Id", AutoIncrement = true)]
  [ExplicitColumns]
  public class StudentLectorDatabaseModel
  {
    [Column("ID")]
    [PrimaryKeyColumn(AutoIncrement = true)]
    public int Id { get; set; }

    [Column("STUDENTNAME")]
    public string studentName { get; set; }

    [Column("LECTOREMAIL")]
    public string lectorEmail { get; set; }

    [Column("ACTIVE")]
    public string active { get; set; }
  }
}