using Umbraco.Core.Migrations;

namespace TaurusEdeucation.Database.Lector
{
  public class LectorMigrationPlan : MigrationPlan
  {
    public LectorMigrationPlan()
        : base("LectorDatabaseTable")
    {
      From(string.Empty)
          .To<LectorCreateTables>("first-migration Lector");
    }
  }

  public class LectorCreateTables : MigrationBase
  {
    public LectorCreateTables(IMigrationContext context)
        : base(context)
    {

    }

    public override void Migrate()
    {
      if (!TableExists("LectorDatabaseTable"))
      {
        Create.Table<LectorDatabaseModel>().Do();
      }
    }
  }
}