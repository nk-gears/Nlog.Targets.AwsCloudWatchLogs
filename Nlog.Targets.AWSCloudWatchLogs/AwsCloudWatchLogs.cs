using System;
using System.Collections.Generic;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace Nlog.Targets.AWSCloudWatchLogs
{
    [Target("AwsCloudWatchLogs")] 
    public sealed class AwsCloudWatchLogs : TargetWithLayout
    {
        private AmazonCloudWatchLogsClient _client;
        private string _sequenenceToken;

        [RequiredParameter]
        public string ProfilesLocation { get; set; }

        [RequiredParameter]
        public string ProfileName { get; set; }

        [RequiredParameter]
        public string LogStreamName { get; set; }

        [RequiredParameter]
        public string LogGroupName { get; set; }

        [RequiredParameter]
        public string Region { get; set; }

        public AwsCloudWatchLogs()
        {
            _sequenenceToken = null;
        }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            InternalLogger.Debug("Initializing AWSCloudWatchLogs target");
            try
            {
                var credentials = new StoredProfileAWSCredentials(ProfileName, ProfilesLocation);
                _client = new AmazonCloudWatchLogsClient(credentials, RegionEndpoint.GetBySystemName(Region));
            }
            catch (Exception ex)
            {
                InternalLogger.Fatal("Failed to initialize target. Error is\n{0}\n{1}", ex.Message, ex.StackTrace);
            }

            InternalLogger.Debug("Initialized AWSCloudWatchLogs target");
        }

        protected override async void Write(AsyncLogEventInfo logEvent)
        {
            try
            {
                var request = new PutLogEventsRequest
                {
                    LogEvents = new List<InputLogEvent>
                    {
                        logEvent.ToInputLogEvent(Layout.Render)
                    },
                    LogGroupName = LogGroupName,
                    LogStreamName = LogStreamName,
                    SequenceToken = _sequenenceToken
                }; 

                var result = await _client.PutLogEventsAsync(request);
                _sequenenceToken = result.NextSequenceToken;
            }
            catch (Exception ex)
            {
                InternalLogger.Fatal("Failed to write log to Amazon CloudWatch Logs with\n{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                var request = new PutLogEventsRequest
                {
                    LogEvents = new List<InputLogEvent>
                    {
                        logEvent.ToInputLogEvent(Layout.Render)
                    },
                    LogGroupName = LogGroupName,
                    LogStreamName = LogStreamName,
                    SequenceToken = _sequenenceToken
                };

                var result = _client.PutLogEvents(request);
                _sequenenceToken = result.NextSequenceToken;
            }
            catch (Exception ex)
            {
                InternalLogger.Fatal("Failed to write log to Amazon CloudWatch Logs with\n{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        protected override async void Write(AsyncLogEventInfo[] logEvents)
        {
            try
            {
                var request = new PutLogEventsRequest
                {
                    LogEvents = logEvents.ToInputLogEvents(Layout.Render),
                    LogGroupName = LogGroupName,
                    LogStreamName = LogStreamName,
                    SequenceToken = _sequenenceToken
                };

                var result = await _client.PutLogEventsAsync(request);
                _sequenenceToken = result.NextSequenceToken;
            }
            catch (Exception ex)
            {
                InternalLogger.Fatal("Failed to write logs to Amazon CloudWatch Logs with\n{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
