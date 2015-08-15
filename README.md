# Utils.NET (UtilsDotNet)

Light weight collection of utilities for .NET projects

* _ArgumentParser_
* _Logging_
* _T4 Templates Base_
* _Process helper_


[![Build status](https://ci.appveyor.com/api/projects/status/d6tdpjcbwcrgd68f?svg=true)](https://ci.appveyor.com/project/nayanshah/utilsdotnet)

## Argument Parser

##### Parse & validate arguments or print usage with a single line

* Declarative syntax
 - Properties can be primitive types (``string``, ``int``, ``bool``, etc) or any ``Enum`` or collections (``IEnumerable<string>``, ``IEnumerable<Enum>``, etc) 
 - Arguments are required unless ``[Optional]`` is specified
 - Default values can be specified, e.g. ``[Optional("bot")``, ``[Optional("Saturday,Sunday")]``

* Flexible parsing
  - Supports nearly all variations =>  ``/arg value``, ``/arg:value``, ``/arg=value``, ``-arg value``, ``--arg value``, ``--arg=value``
  - Collections can be space, semicolon or comma separated

* Validation
 - Checks if arguments match expected types
 - Error if required argument is null or 
 - Automatically print help text on ``/h``, ``/?``, ``-h``, ``--help``


##### Example

```csharp
    [Details("Description of the tool")]
    public class Parameters
    {
        [Param(key: "p", longKey: "project")]
        [Details("Project name. e.g. utils")]
        public string Project { get; set; }

        [Param(longKey: "mode")]
        [Details("Enums are automatically parsed")]
        public Mode Mode { get; set; }

        [Param(key: "u", longKey: "user")]
        [Details("User name. Optional and has default value")]
        [Optional("bot")]
        public string User { get; set; }

        [Param(ArgumentType.ParamArray, longKey: "run")]
        [Details("IEnumberable collection for ParamArray arguments")]
        [Optional]
        public IEnumerable<string> Tasks { get; set; }

        [Param(ArgumentType.Flag, "v")]
        [Details("Boolean flags")]
        [Optional]
        public bool Verbose { get; set; }
    }
}
```

```csharp
    public static class Program
    {
        public static int Main(string[] args)
        {
          Parameters parameters = ArgumentParser<Parameters>.Parse();
          if(parameters == null) {
            ArgumentParser<Parameters>.PrintUsage();
            return -1;
          }
          
          Console.WriteLine("Hello from {0}", parameters.User");
          return 0;
        }
    }
}
```

Automatically generated help text

```
Description of the tool

  -p             --project                Project name. e.g. utils
                 --mode                   Enums are automatically parsed

  -h             --help                   Prints this help text (Optional)
  -u             --user                   User name. Optional and has default value (Optional)
                 --run                    IEnumberable collection for ParamArray arguments (Optional)
  -v                                      Boolean flags (Optional)
```


