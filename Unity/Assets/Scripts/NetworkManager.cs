using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private int connectionPort = 25001;
    TcpListener listener;
    internal TcpClient client;
    internal NetworkStream stream;

    public CubesManager cubesManager;

    private void Start()
    {
        SetupListener();
        StartCoroutine(ConnectToClient());
    }
    /// <summary>
    /// set up the listener
    /// </summary>
    private void SetupListener()
    {
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
    }

    /// <summary>
    /// connect to client where cube info data is stored
    /// </summary>
    IEnumerator ConnectToClient()
    {
        //wait until a client is detected
        yield return new WaitUntil(() => listener.Pending());
        client = listener.AcceptTcpClient();
        stream = client.GetStream();
        Debug.Log("connected to client");
    }

    /// <summary>
    /// send location of where the file is stored
    /// </summary>
    /// <param name="fileLocation"></param>
    void SendFileLocation(string fileLocation)
    {
        byte[] myWriteBuffer = Encoding.UTF8.GetBytes(fileLocation); //Converting string to byte data
        stream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
    }

    /// <summary>
    /// download the cubes info over the network
    /// </summary>
    /// <param name="fileLocation"></param>
    /// <returns></returns>
    public IEnumerator DownloadCubesInfo(string fileLocation)
    {
        SendFileLocation(fileLocation);
        // wait until data is received
        yield return new WaitUntil(() => stream.DataAvailable);
        byte[] buffer = new byte[client.ReceiveBufferSize];
        //receiving Data
        int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in bytes from Python
        if (bytesRead != 0)
        {
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
            cubesManager.InstantiateCubes(dataReceived);
        }
        else
        {
            Debug.LogWarning($"client returned 0 bytes, resetting connection...");
            client.Close();
            StartCoroutine(ConnectToClient());
        }
    }
}