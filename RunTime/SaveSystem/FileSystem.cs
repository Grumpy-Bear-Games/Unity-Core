using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Games.GrumpyBear.Core.SaveSystem
{
    public static class FileSystem
    {
        private static readonly IFormatter  _formatter;
        
        static FileSystem () {
            var surrogateSelector = new SurrogateSelector();
            
            surrogateSelector.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3Surrogate()
            );
            _formatter = new BinaryFormatter {SurrogateSelector = surrogateSelector};
        }
        
        public static void Delete(string saveFile) => File.Delete(GetPathFromSaveFile(saveFile));

        public static bool Exists(string saveFile) => File.Exists(GetPathFromSaveFile(saveFile));
        
        public static Dictionary<string, Dictionary<string, object>> LoadFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, Dictionary<string, object>>();
            }

            using var stream = File.Open(path, FileMode.Open);
            return (Dictionary<string, Dictionary<string, object>>)_formatter.Deserialize(stream);
        }        
        
        public static void SaveFile(string saveFile, Dictionary<string, Dictionary<string, object>> state)
        {
            var path = GetPathFromSaveFile(saveFile);
            using var stream = File.Open(path, FileMode.Create);
            _formatter.Serialize(stream, state);
        }
        
        private static string GetPathFromSaveFile(string saveFile) => Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        
    }
}
