using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.Services.Interfaces;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SoundService : IService
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private ResourcesService ResourcesService
        {
            get { return ServiceLocator.GetService<ResourcesService>(); }
        }

        #endregion

        private ObjectPool<AudioSource> _audioSourcesPool;
        private List<AudioClip> _loadedAudioClips;

        public AudioSource GetAudioSourceFromPool()
        {
            return _audioSourcesPool.GetObject();
        }

        public void PlayClip(AudioClipsEnum clip)
        {
            PlayClip(GetClip(clip));
        }

        public void PlayClip(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource audioSource = _audioSourcesPool.GetObject();
                audioSource.clip = clip;
                audioSource.Play();
                int clipLength = (int)(clip.length * 100);

                PutBackToPool(audioSource, clipLength);
            }
        }

        public void StopPlaying(AudioSource audioSource)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        public void PlayClip(AudioSource audioSource, AudioClipsEnum clip)
        {
            if (audioSource != null)
            {
                audioSource.clip = GetClip(clip);
                audioSource.Play();
            }
        }

        private AudioClip GetClip(AudioClipsEnum clip)
        {
            return _loadedAudioClips.First(c => c.name == clip.ToString());
        }

        private void PutBackToPool(AudioSource freeAudioSource, int clipLength)
        {
            BackgroundWorker invoker = new BackgroundWorker();
            invoker.DoWork += (sender, args) =>
            {
                Thread.Sleep(clipLength);
                _audioSourcesPool.PutObject(freeAudioSource);
            };
            invoker.RunWorkerAsync();
        }

        private AudioSource AudioSourceGenerator()
        {
            var newGameObject = new GameObject("AudioSource", typeof(AudioSource));
            newGameObject.transform.parent = GameObjectsProviderService.AudioSourcesGameObject.transform;
            return newGameObject.GetComponent<AudioSource>();
        }

        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            _audioSourcesPool = new ObjectPool<AudioSource>(AudioSourceGenerator);
            _loadedAudioClips = ResourcesService.LoadAudioClips(ResourcesPaths.Sounds);

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
