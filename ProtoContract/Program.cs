using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;
using ProtoBuf.Meta;
using System.Reflection;
using System.IO;

namespace ProtoContract
{
    class Program
    {
        static void Main(string[] args)
        {
            CliArgs cliArgs = CliArgs.Parse(args);
            if (!string.IsNullOrWhiteSpace(cliArgs.Error))
            {
                Console.WriteLine();
                Console.WriteLine(cliArgs.Error);
                Console.WriteLine(CliArgs.Usage);
                return;
            }

            try
            {
                List<Assembly> assemblies = LoadAssemblies(cliArgs);

                RuntimeTypeModel runtimeTypeModel = Create(false);

                AddContract(runtimeTypeModel, assemblies);

                GenerateContract(runtimeTypeModel, cliArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine();
                Console.WriteLine(ex);
            }
        }

        static List<Assembly> LoadAssemblies(CliArgs args)
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string path in args.DllPaths)
            {
                try
                {
                    assemblies.Add(Assembly.LoadFrom(path));
                }
                catch (Exception ex)
                {
                    throw new Exception("Load dll failed: " + path, ex);
                }
            }

            return assemblies;
        }

        static RuntimeTypeModel Create(bool inferTagFromName)
        {
            RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Create();
            runtimeTypeModel.InferTagFromNameDefault = inferTagFromName;
            return runtimeTypeModel;
        }

        static void AddContract(RuntimeTypeModel runtimeTypeModel, List<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(ProtoContractAttribute), false).Any())
                    {
                        runtimeTypeModel.Add(type, true);
                    }
                }
            }
        }

        static void GenerateContract(RuntimeTypeModel runtimeTypeModel, CliArgs args)
        {
            string schema = GetSchema(runtimeTypeModel, args);
            File.WriteAllText(args.ProtoPath, schema);
        }

        static string GetSchema(RuntimeTypeModel runtimeTypeModel, CliArgs args)
        {
            BasicList requiredTypes = new BasicList();

            foreach (MetaType meta in runtimeTypeModel.GetTypes())
            {
                MetaType tmp = meta.GetSurrogateOrBaseOrSelf(false);
                if (!requiredTypes.Contains(tmp))
                {
                    requiredTypes.Add(tmp);
                }
            }

            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("syntax = \"proto2\";").AppendLine();
            headerBuilder.AppendLine();

            headerBuilder.Append("option java_package = \"").Append(args.JavaPackage).Append("\";").AppendLine();
            headerBuilder.Append("option java_multiple_files = true;").AppendLine();

            bool requiresBclImport = false;
            StringBuilder bodyBuilder = new StringBuilder();
            // sort them by schema-name
            MetaType[] metaTypesArr = new MetaType[requiredTypes.Count];
            requiredTypes.CopyTo(metaTypesArr, 0);
            Array.Sort(metaTypesArr, MetaType.Comparer.Default);

            // write the messages
            for (int i = 0; i < metaTypesArr.Length; i++)
            {
                MetaType tmp = metaTypesArr[i];
                if (tmp.IsList) continue;
                tmp.WriteSchema(bodyBuilder, 0, ref requiresBclImport);
                bodyBuilder.AppendLine();
            }

            if (requiresBclImport)
            {
                headerBuilder.AppendLine();
                headerBuilder.Append("import \"dotnettype.proto\";").AppendLine();
            }

            return headerBuilder.Append(bodyBuilder).ToString();
        }

    }
}
