using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.DataAccess.Models;
using Dex.DataAccess.Repository;
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

            GenerateIUnitOfWork(unitOfWorkPath, "Dex.DataAccess.UnitOfWork", entityTypes);
            GenerateUnitOfWork(unitOfWorkPath, "Dex.DataAccess.UnitOfWork", entityTypes);
            GenerateDTOsFromEntities(dtosPath, "Dex.Common.DTO", entityTypes);

            Console.WriteLine("All files generated successfully");
            Console.ReadKey();
        }

        private static void GenerateDTOsFromEntities(string path,string @namespace, IEnumerable<Type> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var dtoName = entityType.Name + "DTO";
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

        private static void GenerateIUnitOfWork(string path, string @namespace, IEnumerable<Type> entityTypes)
        {
            new FileBuilder()
                .DefineFile()
                    .Begin().WithPath(path).WithName("IUnitOfWork")
                        .WithInterface()
                        .Begin()
                            .WithNamespace(@namespace).WithUsingDirectives("Dex.DataAccess.Models", "Dex.DataAccess.Repository")
                            .WithAccessSpecifier(AccessSpecifier.Public).WithName("IUnitOfWork").WithAttributes()
                            .WithGenericArguments().WithGenericArgumentConstraint().WithInheritance()
                            .WithProperties(value => IUnitOfWorkPropertySequenceGenerator(value, entityTypes))
                            .WithMethod()
                                .Begin()
                                    .WithAccessSpecifier(AccessSpecifier.None).WithAccessModifier(AccessModifiers.None)
                                    .WithType(typeof(void)).WithName("SaveChanges").WithAttributes().WithGenericArguments()
                                    .WithGenericArgumentConstraint().WithParameters().WithEmptyBody()
                                .End()
                            .End()
                        .End()
                    .End()
                .End()
            .Build();
        }

        private static void GenerateUnitOfWork(string path, string @namespace, IEnumerable<Type> entityTypes)
        {
            new FileBuilder()
                .DefineFile()
                    .Begin().WithPath(path).WithName("UnitOfWork")
                        .WithClass()
                        .Begin()
                            .WithNamespace(@namespace).WithUsingDirectives("Dex.DataAccess.Models", "Dex.DataAccess.Repository")
                            .WithAccessSpecifier(AccessSpecifier.Public).WithClassType(ClassType.Partial).WithName("UnitOfWork").WithAttributes()
                            .WithGenericArguments().WithGenericArgumentConstraint().WithInheritance("IUnitOfWork")
                            .WithField()
                                .Begin()
                                    .WithAccessSpecifier(AccessSpecifier.Private).WithAccessModifier(AccessModifiers.Readonly)
                                    .WithType(typeof(DexContext)).WithName("_dbContext").WithAttributes().WithNoValue()
                                .End()
                            .WithMethod()
                                .Begin()
                                    .WithAccessSpecifier(AccessSpecifier.Public).WithAccessModifier(AccessModifiers.None)
                                    .WithType("UnitOfWork").WithName("").WithAttributes().WithGenericArguments().WithGenericArgumentConstraint()
                                    .WithParameters(Parameter.Standard(typeof(DexContext), "dbContext")).WithMethodBody("_dbContext = dbContext;")
                                .End()
                            .WithProperties(value => UnitOfWorkPropertySequenceGenerator(value, entityTypes))
                            .WithFields(value => UnitOfWorkFieldSequenceGenerator(value, entityTypes))
                            .WithMethod()
                                .Begin()
                                    .WithAccessSpecifier(AccessSpecifier.Public).WithAccessModifier(AccessModifiers.None)
                                    .WithType(typeof(void)).WithName("SaveChanges").WithAttributes().WithGenericArguments()
                                    .WithGenericArgumentConstraint().WithParameters().WithMethodBody("_dbContext.SaveChanges();")
                                .End()
                            .End()
                        .End()
                    .End()
                .End()
            .Build();
        }

        private static IEnumerable<IProperty<IClassBody>> UnitOfWorkPropertySequenceGenerator(Func<IProperty<IClassBody>> value, IEnumerable<Type> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var fieldName = $"_{entityType.Name.ToLowerFirstChar()}";

                var getBody = $@"return {fieldName} ??= new GenericRepository<{entityType.Name}>(_dbContext);";

                var type = typeof(IRepository<>).MakeGenericType(entityType);
                yield return value.Invoke()
                    .Begin()
                    .WithAccessSpecifier(AccessSpecifier.Public).WithAccessModifier(AccessModifiers.None)
                    .WithType(type).WithName(entityType.Name).WithAttributes()
                    .WithGetAccessSpecifier(AccessSpecifier.None).WithGetBody(getBody)
                    .WithSetAccessSpecifier(AccessSpecifier.None).NoSet()
                    .WithNoValue();
            }
        }

        private static IEnumerable<IProperty<IInterfaceBody>> IUnitOfWorkPropertySequenceGenerator(Func<IProperty<IInterfaceBody>> value, IEnumerable<Type> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var type = typeof(IRepository<>).MakeGenericType(entityType);
                yield return value.Invoke()
                    .Begin()
                    .WithAccessSpecifier(AccessSpecifier.None).WithAccessModifier(AccessModifiers.None)
                    .WithType(type).WithName(entityType.Name).WithAttributes()
                    .WithGetAccessSpecifier(AccessSpecifier.None).AutoGet()
                    .WithSetAccessSpecifier(AccessSpecifier.None).NoSet()
                    .WithNoValue();
            }
        }

        private static IEnumerable<IField> UnitOfWorkFieldSequenceGenerator(Func<IField> value, IEnumerable<Type> entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var type = typeof(IRepository<>).MakeGenericType(entityType);
                yield return value.Invoke()
                    .Begin()
                    .WithAccessSpecifier(AccessSpecifier.Private).WithAccessModifier(AccessModifiers.None)
                    .WithType(type).WithName($"_{entityType.Name.ToLowerFirstChar()}").WithAttributes()
                    .WithNoValue();
            }
        }
    }
}
