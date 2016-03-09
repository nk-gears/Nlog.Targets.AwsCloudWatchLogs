using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.CloudWatchLogs.Model;
using NLog;
using NLog.Common;

namespace Nlog.Targets.AWSCloudWatchLogs
{
    internal static class AwsCloudWatchLogsHelpers
    {
        public static InputLogEvent ToInputLogEvent(this LogEventInfo logEventInfo, Func<LogEventInfo, string> layout)
        {
            return new InputLogEvent
            {
                Message = layout(logEventInfo),
                Timestamp = DateTime.UtcNow
            };
        }

        public static InputLogEvent ToInputLogEvent(this AsyncLogEventInfo logEventInfo, Func<LogEventInfo, string> layout)
        {
            return new InputLogEvent
            {
                Message = layout(logEventInfo.LogEvent),
                Timestamp = DateTime.UtcNow
            };
        }

        public static List<InputLogEvent> ToInputLogEvents(this AsyncLogEventInfo[] logEventInfos, Func<LogEventInfo, string> layout)
        {
            return logEventInfos.Select(info => new InputLogEvent
            {
                Message = layout(info.LogEvent),
                Timestamp = DateTime.UtcNow
            }).ToList();
        }

    }
}