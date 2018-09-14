using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapDestroyScript : MonoBehaviour
    {
        #region Services

        private DestructibleTerrainService DestructibleTerrainService
        {
            get { return ServiceLocator.GetService<DestructibleTerrainService>(); }
        }


        #endregion

        private SpriteRenderer _spriteRenderer;

        // Use this for initialization
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void MergeWithMainTexture(Vector2 explosionCenter, Sprite explosionSprite)
        {
            DestructibleTerrainService.MergeWithMainTexture(explosionCenter, explosionSprite, _spriteRenderer, gameObject);
        }
    }
}
