using DragonFiesta.Networking.HandlerTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonFiesta.Networking
{
    public class NetworkReflector
    {
        public static IEnumerable<Type> GetHandlerTypes()
        {
            return (Reflector.Global.Assemblies.SelectMany(t => t.GetTypes()).Where(t => t.IsClass && t.IsSubclassOf(typeof(HandlerType))));
        }

        public static Dictionary<byte, Dictionary<UInt16, string>> GetAvabileOpcodes()
        {
            Dictionary<byte, Dictionary<UInt16, string>> toRet = new Dictionary<byte, Dictionary<UInt16, string>>();

            foreach (var AssemblyType in GetHandlerTypes())
            {
                var values = AssemblyType.GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (var s in values)
                {
                    if (s.Name == "_Header") continue;  //except because is handlet manuel alredy

                    try
                    {
                        byte Header = (byte)AssemblyType.GetField("_Header", BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Static)?.GetValue(0);
                        var Field = s.GetValue(null);
                        if (Field is ushort pHandlerType)
                        {
                            string opcodeNames = s.ToString().Split(' ')?[1];

                            if (toRet.TryGetValue(Header, out Dictionary<ushort, string> mM))
                            {
                                if (!mM.ContainsKey(pHandlerType) && opcodeNames != "_header")
                                {
                                    mM.Add(pHandlerType, opcodeNames);
                                }
                            }
                            else
                            {
                                toRet.Add(Header, new Dictionary<UInt16, string>());
                                toRet[Header].Add(pHandlerType, opcodeNames);
                            }
                        }
                        else
                        {
                            EngineLog.Write(EngineLogLevel.Exception, "invalid opcodeType for {0}", s.Name);
                        }
                    }
                    catch
                    {
                        EngineLog.Write(EngineLogLevel.Exception, "Can find _Header for {0}", AssemblyType.Name);
                        break;
                    }
                }
            }
            return toRet;
        }

        //Header ClientRegion type methode

        public static Dictionary<byte, Dictionary<ushort, Dictionary<ClientRegion, MethodInfo>>> GivePacketMethods()
        {
            IEnumerable<Tuple<byte, PacketHandlerAttribute[], MethodInfo>> Handlers = (from PacketHandler in Reflector.Global.GetTypesWithAttribute<PacketHandlerClassAttribute>()
                                                                                       from Method in PacketHandler.Item2.GetMethods()
                                                                                       let Handler = Attribute.GetCustomAttributes(Method, typeof(PacketHandlerAttribute)) as PacketHandlerAttribute[]
                                                                                       select new Tuple<byte, PacketHandlerAttribute[], MethodInfo>(PacketHandler.Item1.Header, Handler.ToArray(), Method));

            var toRet = new Dictionary<byte, Dictionary<ushort, Dictionary<ClientRegion, MethodInfo>>>();

            foreach (var HeaderType in Handlers)
            {
                byte Header = HeaderType.Item1;

                if (!toRet.ContainsKey(Header)) //Create List for this header
                    toRet.Add(Header, new Dictionary<ushort, Dictionary<ClientRegion, MethodInfo>>());

                foreach (var Handler in HeaderType.Item2)
                {
                    if (!toRet[Header].ContainsKey(Handler.Type))
                        toRet[Header].Add(Handler.Type, new Dictionary<ClientRegion, MethodInfo>());

                    if (!toRet[Header][Handler.Type].ContainsKey(Handler.Region))
                        toRet[Header][Handler.Type].Add(Handler.Region, HeaderType.Item3);
                    else
                    {
                        EngineLog.Write(EngineLogLevel.Warning, "Dublicate Handler Found Region : {0} Header : {1} Type : {2} found !", Handler.Region, Header, Handler.Type);
                        continue;
                    }
                }
            }
            return toRet;
        }
    }
}