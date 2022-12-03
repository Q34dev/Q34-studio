using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private SpriteRenderer sRenderer;

    private float length;
    private Transform cam;
    [SerializeField] private float parallaxEffect;

    private bool fadingOut, fadingIn;

    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        cam = Camera.main.transform;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        fadingOut = false;
        fadingIn = false;
    }

    void Update()
    {
        float temp = Vector2.Distance(transform.position, cam.position);

        if (temp > length)
            transform.position += new Vector3(length * 2, 0, 0);
        else
            transform.position -= new Vector3(4f * Time.deltaTime * parallaxEffect, 0, 0);

        if (fadingOut && sRenderer.color.a > 0)
        {
            SetAlpha(sRenderer.color.a - Time.deltaTime);
        }
        else if (fadingOut)
        {
            fadingOut = false;
        }

        if (fadingIn && sRenderer.color.a < 1)
        {
            SetAlpha(sRenderer.color.a + Time.deltaTime);
        }
        else if (fadingIn)
        {
            fadingIn = false;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = sRenderer.color;
        color.a = alpha;
        sRenderer.color = color;
    }

    public void StartFade(bool fadeIn)
    {
        if (fadeIn)
            fadingIn = true;
        else
            fadingOut = true;
    }
}
