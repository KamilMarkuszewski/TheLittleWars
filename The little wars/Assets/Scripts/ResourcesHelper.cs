using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ResourcesHelper
    {
        public static List<T> LoadScriptableObjects<T>(string path) where T : ScriptableObject
        {
            var loadedObjects = Resources.LoadAll(path, typeof(T));
            var scriptableObjectsList = loadedObjects.Select(i => i as T).Where(i => i != null).ToList();
            Debug.Log(string.Format("{0} objects loaded from path {1} ", scriptableObjectsList.Count, path));
            return scriptableObjectsList;
        }

        public static T LoadScriptableObject<T>(string path) where T : ScriptableObject
        {
            var loadedObject = Resources.Load(path, typeof(T));
            Debug.Log(string.Format("object loaded from path {0} ", path));
            return loadedObject as T;
        }

    }
}
