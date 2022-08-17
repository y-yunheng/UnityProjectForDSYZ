using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using UnityEngine;

class MemorySharePython
{
    //���������������ڷ�������գ����û�����Ϊ10

    private int memorysize;
    private string Sendfilename;
    private string Recvicefilename;
    private MemoryMappedFile mmfsend;
    private MemoryMappedViewAccessor viewAccessorSend;

    public MemorySharePython( int memorysize =10000)
    {
      
        this.memorysize = memorysize;

        

    }


    public void WriteMemory(string sengstr, string Sendfilename)
    {
        sengstr = sengstr + "@";
        //MemoryMappedFile.OpenExisting(Sendfilename)
        mmfsend = MemoryMappedFile.CreateOrOpen(Sendfilename, memorysize, MemoryMappedFileAccess.ReadWrite);
        viewAccessorSend = mmfsend.CreateViewAccessor(0, sengstr.Length);
        viewAccessorSend.Write(0, sengstr.Length);
        viewAccessorSend.WriteArray<byte>(0, System.Text.Encoding.ASCII.GetBytes(sengstr), 0, sengstr.Length);
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
        
        viewAccessor.Dispose();
        mmf.Dispose();
        File.Delete(Recvicefilename);
        return msg;
    }
 





}