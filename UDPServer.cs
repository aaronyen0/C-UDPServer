using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;

namespace NewUDPServer
{
    public class UDPServer
    {
        private string gServerName;

        public enum SendType { Random, FixedSize, PacketSize };
        public struct ServerComponent
        {
            public String fileName;
            public IPEndPoint SrcIP;
            public IPEndPoint DstIP;

            public Label ProgressLbl;
            public RichTextBox ServerRtb;
            public CheckBox CycleChk;
            public ComboBox SendTypeCbo;
        };

        private const int SERVERBUFFERSIZE = 0x2000;

        public RichTextBox gSystemRtb;
        //public Label gProgress;

        #region GlobalVariable
        public ServerComponent gServerComponent;

        private FileStream gFs;
        private Socket gSock;

        private ManualResetEvent gPause;
        private ManualResetEvent gStop;
        private ManualResetEvent gPrint;

        private Thread gThread;
        private SendType gTypeFlag;
        private bool gCycleFlag;

        private float gFileLen;
        private byte[] gBuffer;
        private Random gRnd;
        #endregion

        public UDPServer(string serverName)
        {
            gServerName = serverName;

            gBuffer = new byte[SERVERBUFFERSIZE];
            gPause = new ManualResetEvent(true);
            gStop = new ManualResetEvent(false);
            gPrint = new ManualResetEvent(false);
            gRnd = new Random();
            gCycleFlag = false;
            gTypeFlag = SendType.Random;
        }

        public UDPServer(string serverName, RichTextBox system) 
        {
            gServerName = serverName;
            gSystemRtb = system;

            gBuffer = new byte[SERVERBUFFERSIZE];
            gPause = new ManualResetEvent(true);
            gStop = new ManualResetEvent(false);
            gPrint = new ManualResetEvent(false);
            gRnd = new Random();
            gCycleFlag = false;
            gTypeFlag = SendType.Random;
        }

        private int CheckComponent()
        {
            if (gServerComponent.SrcIP == null) 
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]No Source Information.\n", gServerName);
                return -2;
            }
            if (gServerComponent.DstIP == null) 
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]No Destination Information.\n", gServerName);
                return -3;
            }
            if (gServerComponent.ServerRtb == null || gServerComponent.SendTypeCbo == null || 
                gServerComponent.ProgressLbl == null || gServerComponent.CycleChk == null) 
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]Missing some parts.\n", gServerName);
                return -4;
            }
            return 0;
        }

        #region Action
        
        public int StartAndPause() 
        {
            int rv;
            if (gThread == null)
            {
                if ((rv = CheckComponent()) < 0)
                {
                    return rv;
                }

                if (Enum.TryParse(gServerComponent.SendTypeCbo.Text, out gTypeFlag) == false)
                {
                    DelegateTool.RtbWrite(gSystemRtb, "[{0}]Send Type Error\n", gServerName);
                    return -8;
                }

                try 
                {
                    gSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    gSock.EnableBroadcast = true;
                    gSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

                    gSock.Bind(gServerComponent.SrcIP);
                }
                catch(Exception ex)
                {
                    DelegateTool.RtbWrite(gSystemRtb, "[{0}]{1}\n", gServerName, ex.Message);
                    //Console.WriteLine(ex.Message);
                    return -10;
                }

                try 
                {
                    gFs = new FileStream(gServerComponent.fileName, FileMode.Open, FileAccess.Read);
                    gFs.Seek(0, SeekOrigin.Begin);
                    gFileLen = (float)(gFs.Length);
                }
                catch(Exception ex)
                {
                    DelegateTool.RtbWrite(gSystemRtb, "[{0}]{1}\n", gServerName, ex.Message);
                    //Console.WriteLine(ex.Message);
                    goto EXIT1;
                }

                gPause.Set();
                gStop.Reset();
                gPrint.Reset();

                gThread = new Thread(MainThread);
                gThread.Start();
            }
            else 
            {
                if (gPause.WaitOne(0) == true)
                {
                    gPause.Reset();
                }
                else 
                {
                    gPause.Set();
                }
            }
            return 0;

            EXIT1:
            gSock.Dispose();
            gSock = null;
            return -20;
        }
        public int Stop() 
        {
            if (gThread == null) 
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]Thread is not alive.\n", gServerName);
                return -1;
            }
            gStop.Set();
            gPause.Set();
            return 0;
        }
        public int Print() 
        {
            if (gPrint.WaitOne(0) == true) 
            {
                DelegateTool.RtbWrite(gSystemRtb,"[{0}]Print Close\n", gServerName);
                gPrint.Reset();
            }
            else
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]Print Open\n", gServerName);
                gPrint.Set();
            }
            return 0;
        }

        public int ReadFile(string fileName) 
        {
            if (gThread != null) 
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]Thread is alive.\n", gServerName);
                return -1;
            }
            gServerComponent.fileName = fileName;
            DelegateTool.RtbWrite(gSystemRtb, "[{0}]Set FileName:{1}\n",gServerName, fileName);
            return 0;
        }
        public int SourceSet(string ip, string port) 
        {
            if (gThread != null) 
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]Thread is alive.\n", gServerName);
                return -1;
            }

            try
            {
                gServerComponent.SrcIP = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            }
            catch(Exception ex)
            {
                DelegateTool.RtbWrite_Limit(gSystemRtb, "[{0}]{1}\n", gServerName, ex.Message);
                return -2;
            }
            DelegateTool.RtbWrite(gSystemRtb, "[{0}]Set Src Info IP:{1}, Port:{2}\n",gServerName, ip, port);
            return 0;
        }
        public int DestinationSet(string ip, string port) 
        {
            if (gThread != null)
            {
                DelegateTool.RtbWrite(gSystemRtb, "[{0}]Thread is alive.\n", gServerName);
                return -1;
            }
            try
            {
                gServerComponent.DstIP = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            }
            catch (Exception ex)
            {
                DelegateTool.RtbWrite_Limit(gSystemRtb, "[{0}]{1}\n", gServerName, ex.Message);
                return -2;
            }
            DelegateTool.RtbWrite(gSystemRtb, "[{0}]Set Dst Info IP:{1}, Port:{2}\n",gServerName, ip, port);
            return 0;
        }

        #endregion

        #region MainLoop

        public void MainThread()
        {
            FileStream testFp = new FileStream(String.Format("Test_{0}", DateTime.Now.ToString("yyyyMMdd_HHmmss")), FileMode.Create, FileAccess.Write);
            Label pLbl = gServerComponent.ProgressLbl;
            Form1.timeBeginPeriod(1);
            int rv, offset = 0, totalLen = 0;
            int rHead, rLen;
            float progress;
            DelegateTool.RtbWrite(gSystemRtb, "[{0}]Thread Start\n",gServerName);
            while (true)
            {
                gPause.WaitOne();
                if (gStop.WaitOne(0) == true)
                {
                    goto EXIT;
                }

                rv = gFs.Read(gBuffer, totalLen, SERVERBUFFERSIZE - totalLen);
                if (rv <= 0)
                {
                    if (totalLen > 0) 
                    {
                        //gSock.SendTo(gBuffer, 0, totalLen, SocketFlags.None, gServerComponent.DstIP);
                        testFp.Write(gBuffer, 0, totalLen);
                    }

                    if (gCycleFlag == true)
                    {
                        DelegateTool.RtbWrite(gSystemRtb, "[{0}]Replay.\n", gServerName);
                        totalLen = 0;
                        gFs.Seek(0, SeekOrigin.Begin);
                        continue;
                    }
                    else
                    {
                        DelegateTool.RtbWrite(gSystemRtb, "[{0}]rv = {1}\n", gServerName, rv);
                        break;
                    }
                }
                totalLen += rv;
                if (gFileLen != 0)
                {
                    progress = (float)gFs.Position / gFileLen;
                    DelegateTool.LblText(pLbl, progress.ToString("0.00%"));
                    //DelegateControl.LabelText(gProgressLabel, progress.ToString("0.00%"));
                }

                //傳送資料
                offset = 0;
                while(true)
                {
                    rv = GetNextLength(gBuffer, offset, totalLen, out rHead, out rLen);
                    if (rv == 1)
                    {
                        //gSock.SendTo(gBuffer, rHead, rLen, SocketFlags.None, gServerComponent.DstIP);
                        testFp.Write(gBuffer, rHead, rLen);
                        offset = rHead + rLen;
                        if (gPrint.WaitOne(0)) 
                        {
                            DelegateTool.RtbWriteHex(gServerComponent.ServerRtb, gBuffer, rHead, rLen);
                        }
                    }
                    else if(rv == 0)
                    {
                        totalLen = rLen;
                        break;
                    }
                    else
                    {
                        totalLen = 0;
                        break;
                    }
                }
                Thread.Sleep(1);
            }

        EXIT:
            gThread = null;
            gFs.Dispose();
            gSock.Dispose();
            Form1.timeEndPeriod(1);
            DelegateTool.RtbWrite(gSystemRtb, "[{0}]Thread Leave\n",gServerName);

            testFp.Dispose();
        }

        //決定這一輪要傳送的offset和封包長度，決定的依據有：1.buffer內可用資料的長度、2.建thread時切割分包的選則
        //回傳值：
        //0：資料不足傳送，並自動將剩餘資料推至buffer最前面，此時回傳值的rLen代表剩餘可用資料長度
        //1：可根據rHead和rLen的值送出資料
        //-1：資料有問題，buffer可用資料直接歸0
        private int GetNextLength(byte[] buffer, int offset, int totalLen, out int rHead, out int rLen) 
        {
            const byte constHead = 0x1b, constTail1 = 0x0d, constTail2 = 0x0a;
            int head, tail, tmpLen;

            rLen = 0;
            rHead = 0;
            if (gTypeFlag == SendType.Random) 
            {
                tmpLen = gRnd.Next(257, 800);
                //tmpLen = gRnd.Next(1,15);
                if (tmpLen > totalLen - offset)
                {
                    rHead = 0;
                    rLen = totalLen - offset;
                    Buffer.BlockCopy(buffer, offset, buffer, rHead, rLen);
                    return 0;
                }
                else 
                {
                    rHead = offset;
                    rLen = tmpLen;
                    return 1;
                }
            }
            else if (gTypeFlag == SendType.FixedSize)
            {
                //tmpLen = 400;
                tmpLen = 23;
                if (tmpLen > totalLen - offset)
                {
                    rHead = 0;
                    rLen = totalLen - offset;
                    Buffer.BlockCopy(buffer, offset, buffer, rHead, rLen);
                    return 0;
                }
                else
                {
                    rHead = offset;
                    rLen = tmpLen;
                    return 1;
                }
            }
            else if (gTypeFlag == SendType.PacketSize)
            {
                head = 0;
                tail = 0;
                while (true)
                {
                    head = Array.IndexOf(buffer, constHead, offset, totalLen - offset);
                    offset = head + 1;
                    if (head == -1)
                    {
                        rLen = 0;
                        rHead = 0;
                        return -1;
                    }
                    else if ((offset < totalLen && buffer[offset] != constTail1) || offset == totalLen)
                    {
                        break;
                    }
                }

                while (head != -1)
                {
                    tail = Array.IndexOf(buffer, constTail2, offset, totalLen - offset);
                    offset = tail + 1;
                    if (tail == -1)
                    {
                        rLen = totalLen - head;
                        rHead = 0;
                        Buffer.BlockCopy(buffer, head, buffer, rHead, rLen);
                        return 0;
                    }
                    else if (buffer[tail - 1] == constTail1)
                    {
                        rHead = head;
                        rLen = tail - head + 1;
                        return 1;
                    }
                }
                DelegateTool.RtbWrite(gSystemRtb, "Error1, CheckProgram\n");
                return -1;
            }
            else 
            {
                DelegateTool.RtbWrite(gSystemRtb, "Error2, CheckProgram\n");
            }
            return -1;
        }
        
        #endregion
    }
}
