# PlainLogsToDbConverter

## Description
The project aims to help log analysis when there are only "plain old text logs" implemented, but a structured log information would help.

The application read logs from plain text file and convert them into records inserted into a database table.
The conversion process can be configured to extract specific parts of text logs as fields and save their values into separate columns in the target table.

## How to use
The logs conversion process is configured with *log-conversion-config.json* file.
Sample file is included in the application repository.
Configuration parameters:
 - `InputLogFilePath` - path to the file to be processed
 - `ConnectionString` - connection string to the target MS SQL Server database
 - `LogTableName` - name of the table where the logs should be inserted - if the table does not exist, it will be created automatically
 - `LogRegexTemplates` - array of log matching patterns matching and templates for structurized logs to be saved into a database. Log patterns and templates are described more thoroughly in the next section.

## Log matching patterns and structured log templates
### Matching pattern
Pattern matching uses standard C# Regex class pattern syntaxt. One important thing to note is additional function assigned to named capture groups, which are used to extract values for specific fields. Groups captured using defined Regex pattern are matched by name with fields defined in structured log templates.
### Structured log template
When a match is found for specific named group, its name represents extracted field's name.
The matched group name and value are then used to fill target structured log template in the appropriate place.

## Features summary
This is very early version of the project, features supported so far:
 - converting text logs into structured information inserted into SQL database table
 - automatic table creation
 - automatic columns creation for each detected field mentioned in the structured templates
 - conversion configuration using dedicated JSON file
 - single-line log entries support
