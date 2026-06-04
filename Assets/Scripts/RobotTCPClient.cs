using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class RobotWASDAdvanced : MonoBehaviour
{
    public string ip = "192.168.12.182";
    public int port = 12345;

    public int moveSpeed = 50;

    public int cameraValue = 50;   // valor inicial
    public int cameraStep = 10;
    public int cameraMin = 0;
    public int cameraMax = 100;

    public float sendRate = 0.1f;

    private TcpClient client;
    private NetworkStream stream;

    private float timer;

    private string lastCommand = "";
    private bool buzzerSent = false;
    public string command = "";
    string moveCommand = "";
    string turnCommand = "";
    string cameraCommand = "";
    string buzzerCommand = "";
    void Start()
    {
        Connect();
    }

    void Connect()
    {
        try
        {
            client = new TcpClient();
            client.Connect(ip, port);
            stream = client.GetStream();
            Debug.Log("TCP conectado");
        }
        catch (Exception e)
        {
            Debug.LogError("TCP error: " + e.Message);
        }
    }

    void Update()
    {
        if (stream == null) return;

        timer += Time.deltaTime;
        if (timer < sendRate) return;
        timer = 0f;

        HandleInput();
    }

    void HandleInput()
    {
        moveCommand = "";
        turnCommand = "";
        cameraCommand = "";
        buzzerCommand = "";

        // ---------------- MOVIMIENTO ----------------
        if (Input.GetKey(KeyCode.W))
            moveCommand = $"Move Forward{moveSpeed}";
        else if (Input.GetKey(KeyCode.S))
            moveCommand = $"Move Backward{moveSpeed}";
        else
            moveCommand = "Move Stop";

        // ---------------- GIRO ----------------
        if (Input.GetKey(KeyCode.A))
            turnCommand = $"Turn Left{moveSpeed}";
        else if (Input.GetKey(KeyCode.D))
            turnCommand = $"Turn Right{moveSpeed}";
        else
            turnCommand = "Turn Center90";

        // ---------------- CÁMARA ----------------
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cameraValue -= cameraStep;
            cameraValue = Mathf.Clamp(cameraValue, cameraMin, cameraMax);
            cameraCommand = $"Camera Left{cameraValue}";
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            cameraValue += cameraStep;
            cameraValue = Mathf.Clamp(cameraValue, cameraMin, cameraMax);
            cameraCommand = $"Camera Left{cameraValue}";
        }
        else
        {
            cameraCommand = "Camera Stop";
        }

        // ---------------- ENVÍO FINAL ----------------
        Send(moveCommand);
        Send(turnCommand);
        Send(cameraCommand);
    }
    void Send(string msg)
    {
        try
        {
            if (stream == null) return;

            if (msg == lastCommand) return;
            lastCommand = msg;

            byte[] data = Encoding.UTF8.GetBytes(msg + ">");
            stream.Write(data, 0, data.Length);

            Debug.Log("Sent: " + msg);
        }
        catch (Exception e)
        {
            Debug.LogError("Send error: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        stream?.Close();
        client?.Close();
    }
}