using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    public class DestroyScript : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        // Use this for initialization
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void MergeWithMainTexture(Vector2 explosionCenter, Sprite explosionSprite)
        {
            var mainTxt = _spriteRenderer.sprite.texture;
            var tex2 = explosionSprite.texture;
            Color32[] mainPixels = mainTxt.GetPixels32();
            Color32[] tex2Pix = tex2.GetPixels32();

            var pixelsPerUnit = _spriteRenderer.sprite.pixelsPerUnit;

            Vector2 relativePosition = new Vector2(explosionCenter.x - transform.position.x, explosionCenter.y - transform.position.y);
            Vector2Int relativePositionInPixels = new Vector2Int((int)((relativePosition.x) * pixelsPerUnit), (int)((relativePosition.y) * pixelsPerUnit));

            int MainTextMidPoint = mainPixels.Length / 2;

            for (int j = 0; j < tex2.height; j++)
            {
                for (int i = 0; i < tex2.width; i++)
                {
                    var color = tex2Pix[i + j * tex2.width];
                    if (Math.Abs(color.a) > 0.1f)
                    {
                        int iRel = relativePositionInPixels.x + i - tex2.width / 2;
                        int jRel = relativePositionInPixels.y + j - tex2.height / 2;

                        if (iRel < -mainTxt.width/2) { iRel = -mainTxt.width / 2; }
                        if (iRel > mainTxt.width / 2) { iRel = mainTxt.width / 2; }
                        if (jRel < -mainTxt.height / 2) { jRel = -mainTxt.height / 2; }
                        if (jRel > mainTxt.height / 2) { jRel = mainTxt.height / 2; }


                        int position = MainTextMidPoint + iRel + jRel * mainTxt.width;

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

            var sp = Sprite.Create(photo, new Rect(-_spriteRenderer.sprite.rect.x, -_spriteRenderer.sprite.rect.y, photo.width, photo.height), new Vector2(0.5f, 0.5f));
            _spriteRenderer.sprite = sp;


            gameObject.AddComponent<PolygonCollider2D>();
            var col = GetComponent<PolygonCollider2D>();
            Destroy(col);
        }

    }
}
