using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaurusEdeucation.Database.Lector;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web.Scheduling;

namespace Umbraco.Web.UI
{
    // We start by setting up a composer and component so our task runner gets registered on application startup
    public class AutoSenderComposer : ComponentComposer<AutoSenderComponent>
    {
    }

    public class AutoSenderComponent : IComponent
    {
        private IProfilingLogger Logger;
        private IRuntimeState Runtime;
        private IContentService ContentService;
        private BackgroundTaskRunner<IBackgroundTask> AutoSenderRunner;

        public AutoSenderComponent(IProfilingLogger logger, IRuntimeState runtime, IContentService contentService)
        {
            Logger = logger;
            Runtime = runtime;
            ContentService = contentService;
            AutoSenderRunner = new BackgroundTaskRunner<IBackgroundTask>("EmailAutoSender", Logger);
        }

        public void Initialize()
        {
            DateTime today = DateTime.Now;
            DateTime nextDay = today.AddDays(1);
            DateTime dateWhen = (today.Hour < 6)
                                    ? new DateTime(today.Year, today.Month, today.Day, 6, 0, 0)
                                    : new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 6, 0, 0);

            TimeSpan whenStart = dateWhen - DateTime.Now;
            //v milivteřinách
            int delayBeforeWeStart = (int)whenStart.TotalMilliseconds;
            int howOftenWeRepeat = (int)new TimeSpan(24, 0, 0).TotalMilliseconds;

            var task = new StartSending(AutoSenderRunner, delayBeforeWeStart, howOftenWeRepeat, Runtime, Logger, ContentService);

            //As soon as we add our task to the runner it will start to run (after its delay period)
            AutoSenderRunner.TryAdd(task);
        }

        public void Terminate()
        {
        }
    }

    // Now we get to define the recurring task
    public class StartSending : RecurringTaskBase
    {
        private IRuntimeState Runtime;
        private IProfilingLogger Logger;
        private IContentService ContentService;

        public StartSending(IBackgroundTaskRunner<RecurringTaskBase> runner, int delayBeforeWeStart, int howOftenWeRepeat, IRuntimeState runtime, IProfilingLogger logger, IContentService contentService)
            : base(runner, delayBeforeWeStart, howOftenWeRepeat)
        {
            Runtime = runtime;
            Logger = logger;
            ContentService = contentService;
        }

        public override bool PerformRun()
        {
            if (DateTime.Now.Day == 10)
            {
                int amount = 0;
                string hmtlString = new TaurusEdeucation.Email_Sender.Emails().GetEmailHTMLBody("TaurusEdeucation.EmailSender.EmailsHtml.Deadline.html");

                Logger.Info<StartSending>("Sending Emails on " + DateTime.Now.Date);

                using (SqlConnection con = new SqlConnection(TaurusEdeucation.config.ConnectionStrings.UmbracoDbDSN))
                {
                    con.Open();
                    string sql = "SELECT * FROM LectorDatabaseTable";

                    SqlCommand command = new SqlCommand(sql, con);


                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        _ = new TaurusEdeucation.Email_Sender.EmailSender((string)reader["EMAIL"], "Upozornění na konec uzávěrky", hmtlString);
                        amount++;
                    }

                    con.Close();
                }
                Logger.Info<StartSending>("Sent " + amount + " emails.");
            }
            // If we want to keep repeating - we need to return true
            // But if we run into a problem/error & want to stop repeating - return false
            return true;
        }

        public override bool IsAsync => false;
    }
}