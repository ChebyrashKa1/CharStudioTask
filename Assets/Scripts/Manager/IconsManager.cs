using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconsManager : Singleton<IconsManager>
{
    [SerializeField] private Sprite[] sprites;

    public Sprite GetUserIcon()
    {
        var random = Random.Range(0, sprites.Length);
        return sprites[random];
    }
}
