using System.Diagnostics;
using UnityEngine;

public class OpenBrowser : MonoBehaviour
{
    public string url = "http://192.168.12.182:8090/?action=stream";

    void Start()
    {
#if UNITY_STANDALONE_WIN
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

#elif UNITY_STANDALONE_LINUX
        Process.Start("xdg-open", url);
#endif
    }
}