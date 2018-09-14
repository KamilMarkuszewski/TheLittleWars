using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ResourcesService : IService
    {
        public List<T> LoadScriptableObjects<T>(string path) where T : ScriptableObject
        {
            var loadedObjects = Resources.LoadAll(path, typeof(T));
            var scriptableObjectsList = loadedObjects.Select(i => i as T).Where(i => i != null).ToList();
            Debug.Log(string.Format("{0} objects loaded from path {1} ", scriptableObjectsList.Count, path));
            return scriptableObjectsList;
        }

        public T LoadScriptableObject<T>(string path) where T : ScriptableObject
        {
            var loadedObject = Resources.Load(path, typeof(T));
            Debug.Log(string.Format("object loaded from path {0} ", path));
            return loadedObject as T;
        }

        public List<AudioClip> LoadAudioClips(string path)
        {
            var loadedObjects = Resources.LoadAll(path, typeof(AudioClip));
            var loadAudioClips = loadedObjects.Select(i => i as AudioClip).Where(i => i != null).ToList();
            Debug.Log(string.Format("{0} objects loaded from path {1} ", loadAudioClips.Count, path));
            return loadAudioClips;
        }

        public AudioClip LoadAudioClip(string path)
        {
            var loadedObject = Resources.Load(path, typeof(AudioClip));
            Debug.Log(string.Format("object loaded from path {0} ", path));
            return loadedObject as AudioClip;
        }


        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
