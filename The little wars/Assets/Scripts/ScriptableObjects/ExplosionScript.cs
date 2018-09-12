using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Constants;
using Assets.Scripts.Helpers;
using Assets.Scripts.Scripts;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class ExplosionScript : MonoBehaviour
{
    private Sprite _explSprite;
    private void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            _explSprite = sr.sprite;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case Tags.Map:
                ColideWithMap(collision);
                break;

            case Tags.Unit:
                ColideWithUnit(collision);
                break;

            case Tags.Bullet:
                ColideWithBullet(collision);
                break;
        }
        Destroy(transform.parent.gameObject);
    }

    private void ColideWithMap(Collision2D collision)
    {
        DestroyMap(collision.collider.gameObject);
    }

    private void ColideWithUnit(Collision2D collision)
    {
        var moveScript = collision.collider.gameObject.GetComponent<CharacterMoveScript>();
        GameObjectsProviderHelper.GameModel.ChangeHp(moveScript.Unit, -50);
    }

    private void ColideWithBullet(Collision2D collision)
    {

    }

    private void DestroyMap(GameObject map)
    {
        var mapDestroyScript = map.GetComponent<DestroyScript>();
        if (mapDestroyScript != null)
        {
            Vector2 explosionCenter = new Vector2(transform.position.x, transform.position.y);
            mapDestroyScript.MergeWithMainTexture(explosionCenter, _explSprite);
        }
    }
}
