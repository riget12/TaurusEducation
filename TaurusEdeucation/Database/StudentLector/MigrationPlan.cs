using Umbraco.Core.Migrations;

namespace TaurusEdeucation.Database.StudentLector
{
  public class StudentLectorMigrationPlan : MigrationPlan
  {
    public StudentLectorMigrationPlan()
        : base("StudentLectorDatabaseTable")
    {
      From(string.Empty)
          .To<StudentLectorCreateTables>("first-migration StudentLector");
    }
  }

  public class StudentLectorCreateTables : MigrationBase
  {
    public StudentLectorCreateTables(IMigrationContext context)
        : base(context)
    {

    }

    public override void Migrate()
    {
      if (!TableExists("StudentLectorDatabaseTable"))
      {
        Create.Table<StudentLectorDatabaseModel>().Do();
      }
    }
  }
}