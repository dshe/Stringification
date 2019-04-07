using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Stringification;

namespace TestStringifier
{
    public class AssemblyTest
    {
        protected readonly Action<string> Write;
        public AssemblyTest(ITestOutputHelper output) => Write = output.WriteLine;

        /*
        [Fact]
        public void T00_ThisAssembly()
        {
            var path = @"\prg\myProjects\GitHub\InterReact\InterReact\bin\debug\netstandard2.0\InterReact.dll";

            var list = new List<Type>();

            foreach (var type in GetTypes(path).Where(t => t.IsPublic && !t.IsAbstract && t.Namespace == "InterReact.Messages"))
            {
                var instance = type.GetTypeInfo().CreateInstance();
                if (instance == null)
                    list.Add(type);
                else
                    Write($"{instance.Stringify(includeTypeName: true)}");
            }

            Write($"\n\nCould not create instance of types:\n");
            foreach (var type in list)
                Write($"'{type.FullName}'");
        }
        */

        private static IEnumerable<Type> GetTypes(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            try
            {
                assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                //e.LoaderExceptions;
                return e.Types.Where(type => type != null);
            }
            throw new Exception();
        }

    }
}

