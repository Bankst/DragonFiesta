using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using Microsoft.CSharp;

namespace DragonFiesta.Utils.IO.CS
{
	public class CSFile
	{
		public static MethodInfo LoadCode(String FilePath, String NameSpace, String MethodName)
		{
			String[] CSCodeLines = File.ReadAllLines(FilePath);

			String Source = "";

			foreach (String CSCodeLine in CSCodeLines) { Source += String.Concat(CSCodeLine, "\n"); }

			Dictionary<String, String> PO = new Dictionary<String, String>();
			PO.Add("CompilerVersion", "v3.5");

			CSharpCodeProvider CSCP = new CSharpCodeProvider(PO);

			CompilerParameters CP = new CompilerParameters()
			{ GenerateInMemory = false, GenerateExecutable = false };

			CompilerResults CR = CSCP.CompileAssemblyFromSource(CP, Source);

			if (CR.Errors.Count != 0) { Environment.Exit(0); }

			Object NSI = CR.CompiledAssembly.CreateInstance(NameSpace);

			MethodInfo Method = NSI.GetType().GetMethod(MethodName);

			return Method;
		}

		public static String[] LoadMethodNames(String FilePath, String NameSpace)
		{
			String[] CSCodeLines = File.ReadAllLines(FilePath);

			String Source = "";

			foreach (String CSCodeLine in CSCodeLines) { Source += String.Concat(CSCodeLine, "\n"); }

			Dictionary<String, String> PO = new Dictionary<String, String>();
			PO.Add("CompilerVersion", "v3.5");

			CSharpCodeProvider CSCP = new CSharpCodeProvider(PO);

			CompilerParameters CP = new CompilerParameters()
			{ GenerateInMemory = true, GenerateExecutable = false };

			CompilerResults CR = CSCP.CompileAssemblyFromSource(CP, Source);

			if (CR.Errors.Count != 0) { Environment.Exit(0); }

			Object NSI = CR.CompiledAssembly.CreateInstance(NameSpace);

			List<String> MethodNames = new List<String>();

			foreach (MethodInfo MI in NSI.GetType().GetMethods().Where(Name => Name.Name != "ToString" && Name.Name != "Equals" && Name.Name != "GetHashCode" && Name.Name != "GetType")) { MethodNames.Add(MI.Name); }

			return MethodNames.ToArray();
		}
	}
}
