using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;
using Unity.Mathematics;
using TMPro;

public class UDPServer : MonoBehaviour
{
    private UdpClient udpClient;
    private int udpPort = 12345;
    public Transform shaver;
    public string receivedData;

    [Header("UI")]
    [SerializeField] Transform textTransform;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;

    public  float positionThreshold = 0.01f;
    public  float rotationThreshold = 0.01f;

    void Start()
    {
        
        // Initialize UDP client and bind to the port
        udpClient = new UdpClient(udpPort);
        Debug.Log("Listening for UDP data on port " + udpPort);

        // Begin receiving data asynchronously
        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
    }

    private void ReceiveCallback(System.IAsyncResult ar)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, udpPort);
        byte[] receivedBytes = udpClient.EndReceive(ar, ref endPoint);

        // Convert received bytes to a string
        receivedData = Encoding.UTF8.GetString(receivedBytes);

        // Handle the received data as needed (you can display it, process it, etc.)
        Debug.Log("Received data: " + receivedData);

        // Continue listening for more data
        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
    }
    private void Update()
    {
        text2.text = receivedData;
        string[] dataArray = receivedData.Split(":");
        if (dataArray.Length == 6)
        {
            // Parse received data
            Vector3 newPosition = new Vector3(-float.Parse(dataArray[0]), -float.Parse(dataArray[1]), float.Parse(dataArray[2]));
            Quaternion newRotation = new Quaternion(float.Parse(dataArray[3]), -float.Parse(dataArray[4]), float.Parse(dataArray[5]), 1);

            // Check if the change in position is significant
            if (Vector3.Distance(shaver.transform.position, newPosition) > positionThreshold ||
                Quaternion.Angle(shaver.transform.rotation, newRotation) > rotationThreshold)
            {
                // Update position and rotation
                shaver.transform.position = newPosition;
                shaver.transform.rotation = newRotation;

                text1.text = "Updating GameObject";
            }
            else
            {
                // Small change, no need to update
                text1.text = "Small change, not updating GameObject";
            }
        }
    }


    private void OnDisable()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }

   
}
