# PlainLogsToDbConverter
## General info

### Description
The project aim is to help "plain old text logs" analysis, by transforming log entries into more structured form and putting them into a database. User can configure values to be extracted from log text messages and placed into separate columns in the target table.

### Conversion
The conversion process implemented in the application can be divided into several steps:
 1. **Reading log entry from a text file**
 
 This can be any plain text log file, however, currently only single line log entries are supported (each line is treated like a separate log entry).

 2. **Matching log entry with any of the Regex patterns specified**
 
 Standard C# Regex pattern syntax is used. Patterns can match important values with separate named subexpressions.
 
 3. **Values extracted with named matched subexpressions are used to fill structured log template**
 
 Names of the named matched subexpressions are used to identify appropriate fields in the structured log template.
 
 4. **Structured log is inserted into a database table with appropriate values**
 
 Defined fields are inserted into separate table columns for better logs filtering.

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
Pattern matching uses standard C# Regex class pattern syntaxt. It's important to note that names of the named capture groups defined in the pattern are used to extract values for specific fields.
### Structured log template
When a match is found for specific named group, its name represents extracted field's name.
The matched group name and value are then used to fill target structured log template in the appropriate place.

## Feature summary
Features supported so far:
 - converting text logs into structured information inserted into SQL database table
 - automatic table creation
 - automatic columns creation for each detected field mentioned in the structured templates
 - conversion configuration using dedicated JSON file
 - single-line log entries support
