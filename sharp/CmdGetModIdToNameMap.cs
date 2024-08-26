using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Olympus {
    public class CmdGetModIdToNameMap : Cmd<string, bool> {
        public override bool Taskable => true;

        private static string cacheLocation;

        public override bool Run(string cacheLocation) {
            CmdGetModIdToNameMap.cacheLocation = cacheLocation;
            Console.Error.WriteLine($"[CmdGetIdToNameMap] Cache location set to: {cacheLocation}");
            GetModIDsToNamesMap(ignoreCache: true);
            return true;
        }

        private static readonly object locker = new object();

        internal static Dictionary<string, string> GetModIDsToNamesMap(bool ignoreCache = false) {
            if (!ignoreCache && File.Exists(cacheLocation)) {
                Console.Error.WriteLine($"[CmdGetIdToNameMap] Loading mod IDs from {cacheLocation}");
                lock (locker) {
                    using (Stream inputStream = new FileStream(cacheLocation, FileMode.Open)) {
                        Dictionary<string, string> map = getModIDsToNamesMap(inputStream);
                        if (map.Count > 0) return map;
                    }
                }
            }

            using (HttpWebResponse res = Connect("https://maddie480.ovh/celeste/mod_ids_to_names.json"))
            using (Stream inputStream = res.GetResponseStream()) {
                Console.Error.WriteLine($"[CmdGetIdToNameMap] Loading mod IDs from maddie480.ovh");
                Dictionary<string, string> map = getModIDsToNamesMap(inputStream);
                lock (locker) {
                    if (map.Count > 0) File.WriteAllText(cacheLocation, JsonConvert.SerializeObject(map));
                    return map;
                }
            }
        }

        private static Dictionary<string, string> getModIDsToNamesMap(Stream inputStream) {
            try {
                using (TextReader textReader = new StreamReader(inputStream, Encoding.UTF8))
                using (JsonTextReader jsonTextReader = new JsonTextReader(textReader)) {
                    return new JsonSerializer().Deserialize<Dictionary<string, string>>(jsonTextReader);
                }
            } catch (Exception e) {
                Console.Error.WriteLine("Error loading mod IDs to names list");
                Console.Error.WriteLine(e);
                return new Dictionary<string, string>();
            }
        }
    }
}
