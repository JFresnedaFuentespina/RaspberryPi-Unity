using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RaspberryVideoPlayer : MonoBehaviour
{
    [Header("UI")]
    public RawImage display;

    [Header("Video URL (RTSP / H264 stream)")]
    public string videoUrl = "rtsp://192.168.12.182:8554/stream";

    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    void Start()
    {
        // Crear RenderTexture donde se dibuja el vídeo
        renderTexture = new RenderTexture(1280, 720, 0);
        renderTexture.Create();

        // Asignar a UI
        display.texture = renderTexture;

        // Crear VideoPlayer dinámico
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

        videoPlayer.isLooping = true;
        videoPlayer.playOnAwake = false;

        videoPlayer.errorReceived += OnVideoError;
        videoPlayer.prepareCompleted += OnPrepared;

        videoPlayer.Prepare();
    }

    private void OnPrepared(VideoPlayer vp)
    {
        Debug.Log("[VideoPlayer] Ready, starting playback...");
        vp.Play();
    }

    private void OnVideoError(VideoPlayer vp, string msg)
    {
        Debug.LogError("[VideoPlayer ERROR] " + msg);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.errorReceived -= OnVideoError;
            videoPlayer.prepareCompleted -= OnPrepared;
        }

        if (renderTexture != null)
        {
            renderTexture.Release();
        }
    }
}