using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.DataAccess.Models;
using FluentGeneration;
using FluentGeneration.Extensions;
using FluentGeneration.Interfaces.Class;
using FluentGeneration.Interfaces.Field;
using FluentGeneration.Interfaces.Interface;
using FluentGeneration.Interfaces.Property;
using FluentGeneration.Shared;

namespace Dex.Generation
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("Press enter to begin");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Enter);

            using var db = new DexContext("Server=localhost\\SQLEXPRESS;Database=Dex_Main;Trusted_Connection=True;");
            var entityTypes = db.Model.GetEntityTypes().Select(t => t.ClrType)
                .Where(t => t.Namespace != null && t.Namespace.Contains("DataAccess")).ToList();

            var solutionDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"));
            var dataAccessPath = solutionDir + @"Dex.DataAccess\";
            var commonPath = solutionDir + @"Dex.Common\";

            var unitOfWorkPath = dataAccessPath + @"UnitOfWork\";
            var dtosPath = commonPath + @"DTO\";

            GenerateDTOsFromEntities(dtosPath, "Dex.Common.DTO", entityTypes);

            Console.WriteLine("All files generated successfully");
            Console.ReadKey();
        }

        private static void GenerateDTOsFromEntities(string path,string @namespace, IEnumerable<Type> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var dtoName = entityType.Name + "DTO";
                var newFile = Path.Combine(path, dtoName + ".cs");
                if (File.Exists(newFile))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($@"{newFile} already exists, do you want to override it? Y/N");
                    var key = Console.ReadKey();
                    if (key.Key != ConsoleKey.Y)
                    {
                        Console.WriteLine($@"{newFile} was not overriden");
                        continue;
                    }

                    Console.WriteLine($@"{newFile} was overriden");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                new FileBuilder()
                    .DefineFile()
                        .Begin().WithPath(path).WithName(dtoName)
                            .WithClass()
                            .Begin()
                                .WithNamespace(@namespace).WithUsingDirectives()
                                .WithAccessSpecifier(AccessSpecifier.Public).WithClassType(ClassType.Standard).WithName(dtoName)
                                .WithAttributes().WithGenericArguments().WithGenericArgumentConstraint().WithInheritance()
                                .WithProperties(value => DTOPropertySequenceGenerator(value, entityType, entityTypes))
                            .End()
                        .End()
                    .End()
                .End()
                .Build();
            }
        }

        private static IEnumerable<IProperty<IClassBody>> DTOPropertySequenceGenerator(Func<IProperty<IClassBody>> value, Type entityType, IEnumerable<Type> entityTypes)
        {
            foreach (var property in entityType.GetProperties())
            {
                yield return value.Invoke()
                    .Begin()
                    .WithAccessSpecifier(AccessSpecifier.Public).WithAccessModifier(AccessModifiers.None)
                    .WithType(ReplaceIfEntityType(property.PropertyType)).WithName(property.Name).WithAttributes()
                    .WithGetAccessSpecifier(AccessSpecifier.None).AutoGet()
                    .WithSetAccessSpecifier(AccessSpecifier.None).AutoSet()
                    .WithNoValue();
            }

            string ReplaceIfEntityType(Type type)
            {
                var name = type.FormatTypeName(false);
                foreach (var entityName in entityTypes.Select(et => et.Name))
                {
                    var pattern = $@"\b{entityName}\b";
                    name = Regex.Replace(name, pattern, entityName + "DTO");
                }

                return name;
            }
        }
    }
}
