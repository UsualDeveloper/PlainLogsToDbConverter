# PlainLogsToDbConverter

Project that aims to help log analysis when there are only "plain old text logs" implemented, but a structured log information would help.

// TODO: usage tutorial


This is very early version of the project, features supported so far:
 - converting text logs into structured information inserted into SQL database table
 - automatic table creation
 - automatic columns creation for each detected field mentioned in the structured templates
 - conversion configuration using dedicated JSON file
 - single-line log entries support
