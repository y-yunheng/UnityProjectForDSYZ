using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using UnityEngine;

public class MemoryShare
{
    //���������������ڷ�������գ����û�����Ϊ10

    private int memorysize;
    private string Sendfilename;
    private string Recvicefilename;
    private string strnull = "";

    public MemoryShare(int memorysize = 10000)
    {

        this.memorysize = memorysize;
        for (int i = 0; i < memorysize; i++)
        {
            strnull += '\x00';
        }


    }


    public void WriteMemory(string sengstr, string Sendfilename)
    {

        sengstr = sengstr + "@";
        MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen(Sendfilename, memorysize, MemoryMappedFileAccess.ReadWrite);
        MemoryMappedViewAccessor viewAccessorSend = mmf.CreateViewAccessor(0, memorysize);
        if (IsRead(viewAccessorSend))
        {
            viewAccessorSend.Write(0, memorysize);
            viewAccessorSend.WriteArray<byte>(0, System.Text.Encoding.ASCII.GetBytes(sengstr), 0, sengstr.Length);
        }
        else
        {

        }


    }


    public string ReadMemory(string Recvicefilename)
    {
        MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Recvicefilename);
        MemoryMappedViewAccessor viewAccessor = mmf.CreateViewAccessor(0, memorysize);
        byte[] charsInMMf = new byte[memorysize];
        viewAccessor.ReadArray<byte>(0, charsInMMf, 0, memorysize);
        string msg = Encoding.ASCII.GetString(charsInMMf);
        if (msg.Contains("}@"))
        {
            int index = msg.IndexOf("}@");
            msg = msg.Substring(0, index + 1);
        }
        JObject jo = JObject.Parse(msg);
        ClearMemory(viewAccessor);
        return jo.ToString();


    }
    public void ClearMemory(MemoryMappedViewAccessor viewAccessor)
    {
        viewAccessor.Write(0, memorysize);
        viewAccessor.WriteArray<byte>(0, System.Text.Encoding.ASCII.GetBytes(strnull), 0, strnull.Length);

    }
    public bool IsRead(MemoryMappedViewAccessor viewAccessor)
    {
        byte[] charsInMMf = new byte[memorysize];
        viewAccessor.ReadArray<byte>(0, charsInMMf, 0, memorysize);
        string msg = Encoding.ASCII.GetString(charsInMMf);
        
        if (msg[0]=='\x00')
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
