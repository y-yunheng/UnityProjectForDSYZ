using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileShare
{
    private string path;
    private string t_sLine;

    public FileShare(string path )
    {
        this.path = path;
    }

    public void CreateOrOPenFile(string info,string name)
    {          //路径、文件名、写入内容
        StreamWriter sw;
        FileInfo fi = new FileInfo(path + name);
        sw = fi.CreateText();        //直接重新写入，如果要在原文件后面追加内容，应用fi.AppendText()
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
    public string ReadFile(string sName)
    {
        StreamReader sr = null;
        sr = File.OpenText(path + sName);
        string result = sr.ReadToEnd();  
        sr.Close();
        sr.Dispose();
        return result;
    }

}

