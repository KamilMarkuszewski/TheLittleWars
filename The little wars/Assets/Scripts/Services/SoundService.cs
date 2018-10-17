using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SoundService : IService
    {
        #region Services

        private ResourcesService ResourcesService
        {
            get { return ServiceLocator.GetService<ResourcesService>(); }
        }

        private ObjectPoolingService ObjectPoolingService
        {
            get { return ServiceLocator.GetService<ObjectPoolingService>(); }
        }

        #endregion


        private List<AudioClip> _loadedAudioClips;

        public AudioSource GetAudioSourceFromPool()
        {
            return ObjectPoolingService.AudioSourcesPool.GetObject();
        }

        public void PlayClip(AudioClipsEnum clip)
        {
            PlayClip(GetClip(clip));
        }

        public void PlayClip(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource audioSource = ObjectPoolingService.AudioSourcesPool.GetObject();
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
                ObjectPoolingService.AudioSourcesPool.PutObject(freeAudioSource);
            };
            invoker.RunWorkerAsync();
        }



        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;


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
