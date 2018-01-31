using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoContract
{
    class CliArgs
    {
        static readonly string[] Keys = new string[] { };

        static readonly char[] KVSeperator = new char[] { '=' };
        static readonly char[] DllPathSeperator = new char[] { ',' };

        public const string Usage = @"
Usage:

ProtoContract --dll_path={your_dll_path[,your_dll_path2]} --proto_path={the_out_proto_file_path} --java_package={the_proto_java_package_name}

For example:

ProtoContract --dll_path=D:/my_contract.dll --proto_path=my_contract.proto --java_package=com.ctrip.my_department.my_contract

Option shortcut:
    -i: --dll_path
    -o: --proto_path
    -p: --java_package

";

        public List<string> DllPaths { get; set; }

        public string ProtoPath { get; set; }

        public string JavaPackage { get; set; }

        public string Error { get; set; }

        public static CliArgs Parse(string[] args)
        {
            CliArgs cliArgs = new CliArgs();

            foreach (string part in args)
            {
                string[] kv = part.Split(KVSeperator, StringSplitOptions.RemoveEmptyEntries);
                if (kv.Length != 2)
                {
                    cliArgs.Error = "Invalid args: " + part;
                    return cliArgs;
                }

                string key = kv[0].ToLower().Trim();
                string value = kv[1].Trim();
                switch (key)
                {
                    case "-i":
                    case "--dll_path":
                        cliArgs.DllPaths = value.Split(DllPathSeperator, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                        break;
                    case "-o":
                    case "--proto_path":
                        cliArgs.ProtoPath = value;
                        break;
                    case "-p":
                    case "--java_package":
                        cliArgs.JavaPackage = value.ToLower();
                        break;
                    default:
                        break;
                }
            }

            if (cliArgs.DllPaths == null || cliArgs.DllPaths.Count == 0)
            {
                cliArgs.Error = "No args --dll_path";
                return cliArgs;
            }

            if (string.IsNullOrWhiteSpace(cliArgs.ProtoPath))
            {
                cliArgs.Error = "No args --proto_path";
                return cliArgs;
            }

            if (string.IsNullOrWhiteSpace(cliArgs.JavaPackage))
            {
                cliArgs.Error = "No args --java_package";
                return cliArgs;
            }

            return cliArgs;
        }

    }
}
