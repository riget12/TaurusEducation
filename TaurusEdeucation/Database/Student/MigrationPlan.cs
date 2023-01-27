using Umbraco.Core.Migrations;

namespace TaurusEdeucation.Database.Student
{
  public class StudentMigrationPlan : MigrationPlan
  {
    public StudentMigrationPlan()
        : base("StudentDatabaseTable")
    {
      From(string.Empty)
          .To<StudentCreateTables>("first-migration Student");
    }
  }

  public class StudentCreateTables : MigrationBase
  {
    public StudentCreateTables(IMigrationContext context)
        : base(context)
    {

    }

    public override void Migrate()
    {
      if (!TableExists("StudentDatabaseTable"))
      {
        Create.Table<StudentDatabaseModel>().Do();
      }
    }
  }
}