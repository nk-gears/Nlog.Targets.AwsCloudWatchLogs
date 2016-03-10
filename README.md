# Nlog.Targets.AwsCloudWatchLogs

Use like so in NLog.config : 

```
<target name="awslogs" type="AwsCloudWatchLogs" layout="${longdate} ${uppercase:${level}} ${message}"
              ProfilesLocation="X:\aws-profiles\credentials"
              ProfileName="profile-name"
              LogGroupName="/loggrouptest/TestNlogTarget"
              LogStreamName="Stream1"
              Region="eu-west"></target>
              
