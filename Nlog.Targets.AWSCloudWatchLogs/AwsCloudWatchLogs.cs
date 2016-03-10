using System;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
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
        private string _logStream;

        [RequiredParameter]
        public string ProfilesLocation { get; set; }

        [RequiredParameter]
        public string ProfileName { get; set; }

        [RequiredParameter]
        public string LogGroupName { get; set; }

        [RequiredParameter]
        public string Region { get; set; }

        public string LogStreamNamePrefix { get; set; }

        public string LogStreamNameDateFormat { get; set; }

        public AwsCloudWatchLogs()
        {
            _sequenenceToken = null;
            LogStreamNameDateFormat = "yyyyMMddHHmmss";
        }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            InternalLogger.Debug("Initializing AWSCloudWatchLogs target");
            try
            {
                var credentials = new StoredProfileAWSCredentials(ProfileName, ProfilesLocation);
                _client = new AmazonCloudWatchLogsClient(credentials, RegionEndpoint.GetBySystemName(Region));

                _logStream = DateTime.UtcNow.ToString(LogStreamNameDateFormat);
                if (!string.IsNullOrWhiteSpace(LogStreamNamePrefix))
                    _logStream = string.Concat(LogStreamNamePrefix, "_", _logStream);

                _client.CreateLogStream(new CreateLogStreamRequest(LogGroupName, _logStream));

            }
            catch (Exception ex)
            {
                InternalLogger.Fatal("Failed to initialize target. Error is\n{0}\n{1}", ex.Message, ex.StackTrace);
            }

            InternalLogger.Debug("Initialized AWSCloudWatchLogs target");
        }

        protected override async void Write(AsyncLogEventInfo[] logEvents)
        {
            try
            {
                var request = new PutLogEventsRequest
                {
                    LogEvents = logEvents.ToInputLogEvents(Layout.Render),
                    LogGroupName = LogGroupName,
                    LogStreamName = _logStream,
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
