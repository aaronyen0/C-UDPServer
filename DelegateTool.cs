using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewUDPServer
{
    public static class DelegateTool
    {
        delegate void LblTextHandler(Label lbl, string text);
        delegate void TxtWriteHandler(TextBox txt, string str);
        delegate void RtbWriteHandler(RichTextBox txt, string str);
        delegate void RtbWriteHexHandler(RichTextBox txt, byte[] array, int offset, int len);

        public static void LblText(Label lbl, string text)
        {
            if(lbl == null)
            {
                return;
            }
            if (lbl.InvokeRequired)
            {
                LblTextHandler handle = new LblTextHandler(LblText);
                lbl.Invoke(handle, lbl, text);
            }
            else
            {
                lbl.Text = text;
            }
        }

        public static void TxtWrite(TextBox txt, string str)
        {
            if (txt == null)
            {
                return;
            }
            if (txt.InvokeRequired)
            {
                TxtWriteHandler handle = new TxtWriteHandler(TxtWrite);
                txt.Invoke(handle, txt, str);
            }
            else
            {
                txt.AppendText(str);
            }
        }

        public static void RtbWrite(RichTextBox rtb, string str)
        {
            if (rtb == null)
            {
                return;
            }
            if (rtb.InvokeRequired)
            {
                RtbWriteHandler handle = new RtbWriteHandler(RtbWrite);
                rtb.Invoke(handle, rtb, str);
            }
            else
            {
                rtb.AppendText(str);
            }
        }

        public static void RtbWrite(RichTextBox rtb, String format, params object[] args)
        {
            if (rtb == null)
            {
                return;
            }
            if (rtb.InvokeRequired)
            {
                RtbWriteHandler handle = new RtbWriteHandler(RtbWrite);
                rtb.Invoke(handle, rtb, String.Format(format, args));
            }
            else
            {
                rtb.AppendText(String.Format(format, args));
            }
        }

        public static void RtbWrite_Limit(RichTextBox rtb, string str)
        {
            int line, tmpStart, tmpEnd;
            if (rtb == null)
            {
                return;
            }

            if (rtb.InvokeRequired)
            {
                RtbWriteHandler handle = new RtbWriteHandler(RtbWrite_Limit);
                rtb.Invoke(handle, rtb, str);
            }
            else
            {
                line = rtb.Lines.Length;
                if (line > 0x400)
                {
                    tmpStart = rtb.GetFirstCharIndexFromLine(0); // 第一行第一個字符的索引
                    tmpEnd = rtb.GetFirstCharIndexFromLine(0x200); //第n/2行第一個字符的索引
                    rtb.Text = rtb.Text.Remove(tmpStart, tmpEnd);
                }
                rtb.AppendText(str);
            }
        }

        public static void RtbWrite_Limit(RichTextBox rtb, String format, params object[] args)
        {
            int line, tmpStart, tmpEnd;
            if (rtb == null)
            {
                return;
            }

            if (rtb.InvokeRequired)
            {
                RtbWriteHandler handle = new RtbWriteHandler(RtbWrite_Limit);
                rtb.Invoke(handle, rtb, String.Format(format, args));
            }
            else
            {
                line = rtb.Lines.Length;
                if (line > 0x400)
                {
                    tmpStart = rtb.GetFirstCharIndexFromLine(0); // 第一行第一個字符的索引
                    tmpEnd = rtb.GetFirstCharIndexFromLine(0x200); //第n/2行第一個字符的索引
                    rtb.Text = rtb.Text.Remove(tmpStart, tmpEnd);
                }
                rtb.AppendText(String.Format(format, args));
            }
        }

        public static void RtbWriteHex(RichTextBox rtb, byte[] array, int offset, int len)
        {
            String str = "";
            int line, tmpStart, tmpEnd;
            if (rtb == null)
            {
                return;
            }

            if (rtb.InvokeRequired)
            {
                RtbWriteHexHandler handle = new RtbWriteHexHandler(RtbWriteHex);
                rtb.Invoke(handle, rtb, array, offset, len);
            }
            else
            {
                line = rtb.Lines.Length;
                if (line > 0x400)
                {
                    tmpStart = rtb.GetFirstCharIndexFromLine(0); // 第一行第一個字符的索引
                    tmpEnd = rtb.GetFirstCharIndexFromLine(0x200); //第n/2行第一個字符的索引
                    rtb.Text = rtb.Text.Remove(tmpStart, tmpEnd);
                }
                for (int i = 0; i < len; ++i) 
                {
                    str += String.Format("{0,2:X2}-", array[offset + i]);
                }
                rtb.AppendText(str+"\n");
            }
        }

    }
}
