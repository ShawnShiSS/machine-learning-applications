This is applicable if Specification pattern is used.

The Specification pattern pulls query-specific logic out of the application.
In applications that abstract database query logic behind a Repository abstraction, the specification will typically eliminate the need for many custom Repository implementation classes as well as custom query methods on Repository implementations.

See more details: https://github.com/ardalis/Specification