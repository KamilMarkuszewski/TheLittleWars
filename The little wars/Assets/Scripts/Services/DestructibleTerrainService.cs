using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class DestructibleTerrainService : IService
    {



        public void MergeWithMainTexture(Vector2 explosionCenter, Sprite explosionSprite, SpriteRenderer spriteRenderer, GameObject mapGameObject)
        {
            var mainTxt = spriteRenderer.sprite.texture;
            var tex2 = explosionSprite.texture;
            Color32[] mainPixels = mainTxt.GetPixels32();
            Color32[] tex2Pix = tex2.GetPixels32();

            var pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;

            Vector2 relativePosition = new Vector2(explosionCenter.x - mapGameObject.transform.position.x, explosionCenter.y - mapGameObject.transform.position.y);
            Vector2Int relativePositionInPixels = new Vector2Int((int)(relativePosition.x * pixelsPerUnit), (int)(relativePosition.y * pixelsPerUnit));

            int mainTextMidPoint = mainPixels.Length / 2;

            for (int j = 0; j < tex2.height; j++)
            {
                for (int i = 0; i < tex2.width; i++)
                {
                    var color = tex2Pix[i + j * tex2.width];
                    if (Math.Abs(color.a) > 0.1f)
                    {
                        int iRel = relativePositionInPixels.x + i - tex2.width / 2;
                        int jRel = relativePositionInPixels.y + j - tex2.height / 2;

                        if (iRel < -mainTxt.width / 2) { iRel = -mainTxt.width / 2; }
                        if (iRel > mainTxt.width / 2) { iRel = mainTxt.width / 2; }
                        if (jRel < -mainTxt.height / 2) { jRel = -mainTxt.height / 2; }
                        if (jRel > mainTxt.height / 2) { jRel = mainTxt.height / 2; }


                        int position = mainTextMidPoint + iRel + jRel * mainTxt.width;

                        if (position > 0 && position < mainPixels.Length)
                        {
                            mainPixels[position] = new Color(0, 0, 0, 0);
                        }
                    }
                }
            }

            Texture2D photo = new Texture2D(mainTxt.width, mainTxt.height);
            photo.SetPixels32(mainPixels);
            photo.Apply();

            var sp = Sprite.Create(photo, new Rect(-spriteRenderer.sprite.rect.x, -spriteRenderer.sprite.rect.y, photo.width, photo.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sp;


            mapGameObject.AddComponent<PolygonCollider2D>();
            var col = mapGameObject.GetComponent<PolygonCollider2D>();
            UnityEngine.Object.Destroy(col);
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
