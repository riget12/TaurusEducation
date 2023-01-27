using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using TaurusEdeucation.Database.Student;

namespace TaurusEdeucation.Database.Lector
{
  public class LectorComponent : IComponent
  {
    private readonly IScopeProvider scopeProvider;
    private readonly IMigrationBuilder migrationBuilder;
    private readonly IKeyValueService keyValueService;
    private readonly ILogger logger;

    public LectorComponent(
        IScopeProvider scopeProvider,
        IMigrationBuilder migrationBuilder,
        IKeyValueService keyValueService,
        ILogger logger)
    {
      this.scopeProvider = scopeProvider;
      this.migrationBuilder = migrationBuilder;
      this.keyValueService = keyValueService;
      this.logger = logger;
    }

    public void Initialize()
    {
      var upgrader = new Upgrader(new LectorMigrationPlan());
      upgrader.Execute(scopeProvider, migrationBuilder, keyValueService, logger);
    }

    public void Terminate()
    {
      throw new NotImplementedException();
    }
  }
}