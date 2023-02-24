# Nexogen Logger

## Usage

The Logger class has 3 types of constructors:

#### Logger()  - Log to Console

#### Logger(String Path) - log to file. If Path is empy, logging to working directory
Exceptions:
- ArgumentNullException
### Logger(Stream Stream - log to Stream
Exceptions:
- ArgumentNullException

## Logging methods:

- debug(string message)
- info(string message)
- error(string message)

All three Logs throw a LogMessage Exception if the Log message is longer than 1000 characters.



