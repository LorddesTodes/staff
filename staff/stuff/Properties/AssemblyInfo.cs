using MelonLoader;
using PMAPI;

[assembly: MelonInfo(typeof(stuff.stuff), "stuff", "1.0.0", "leon")]
[assembly: MelonGame("PrimitierDev", "Primitier")]
[assembly: MelonAdditionalDependencies("PMAPI")]
[assembly: MelonPriority(1)]
[assembly: PMAPIMod("stuff")]
