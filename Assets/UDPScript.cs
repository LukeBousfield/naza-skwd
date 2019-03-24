using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPScript : MonoBehaviour
{

    public int Port;
    public string MCAddress;

    private UdpClient Client;
    private IPEndPoint LocalEp;
    private Thread ReceiveThread;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Starting");

        Client = new UdpClient();

        Client.ExclusiveAddressUse = false;
        LocalEp = new IPEndPoint(IPAddress.Any, Port);

        Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        Client.ExclusiveAddressUse = false;

        Client.Client.Bind(LocalEp);

        IPAddress multicastAddress = IPAddress.Parse(MCAddress);
        Client.JoinMulticastGroup(multicastAddress);

        Debug.Log("Listening");

        ReceiveThread = new Thread(new ThreadStart(ReceiveData));
        ReceiveThread.IsBackground = true;
        ReceiveThread.Start();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void ReceiveData()
    {
        Debug.Log("Actually run");
        while (Thread.CurrentThread.IsAlive)
        {
            Debug.Log("Running receive data");
            byte[] data = Client.Receive(ref LocalEp);
            ArrayList parsed = ParseBin(data);
            PrintAL(parsed);
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application quit");
        ReceiveThread.Abort();
        if (Client != null)
        {
            Client.Close();
        }
    }

    ArrayList ParseBin(byte[] data)
    {
        ArrayList values = new ArrayList();
        try
        {
            int numRuns = 0;
            int currOffset = 0;
            while (true)
            {
                if (numRuns > 20)
                {
                    values.Add((sbyte)data[currOffset]);
                    currOffset += 4;
                }
                else
                {
                    values.Add(System.BitConverter.ToDouble(data, currOffset));
                    currOffset += 8;
                }
                numRuns++;
            }
        }
        catch (System.Exception err) { }
        return values;
    }

    void PrintAL(ArrayList list)
    {
        string str = "[";
        for (int i = 0; i < list.Count; i++)
        {
            if (i != 0)
            {
                str += ", ";
            }
            str += list[i].ToString();
        }
        str += "]";
        Debug.Log(str);
    }

}
