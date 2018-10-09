using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NewUDPServer
{
    public partial class UDPServerTabPage:TabPage
    {
        private RichTextBox gSystem;
        private UDPServer gServer;

        private static int gServerTabPage = 1;

        private int _Width;
        private int _Height;

        #region Component
        private SplitContainer _FirstSplit;
        private SplitContainer _SecondSplit;
        private TableLayoutPanel _FirstPnl;

        private RichTextBox _ServerRtb;

        private Button _PathBtn;
        private Button _ReadBtn;
        private Button _BindBtn;
        private Button _ConnectBtn;
        private Button _PrintBtn;
        private Button _StopBtn;
        private Button _StartPauseBtn;

        private TextBox _FileTxt;
        private TextBox _ServerIPTxt;
        private TextBox _ServerPortTxt;
        private TextBox _ClientIPTxt;
        private TextBox _ClientPortTxt;

        private ComboBox _SendTypeCbo;

        private CheckBox _CycleChk;

        private Label _PercentLbl;
        #endregion

        public UDPServerTabPage(Control parent)
        {
            gSystem = null;

            parent.Controls.Add(this);
            this.BackColor = Color.White;
            this.Name = String.Format("Sever_{0}", gServerTabPage);
            this.Text = String.Format("Sever_{0}", gServerTabPage);
            gServerTabPage++;

            _Width = this.Width;
            _Height = this.Height;

            CreateComponent();

            AddFirstSplit(this);
            AddSecondSplit(_FirstSplit.Panel1);
            AddServerRtb(_FirstSplit.Panel2);
            AddFirstPnl(_SecondSplit.Panel1);
            AddFileComponent(_FirstPnl, 0);
            AddServerComponent(_FirstPnl, 1);
            AddClientComponent(_FirstPnl, 2);
            AddBehaviorComponent(_FirstPnl, 3);
            AddActionComponent(_FirstPnl, 4);
        }

        public UDPServerTabPage(Control parent, RichTextBox system)
        {
            gSystem = system;

            parent.Controls.Add(this);
            this.BackColor = Color.White;
            this.Name = String.Format("Sever{0}", gServerTabPage);
            this.Text = String.Format("Sever{0}", gServerTabPage);
            gServerTabPage++;

            _Width = this.Width;
            _Height = this.Height;

            CreateComponent();

            AddFirstSplit(this);
            AddSecondSplit(_FirstSplit.Panel1);
            AddServerRtb(_FirstSplit.Panel2);
            AddFirstPnl(_SecondSplit.Panel1);
            AddFileComponent(_FirstPnl, 0);
            AddServerComponent(_FirstPnl, 1);
            AddClientComponent(_FirstPnl, 2);
            AddBehaviorComponent(_FirstPnl, 3);
            AddActionComponent(_FirstPnl, 4);
        }


        private void CreateComponent()
        {
            _FirstSplit = new SplitContainer();
            _SecondSplit = new SplitContainer();
            _ServerRtb = new RichTextBox();
            _FirstPnl = new TableLayoutPanel();

            _PathBtn = new Button();
            _ReadBtn = new Button();
            _BindBtn = new Button();
            _ConnectBtn = new Button();
            _PrintBtn = new Button();
            _StopBtn = new Button();
            _StartPauseBtn = new Button();

            _FileTxt = new TextBox();
            _ServerIPTxt = new TextBox();
            _ServerPortTxt = new TextBox();
            _ClientIPTxt = new TextBox();
            _ClientPortTxt = new TextBox();

            _SendTypeCbo = new ComboBox();

            _CycleChk = new CheckBox();

            _PercentLbl = new Label();

            this._PathBtn.Click += new System.EventHandler(this.PathBtn_Click);
            this._ReadBtn.Click += new System.EventHandler(this.ReadBtn_Click);
            this._BindBtn.Click += new System.EventHandler(this.BindBtn_Click);
            this._ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            this._StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            this._PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            this._StartPauseBtn.Click += new System.EventHandler(this.StartPauseBtn_Click);

            gServer = new UDPServer(this.Name, gSystem);
            gServer.gServerComponent.CycleChk = _CycleChk;
            gServer.gServerComponent.ProgressLbl = _PercentLbl;
            gServer.gServerComponent.SendTypeCbo = _SendTypeCbo;
            gServer.gServerComponent.ServerRtb = _ServerRtb;
        }

        private int AddFirstSplit(Control parent)
        {
            int pWidth, pHeight;
            if (parent == null || _FirstSplit == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = parent.Height;

            parent.Controls.Add(_FirstSplit);

            _FirstSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            _FirstSplit.Location = new System.Drawing.Point(0, 0);
            _FirstSplit.Size = new System.Drawing.Size(pWidth, pHeight);

            _FirstSplit.Orientation = System.Windows.Forms.Orientation.Vertical;
            if (pWidth > 600)
            {
                _FirstSplit.SplitterDistance = 400;
            }
            else 
            {
                _FirstSplit.SplitterDistance = pWidth * 2 / 3;
            }
            _FirstSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            return 0;
        }

        private int AddSecondSplit(Control parent)
        {
            int pWidth, pHeight;
            if (parent == null || _SecondSplit == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = parent.Height;

            parent.Controls.Add(_SecondSplit);

            _SecondSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            _SecondSplit.Location = new System.Drawing.Point(0, 0);
            _SecondSplit.Size = new System.Drawing.Size(pWidth, pHeight);

            _SecondSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            if (pHeight > 360)
            {
                _SecondSplit.SplitterDistance = 240;
            }
            else
            {
                _SecondSplit.SplitterDistance = pHeight * 2/3;
            }
            _SecondSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            return 0;
        }

        private int AddFirstPnl(Control parent)
        {
            int pWidth, pHeight;
            if (parent == null || _FirstPnl == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = parent.Height;

            parent.Controls.Add(_FirstPnl);

            int pnlWidth = 320, pnlHeight = 200;
            //if (parent.Width > 400)
            //{
            //    tlpWidth = (int)(parent.Width * 0.8f);
            //}
            //else
            //{
            //    tlpWidth = 320;
            //}

            _FirstPnl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            _FirstPnl.Location = new System.Drawing.Point(20,20);
            _FirstPnl.Size = new System.Drawing.Size(pnlWidth, pnlHeight);

            _FirstPnl.ColumnCount = 1;
            _FirstPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _FirstPnl.RowCount = 6;
            _FirstPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            _FirstPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            _FirstPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            _FirstPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            _FirstPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            _FirstPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            return 0;
        }

        private int AddServerRtb(Control parent)
        {
            int pWidth, pHeight;
            if (parent == null || _ServerRtb == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = parent.Height;

            parent.Controls.Add(_ServerRtb);

            _ServerRtb.Dock = System.Windows.Forms.DockStyle.Fill;
            _ServerRtb.Location = new System.Drawing.Point(0, 0);
            _ServerRtb.Size = new System.Drawing.Size(pWidth, pHeight);
            _ServerRtb.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            _ServerRtb.WordWrap = false;
            return 0;
        }

        int AddFileComponent(Control parent, int rowNum)
        {
            TableLayoutPanel pnl;
            int pWidth, pHeight;
            if (parent == null || _FileTxt == null || _PathBtn == null || _ReadBtn == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = (int)((TableLayoutPanel)parent).RowStyles[rowNum].Height;

            pnl = new TableLayoutPanel();
            ((TableLayoutPanel)parent).Controls.Add(pnl, 0, rowNum);

            pnl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            pnl.Location = new System.Drawing.Point(0, 0);
            pnl.Size = new System.Drawing.Size(pWidth, pHeight);
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;

            pnl.ColumnCount = 3;
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.RowCount = 1;
            pnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            ((TableLayoutPanel)pnl).Controls.Add(_FileTxt, 0, 0);
            _FileTxt.Dock = System.Windows.Forms.DockStyle.Fill;

            ((TableLayoutPanel)pnl).Controls.Add(_PathBtn, 1, 0);
            _PathBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _PathBtn.BackColor = Color.Transparent;
            _PathBtn.Text = "...";

            ((TableLayoutPanel)pnl).Controls.Add(_ReadBtn, 2, 0);
            _ReadBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _ReadBtn.BackColor = Color.Transparent;
            _ReadBtn.Text = "Read";

            return 0;
        }

        int AddServerComponent(Control parent, int rowNum)
        {
            TableLayoutPanel pnl;
            int pWidth, pHeight;
            if (parent == null || _ServerIPTxt == null || _ServerPortTxt == null || _BindBtn == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = (int)((TableLayoutPanel)parent).RowStyles[rowNum].Height;

            pnl = new TableLayoutPanel();
            ((TableLayoutPanel)parent).Controls.Add(pnl, 0, rowNum);

            pnl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            pnl.Location = new System.Drawing.Point(0, 0);
            pnl.Size = new System.Drawing.Size(pWidth, pHeight);
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;

            pnl.ColumnCount = 3;
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.RowCount = 1;
            pnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            ((TableLayoutPanel)pnl).Controls.Add(_ServerIPTxt, 0, 0);
            _ServerIPTxt.Dock = System.Windows.Forms.DockStyle.Fill;

            ((TableLayoutPanel)pnl).Controls.Add(_ServerPortTxt, 1, 0);
            _ServerPortTxt.Dock = System.Windows.Forms.DockStyle.Fill;

            ((TableLayoutPanel)pnl).Controls.Add(_BindBtn, 2, 0);
            _BindBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _BindBtn.BackColor = Color.Transparent;
            _BindBtn.Text = "Bind";

            return 0;
        }

        int AddClientComponent(Control parent, int rowNum)
        {
            TableLayoutPanel pnl;
            int pWidth, pHeight;
            if (parent == null || _ClientIPTxt == null || _ClientPortTxt == null || _ConnectBtn == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = (int)((TableLayoutPanel)parent).RowStyles[rowNum].Height;

            pnl = new TableLayoutPanel();
            ((TableLayoutPanel)parent).Controls.Add(pnl, 0, rowNum);

            pnl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            pnl.Location = new System.Drawing.Point(0, 0);
            pnl.Size = new System.Drawing.Size(pWidth, pHeight);
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;

            pnl.ColumnCount = 3;
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.RowCount = 1;
            pnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            ((TableLayoutPanel)pnl).Controls.Add(_ClientIPTxt, 0, 0);
            _ClientIPTxt.Dock = System.Windows.Forms.DockStyle.Fill;

            ((TableLayoutPanel)pnl).Controls.Add(_ClientPortTxt, 1, 0);
            _ClientPortTxt.Dock = System.Windows.Forms.DockStyle.Fill;

            ((TableLayoutPanel)pnl).Controls.Add(_ConnectBtn, 2, 0);
            _ConnectBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _ConnectBtn.BackColor = Color.Transparent;
            _ConnectBtn.Text = "Connect";

            return 0;
        }

        int AddBehaviorComponent(Control parent, int rowNum)
        {
            TableLayoutPanel pnl;
            int pWidth, pHeight;
            if (parent == null || _SendTypeCbo == null || _CycleChk == null || _PercentLbl == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = (int)((TableLayoutPanel)parent).RowStyles[rowNum].Height;

            pnl = new TableLayoutPanel();
            ((TableLayoutPanel)parent).Controls.Add(pnl, 0, rowNum);

            pnl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            pnl.Location = new System.Drawing.Point(0, 0);
            pnl.Size = new System.Drawing.Size(pWidth, pHeight);
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;

            pnl.ColumnCount = 3;
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            pnl.RowCount = 1;
            pnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            ((TableLayoutPanel)pnl).Controls.Add(_SendTypeCbo, 0, 0);
            _SendTypeCbo.Dock = System.Windows.Forms.DockStyle.Fill;
            foreach (string str in Enum.GetNames(typeof(UDPServer.SendType)))
            {
                _SendTypeCbo.Items.Add(str);
                if ((UDPServer.SendType)Enum.Parse(typeof(UDPServer.SendType), str, true) == 0) 
                {
                    _SendTypeCbo.Text = str;
                }
            }

            ((TableLayoutPanel)pnl).Controls.Add(_CycleChk, 1, 0);
            _CycleChk.Dock = System.Windows.Forms.DockStyle.Fill;
            _CycleChk.BackColor = Color.Transparent;
            _CycleChk.Text = "Cycle";

            ((TableLayoutPanel)pnl).Controls.Add(_PercentLbl, 2, 0);
            _PercentLbl.AutoSize = true;
            _PercentLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            _PercentLbl.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _PercentLbl.BackColor = Color.Transparent;
            _PercentLbl.Text = "---.--%";


            return 0;
        }

        int AddActionComponent(Control parent, int rowNum)
        {
            TableLayoutPanel pnl;
            int pWidth, pHeight;
            if (parent == null || _PrintBtn == null || _StopBtn == null || _StartPauseBtn == null)
            {
                return -1;
            }
            pWidth = parent.Width;
            pHeight = (int)((TableLayoutPanel)parent).RowStyles[rowNum].Height;

            pnl = new TableLayoutPanel();
            ((TableLayoutPanel)parent).Controls.Add(pnl, 0, rowNum);

            pnl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            pnl.Location = new System.Drawing.Point(0, 0);
            pnl.Size = new System.Drawing.Size(pWidth, pHeight);
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;

            pnl.ColumnCount = 3;
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            pnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            pnl.RowCount = 1;
            pnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            ((TableLayoutPanel)pnl).Controls.Add(_PrintBtn, 0, 0);
            _PrintBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _PrintBtn.BackColor = Color.Transparent;
            _PrintBtn.Text = "Print";

            ((TableLayoutPanel)pnl).Controls.Add(_StopBtn, 1, 0);
            _StopBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _StopBtn.BackColor = Color.Transparent;
            _StopBtn.Text = "Stop";

            ((TableLayoutPanel)pnl).Controls.Add(_StartPauseBtn, 2, 0);
            _StartPauseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            _StartPauseBtn.BackColor = Color.Transparent;
            _StartPauseBtn.Text = "Start/Pause";

            return 0;
        }

    }
}
