using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public static class QuaternionExtension
{
    public static float GetRoll(this Quaternion q)
    {
        return Mathf.Atan2(2 * q.y * q.w - 2 * q.x * q.z, 1 - 2 * q.y * q.y - 2 * q.z * q.z) * 180f / Mathf.PI; //q.x
    }

    public static float GetPitch(this Quaternion q)
    {
        return Mathf.Atan2(2 * q.x * q.w - 2 * q.y * q.z, 1 - 2 * q.x * q.x - 2 * q.z * q.z) * 180f / Mathf.PI; //q.y
    }

    public static float GetYaw(this Quaternion q)
    {
        return Mathf.Asin(2 * q.x * q.y + 2 * q.z * q.w) * 180f / Mathf.PI; //q.z
    }

    public static string Describe(this Quaternion q)
    {
        float roll = q.GetRoll();
        float pitch = q.GetPitch();
        float yaw = q.GetYaw();
        return " Roll: " + roll + "    Pitch: " + pitch + "    Yaw: " + yaw;
    }
}

public struct CoordPosition
{
    public double Altitude, Latitude, Longitude;

    public CoordPosition(double altitude, double latitude, double longitude)
    {
        this.Altitude = altitude;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public string Describe()
    {
        return " Altitude: " + Altitude + "    Latitude: " + Latitude + "    Longitude: " + Longitude;
    }

}

public struct SystemState
{

    public Quaternion Rotation;
    public CoordPosition Position;

    public SystemState(Quaternion Rotation, CoordPosition Position)
    {
        this.Rotation = Rotation;
        this.Position = Position;
    }

    public string Describe()
    {
        string str = "";
        str += "    Position: " + Position.Describe();
        str += "\n";
        str += "    Rotation: " + Rotation.Describe();
        str += "\n";
        return str;
    }

}

public class TotalState
{

    public SystemState CMSystemState;
    public SystemState LASSystemState;
    public SystemState BoosterSystemState;
    public sbyte EngineFlag;
    public sbyte Reserved;

    public TotalState(ArrayList arr)
    {
        CMSystemState = GetSystemState(arr, 0);
        LASSystemState = GetSystemState(arr, 1);
        BoosterSystemState = GetSystemState(arr, 2);
        EngineFlag = (sbyte)arr[arr.Count - 2];
        Reserved = (sbyte)arr[arr.Count - 1];
    }

    public string Describe()
    {
        string str = "";
        str += "CM System: ";
        str += "\n";
        str += CMSystemState.Describe();
        str += "LAS System: ";
        str += "\n";
        str += LASSystemState.Describe();
        str += "Booster System: ";
        str += "\n";
        str += BoosterSystemState.Describe();
        str += "Engine flag: " + EngineFlag;
        str += "\n";
        str += "Reserved: " + Reserved;
        return str;
    }

    private static SystemState GetSystemState(ArrayList arr, int index)
    {
        int posStart = index * 3;
        double altitude = (double)arr[posStart];
        double latitude = (double)arr[posStart + 1];
        double longitude = (double)arr[posStart + 2];
        if (index == 1)
        {
            longitude = longitude - .0000006;
        }
        if (index == 2)
        {
            longitude = longitude - .0000008;
        }
        CoordPosition pos = new CoordPosition(altitude, latitude, longitude);

        int rotStart = 9 + 4 * index;

        double w = (double)arr[rotStart];
        double x = (double)arr[rotStart + 1];
        double y = (double)arr[rotStart + 2];
        double z = (double)arr[rotStart + 3];

        //float x1 = (float)System.Math.Atan2(2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z);
        //float y1 = (float)System.Math.Atan2(2 * x * w - 2 * y * z, 1 - 2 * x * x - 2 * z * z);
        //float z1 = (float)System.Math.Asin(2 * x * y + 2 * z * w);

        //Vector3 rotation = new Vector3(x1, y1, z1);

        Quaternion rotation = new Quaternion((float)x, (float)y, (float)z, (float)w);

        SystemState state = new SystemState(rotation, pos);

        return state;
    }
}

public class UDPScript : MonoBehaviour
{

    public int Port;
    public string MCAddress;

    public GameObject CMSystemGameObject;
    public GameObject LASSystemGameObject;
    public GameObject BoosterSystemGameObject;

    public GameObject StateText;

    private UdpClient Client;
    private IPEndPoint LocalEp;
    private Thread ReceiveThread;
    private int LoopNum;

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

    private void Awake()
    {
        UnityThread.initUnityThread();
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
            LoopNum++;
            //Debug.Log("Running receive data");
            byte[] data = Client.Receive(ref LocalEp);
            ArrayList parsed = ParseBin(data);
            //PrintAL(parsed);
            TotalState state = new TotalState(parsed);
            //Debug.Log(state.Describe());
            UnityThread.executeInUpdate(() =>
            {
                StateText.GetComponent<Text>().text = "Loop: " + LoopNum + "\n" + state.Describe();
                SystemAdapter boosterSA = BoosterSystemGameObject.GetComponent<SystemAdapter>();
                boosterSA.UpdateSystem(state.BoosterSystemState);
                SystemAdapter lasSA = LASSystemGameObject.GetComponent<SystemAdapter>();
                lasSA.UpdateSystem(state.LASSystemState);
                SystemAdapter cmSA = CMSystemGameObject.GetComponent<SystemAdapter>();
                cmSA.UpdateSystem(state.CMSystemState);
                //Debug.Log(cmSA);
            });
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
        catch (System.Exception) { }
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
