using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RaspberryCamera : MonoBehaviour
{
    public RawImage img;
    string baseUrl = "http://192.168.12.182:8090/?action=snapshot&n=";

    public float fps = 10f;

    int counter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Load());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Load()
    {
        while (true)
        {
            string url = baseUrl + counter++;

            UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(req);
                img.texture = tex;
            }

            yield return new WaitForSeconds(1f / fps);
        }

    }
}