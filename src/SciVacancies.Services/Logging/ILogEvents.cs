using System;
using System.Diagnostics.Tracing;

namespace SciVacancies.Services.Logging
{
    public interface ILogEvents
    {
        //todo: LOGGING_COMMENTED_OUT
        //#region General

        //[Event(100, Level = EventLevel.Error, Message = "Error during service call")]
        //void GeneralExceptionError(Exception e);

        //#endregion

        //#region Postgresql

        //[Event(200, Level = EventLevel.Error, Message = "Postgresql connection error")]
        //void PostgresqlConnectionError(Exception e);

        //#endregion

        //#region ElasticSearch

        //[Event(300, Level = EventLevel.Verbose, Message = "Elasticsearch connection error")]
        //void ElasticsearchConnectionError(Exception e);

        //#endregion

        //#region Quartz

        //[Event(400, Level = EventLevel.Error, Message = "Quartz scheduler error")]
        //void QartzSchedulerError(Exception e);

        //#endregion

        //#region Email

        //[Event(500, Level = EventLevel.Error, Message = "Email sending error")]
        //void EmailSendingError(Exception e);

        //#endregion
    }
}
