# AdvancedBuilder
Redesigning the idea of building Settings in a stub (QuasarRAT)

The idea:
Not having to deal with identifying properties by integers. Like QuasarRAT currently does:
```c#
strings++;
bools++;
switch()
```
But instead, having a mutual class and settings the properties like you normally would, then writing calls in the .ctor of the stub
```c#
class Settings
{
    public string My_cool_value { get; set; }
}

class Builder_program
{
    Settings settings = new Settings() {
        My_cool_value = "This string was injected"
    };
    DynamicBuilder<Settings>().Build(settings, "output.exe");
}
```

Current to-do:
* Add SET calls to .ctor, instead of simply replacing ldstr/ldc_i4_1 values. That way we can work with non-static Settings class, which don't already have calls in the .ctor
* Add SET calls to static classes (.cctor)
* A better way of parsing the mutual Settings class (???)

Currently does:
* Loads a stub and replaces the ldstr or ldc_i4 values in .ctor (non-static)

# Build requirements
* dnlib https://github.com/yck1509/dnlib