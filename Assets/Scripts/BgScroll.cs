using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour
{
    public Renderer meshRender;
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        // Vector2 offset = meshRender.material.mainTextureOffset;
        // offset = offset + new Vector2(0, speed * Time.deltaTime);
        // meshRender.material.mainTextureOffset = offset;

        meshRender.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
