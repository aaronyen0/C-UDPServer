using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewUDPServer
{
    public partial class UDPServerTabPage
    {
        private void PathBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            _FileTxt.Text = file.FileName;
            file.Dispose();
        }

        private void ReadBtn_Click(object sender, EventArgs e)
        {
            int rv = gServer.ReadFile(_FileTxt.Text);
        }

        private void BindBtn_Click(object sender, EventArgs e)
        {
            int rv = gServer.SourceSet(_ServerIPTxt.Text, _ServerPortTxt.Text);
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            int rv = gServer.DestinationSet(_ClientIPTxt.Text, _ClientPortTxt.Text);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            int rv = gServer.Print();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            int rv = gServer.Stop();
        }

        private void StartPauseBtn_Click(object sender, EventArgs e)
        {
            int rv = gServer.StartAndPause();
        }
    }
}
