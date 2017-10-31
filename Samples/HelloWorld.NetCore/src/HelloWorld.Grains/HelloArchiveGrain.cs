using HelloWorld.Interfaces;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelloWorld.Lib1;
using SqlStreamStore;

namespace HelloWorld.Grains
{
    public class HelloArchiveGrain : Grain<GreetingArchive>, IHelloArchive
    {
        public async Task<string> SayHello(string greeting)
        {
            State.Greetings.Add(greeting);

            // reference some types
            // to force loading of the assemblies
            var t1 = new[]
            {
                typeof(SomeClassFromLib1),
                typeof(IStreamStore),
                typeof(MsSqlStreamStore)
            };

            await WriteStateAsync();

            return $"You said: '{greeting}', I say: Hello!";
        }

        public Task<IEnumerable<string>> GetGreetings()
        {
            return Task.FromResult<IEnumerable<string>>(State.Greetings);
        }
    }

    public class GreetingArchive
    {
        public List<string> Greetings { get; } = new List<string>();
    }
}
