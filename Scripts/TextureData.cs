using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Texture")]
public class TextureData : ScriptableObject
{
    public List<Texture2D> list = new List<Texture2D>();
}
