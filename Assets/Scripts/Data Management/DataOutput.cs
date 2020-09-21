using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataOutput : MonoBehaviour
{
    public SimpleController Skia;
    public LightController Lux;

    StreamWriter writer;
    StreamReader reader;
    List<string> myText = new List<string>();
    FileInfo myFile;

    float totalTimer;
    float intervalTimer;
    public float interval = 1;

    // Start is called before the first frame update
    void Start()
    {
        string address = Application.dataPath + "/Scripts/Data Management/matrices.csv";
        myFile = new FileInfo(address);
        if (myFile.Exists)
        {
            myFile.Delete();
            myFile.Refresh();
        }

        //writer = myFile.CreateText();
        string message = "time, skia position, lux position";
        myText.Add(message);
        WriteIntoTxt(message);
        //writer = myFile.AppendText();

        totalTimer = 0;
        intervalTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        totalTimer += Time.deltaTime;
        intervalTimer += Time.deltaTime;
        if(intervalTimer >= interval)
        {
            intervalTimer = 0;
            string message = "" + totalTimer + ", ("
                + Skia.transform.position.x + "," + Skia.transform.position.y + "), ("
                + Lux.transform.position.x + ", " + Lux.transform.position.y + ", " + Lux.transform.position.z + ")";
            myText.Add(message);
            WriteIntoTxt(message);
        }
    }

    public void WriteIntoTxt(string message)
    {
        if (!myFile.Exists)
        {
            writer = myFile.CreateText();
        }
        else
        {
            writer = myFile.AppendText();
        }
        foreach(string s in myText)
        {
            writer.WriteLine(s);
        }
        //writer.WriteLine(message);
        writer.Flush();
        writer.Dispose();
        writer.Close();
    }

    public void ReadOutTxt()
    {
        myText.Clear();
        reader = new StreamReader(Application.dataPath + "/matrices.csv", Encoding.UTF8);
        string text;
        while ((text = reader.ReadLine()) != null)
        {
            //myText.Add(int.Parse(text));
            myText.Add(text);
        }
        reader.Dispose();
        reader.Close();
    }

    public List<string> GetmytxtList()
    {
        ReadOutTxt();
        return myText;
    }
}
