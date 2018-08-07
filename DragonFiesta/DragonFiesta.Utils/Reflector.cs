using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

//TODO Make it better
public class Reflector
{
    public static Reflector Global
    {
        get
        {
            return _Global ?? (_Global = new Reflector(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("DragonFiesta.")).ToArray()));
        }
    }

    private static Reflector _Global;

    public Assembly[] Assemblies { get; private set; }

    public Reflector(params Assembly[] Assemblies)
    {
        this.Assemblies = Assemblies;
    }

    public Type[] GetTypes()
    {
        return (from type in Assemblies.SelectMany(a => a.GetTypes())
                select type).ToArray();
    }

    public MethodInfo[] GetMethods()
    {
        return (from method in GetTypes().SelectMany(t => t.GetMethods())
                select method).ToArray();
    }

    public Pair<pAttribute, Type>[] GetTypesWithAttribute<pAttribute>()
           where pAttribute : Attribute
    {
        return (from type in GetTypes()
                let attr = Attribute.GetCustomAttribute(type, typeof(pAttribute)) as pAttribute
                where attr != null
                select new Pair<pAttribute, Type>(attr, type)).ToArray();
    }

    public Pair<pAttribute, MethodInfo>[] GetMethodsWithAttribute<pAttribute>()
    where pAttribute : Attribute
    {
        return (from method in GetMethods()
                let attr = Attribute.GetCustomAttribute(method, typeof(pAttribute)) as pAttribute
                where attr != null
                select new Pair<pAttribute, MethodInfo>(attr, method)).ToArray();
    }

    public static Pair<List<pAttribute>, MethodInfo>[] GetMethodsFromTypeWithAttributes<pAttribute>(Type pType)
        where pAttribute : Attribute
    {
        return (from method in pType.GetMethods()
                let attr = (Attribute.GetCustomAttributes(method, typeof(pAttribute)) as pAttribute[]).ToList()
                where attr != null
                select new Pair<List<pAttribute>, MethodInfo>(attr, method)).ToArray();
    }

    public static IEnumerable<Func<bool>> GetInitializerServerMethods(ServerType InitType)
    {
        return (from type in Global.GetTypes()
                let serverModuleAttribute = (Attribute.GetCustomAttributes(type, typeof(ServerModuleAttribute)) as ServerModuleAttribute[]).FirstOrDefault(m => m.InitialType == InitType)
                where serverModuleAttribute != null
                from method in type.GetMethods()
                let initMethodAttribute = Attribute.GetCustomAttribute(method, typeof(InitializerMethodAttribute)) as InitializerMethodAttribute
                where initMethodAttribute != null
                orderby serverModuleAttribute.InitializationStage ascending
                select (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method));
    }

    public static IEnumerable<Func<bool>> GetInitializerGameServerMethods(ServerType InitType)
    {
        return (from type in Global.GetTypes()
                let serverModuleAttribute = (Attribute.GetCustomAttributes(type, typeof(GameServerModuleAttribute)) as ServerModuleAttribute[]).FirstOrDefault(m => m.InitialType == InitType)
                where serverModuleAttribute != null
                from method in type.GetMethods()
                let initMethodAttribute = Attribute.GetCustomAttribute(method, typeof(InitializerMethodAttribute)) as InitializerMethodAttribute
                where initMethodAttribute != null
                orderby serverModuleAttribute.InitializationStage ascending
                select (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method));
    }

    public static IEnumerable<Func<bool>> GetInitializerGameMethods(ServerType InitType)
    {
        return (from assembly in AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.GlobalAssemblyCache)
                from type in assembly.GetTypes()
                let serverModuleAttribute = (Attribute.GetCustomAttributes(type, typeof(GameServerModuleAttribute)) as GameServerModuleAttribute[]).FirstOrDefault(m => m.InitialType == InitType)
                where serverModuleAttribute != null
                from method in type.GetMethods()
                let initMethodAttribute = Attribute.GetCustomAttribute(method, typeof(InitializerMethodAttribute)) as InitializerMethodAttribute
                where initMethodAttribute != null
                orderby serverModuleAttribute.InitializationStage ascending
                select (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method));
    }



    public static IEnumerable<Action> GetServerShutdownMethods()
    {
        return (from method in Global.GetMethodsWithAttribute<ShutdownAttribute>()
                where method != null
                orderby method.First.Type descending
                select (Action)Delegate.CreateDelegate(typeof(Action), method.Second));
    }

    public static Dictionary<string, Dictionary<string, MethodInfo>> GiveCategoryConsoleMethods()
    {
        IEnumerable<Pair<string, Pair<List<ConsoleCommandAttribute>, MethodInfo>>> methods = from t in Global.GetTypesWithAttribute<ConsoleCommandCategory>()
                                                                                             from m in GetMethodsFromTypeWithAttributes<ConsoleCommandAttribute>(t.Item2)
                                                                                             where m != null
                                                                                             select new Pair<string, Pair<List<ConsoleCommandAttribute>, MethodInfo>>(t.Item1.Category, m);

        var toRet = new Dictionary<string, Dictionary<string, MethodInfo>>();
        foreach (var pair in methods)
        {
            string Category = pair.First.ToUpper();

            if (!toRet.ContainsKey(Category))
                toRet.Add(Category, new Dictionary<string, MethodInfo>());

            foreach (var CommandVariation in pair.Item2.Item1)
            {
                string cmd = CommandVariation.Command.ToUpper();

                if (!toRet[Category].ContainsKey(cmd))
                    toRet[Category].Add(cmd, pair.Second.Second);
            }
        }

        return toRet;
    }

    public static IEnumerable<Type> GiveServerTasks() =>
        from type in Global.GetTypes()
        let attr = Attribute.GetCustomAttribute(type, typeof(ServerTaskClass)) as ServerTaskClass
        where attr != null
        select type;


}