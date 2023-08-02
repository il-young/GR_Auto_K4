using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Cognex.DataMan.SDK;
using System.Threading;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using System.IO;

namespace Bank_Host
{
	public partial class MainForm : Form
	{
		private ResultCollector _results;

		private SynchronizationContext _syncContext = null;
		private EthSystemDiscoverer _ethSystemDiscoverer = null;
		private SerSystemDiscoverer _serSystemDiscoverer = null;
		private ISystemConnector _connector = null;
		private DataManSystem _system = null;
		private object _currentResultInfoSyncLock = new object();
		private bool _closing = false;
		private bool _autoconnect = false;
		private object _listAddItemLock = new object();
		private GuiLogger _logger;

        private string Result_string = "";

		public MainForm()
		{
			InitializeComponent();

			// The SDK may fire events from arbitrary thread context. Therefore if you want to change
			// the state of controls or windows from any of the SDK' events, you have to use this
			// synchronization context to execute the event handler code on the main GUI thread.
			_syncContext = WindowsFormsSynchronizationContext.Current;

			//Setting up WindowsCE-specific event handlers manually, as they are lost from the Designer.cs file upon save
#if !WindowsCE
			cbEnableKeepAlive.CheckedChanged += new System.EventHandler(this.cbEnableKeepAlive_CheckedChanged);
#else
            cbEnableKeepAlive.CheckStateChanged += new System.EventHandler(this.cbEnableKeepAlive_CheckedChanged);
#endif

#if !WindowsCE
			cbLiveDisplay.CheckedChanged += new System.EventHandler(this.cbLiveDisplay_CheckedChanged);
#else
            cbLiveDisplay.CheckStateChanged += new System.EventHandler(this.cbLiveDisplay_CheckedChanged);
#endif

#if !WindowsCE
			cbLoggingEnabled.CheckedChanged += new System.EventHandler(this.cbLoggingEnabled_CheckedChanged);
#else
            cbLoggingEnabled.CheckStateChanged += new System.EventHandler(this.cbLoggingEnabled_CheckedChanged);
#endif

#if !WindowsCE
			FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
#else
            Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_FormClosing);
#endif

#if !WindowsCE
			btnTrigger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTrigger_MouseDown);
			btnTrigger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTrigger_MouseUp);
#else
            btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
#endif

            
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			_logger = new GuiLogger(tbLog, cbLoggingEnabled.Checked, ref _closing);

			// Create discoverers to discover ethernet and serial port systems.
			_ethSystemDiscoverer = new EthSystemDiscoverer();
			_serSystemDiscoverer = new SerSystemDiscoverer();

			// Subscribe to the system discoved event.
			_ethSystemDiscoverer.SystemDiscovered += new EthSystemDiscoverer.SystemDiscoveredHandler(OnEthSystemDiscovered);
			_serSystemDiscoverer.SystemDiscovered += new SerSystemDiscoverer.SystemDiscoveredHandler(OnSerSystemDiscovered);

			// Ask the discoverers to start discovering systems.
			_ethSystemDiscoverer.Discover();
			_serSystemDiscoverer.Discover();

			RefreshGui();
		}

		private void MainForm_FormClosing(object sender, EventArgs e)
		{
			_closing = true;
			_autoconnect = false;

			if (null != _system && _system.State == Cognex.DataMan.SDK.ConnectionState.Connected)
				_system.Disconnect();

			_ethSystemDiscoverer.Dispose();
			_ethSystemDiscoverer = null;

			_serSystemDiscoverer.Dispose();
			_serSystemDiscoverer = null;
		}

		private void Results_ComplexResultCompleted(object sender, ComplexResult e)
		{
			_syncContext.Post(
				delegate
				{
					ShowResult(e);
				},
				null);
		}

		private void Results_SimpleResultDropped(object sender, SimpleResult e)
		{
			_syncContext.Post(
				delegate
				{
					ReportDroppedResult(e);
				},
				null);
		}

		private void ReportDroppedResult(SimpleResult result)
		{
			AddListItem(string.Format("Partial result dropped: {0}, id={1}", result.Id.Type.ToString(), result.Id.Id));
		}

		private void RefreshGui()
		{
			bool system_connected = _system != null && _system.State == Cognex.DataMan.SDK.ConnectionState.Connected;
			bool system_ready_to_connect = _system == null || _system.State == Cognex.DataMan.SDK.ConnectionState.Disconnected;
			bool gui_ready_to_connect = listBoxDetectedSystems.SelectedIndex != -1 && listBoxDetectedSystems.Items.Count > listBoxDetectedSystems.SelectedIndex;

			btnConnect.Enabled = system_ready_to_connect && gui_ready_to_connect;
			btnDisconnect.Enabled = system_connected;
			btnTrigger.Enabled = system_connected;
			cbLiveDisplay.Enabled = system_connected;
		}

        public void Connect()
        {
            btnConnect_Click("Form1", EventArgs.Empty);
        }

        


		private void btnConnect_Click(object sender, EventArgs e)
		{
			if (listBoxDetectedSystems.SelectedIndex == -1 || listBoxDetectedSystems.SelectedIndex >= listBoxDetectedSystems.Items.Count)
				return;

			btnConnect.Enabled = false;
			_autoconnect = false;

			try
			{
				var system_info = listBoxDetectedSystems.Items[listBoxDetectedSystems.SelectedIndex];

				if (system_info is EthSystemDiscoverer.SystemInfo)
				{
					EthSystemDiscoverer.SystemInfo eth_system_info = system_info as EthSystemDiscoverer.SystemInfo;
					EthSystemConnector conn = new EthSystemConnector(eth_system_info.IPAddress, eth_system_info.Port);

					conn.UserName = "admin";
					conn.Password = txtPassword.Text;

					_connector = conn;
				}
				else if (system_info is SerSystemDiscoverer.SystemInfo)
				{
					SerSystemDiscoverer.SystemInfo ser_system_info = system_info as SerSystemDiscoverer.SystemInfo;
					SerSystemConnector conn = new SerSystemConnector(ser_system_info.PortName, ser_system_info.Baudrate);

					_connector = conn;
				}

				_logger.Enabled = cbLoggingEnabled.Checked;
				_connector.Logger = _logger;

				_system = new DataManSystem(_connector);
				_system.DefaultTimeout = 5000;

				// Subscribe to events that are signalled when the system is connected / disconnected.
				_system.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
				_system.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);
				_system.SystemWentOnline += new SystemWentOnlineHandler(OnSystemWentOnline);
				_system.SystemWentOffline += new SystemWentOfflineHandler(OnSystemWentOffline);
				_system.KeepAliveResponseMissed += new KeepAliveResponseMissedHandler(OnKeepAliveResponseMissed);
				_system.BinaryDataTransferProgress += new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);
				_system.OffProtocolByteReceived += new OffProtocolByteReceivedHandler(OffProtocolByteReceived);
				_system.AutomaticResponseArrived += new AutomaticResponseArrivedHandler(AutomaticResponseArrived);

				// Subscribe to events that are signalled when the device sends auto-responses.
				ResultTypes requested_result_types = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
				_results = new ResultCollector(_system, requested_result_types);
				_results.ComplexResultCompleted += Results_ComplexResultCompleted;
				_results.SimpleResultDropped += Results_SimpleResultDropped;

				_system.SetKeepAliveOptions(cbEnableKeepAlive.Checked, 3000, 1000);

				_system.Connect();

                BankHost_main.bVisionConnect = true;


                try
				{
					_system.SetResultTypes(requested_result_types);
				}
				catch(Exception ex)
				{ }
			}
			catch (Exception ex)
			{
				CleanupConnection();

				AddListItem("Failed to connect: " + ex.ToString());
                BankHost_main.bVisionConnect = false;
            }

			_autoconnect = true;
			RefreshGui();
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			try
			{
				if (_system == null || _system.State != Cognex.DataMan.SDK.ConnectionState.Connected)
					return;

				btnDisconnect.Enabled = false;

				_autoconnect = false;
				_system.Disconnect();

				CleanupConnection();

				_results.ClearCachedResults();
				_results = null;
			}
			finally
			{
				RefreshGui();
			}
		}

		private void cbEnableKeepAlive_CheckedChanged(object sender, EventArgs e)
		{
			if (null != _system)
				_system.SetKeepAliveOptions(cbEnableKeepAlive.Checked, 3000, 1000);
		}

		private void cbLiveDisplay_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (cbLiveDisplay.Checked)
				{
					btnTrigger.Enabled = false;

					_system.SendCommand("SET LIVEIMG.MODE 2");
					_system.BeginGetLiveImage(
						ImageFormat.jpeg,
						ImageSize.Sixteenth,
						ImageQuality.Medium,
						OnLiveImageArrived,
						null);
				}
				else
				{
					btnTrigger.Enabled = true;

					_system.SendCommand("SET LIVEIMG.MODE 0");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to set live image mode: " + ex.ToString());
			}
		}

		private void listBoxDetectedSystems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxDetectedSystems.SelectedIndex != -1 && listBoxDetectedSystems.Items.Count > listBoxDetectedSystems.SelectedIndex)
			{
				var system_info = listBoxDetectedSystems.Items[listBoxDetectedSystems.SelectedIndex];

				if (system_info is EthSystemDiscoverer.SystemInfo)
				{
					EthSystemDiscoverer.SystemInfo eth_system_info = system_info as EthSystemDiscoverer.SystemInfo;

					txtDeviceIP.Text = eth_system_info.IPAddress.ToString();
				}
				else if (system_info is SerSystemDiscoverer.SystemInfo)
				{
					SerSystemDiscoverer.SystemInfo ser_system_info = system_info as SerSystemDiscoverer.SystemInfo;

					txtDeviceIP.Text = ser_system_info.PortName;
				}
			}

			RefreshGui();
		}

		private void btnRefreshSystemList_Click(object sender, EventArgs e)
		{
			try
			{
				if (_ethSystemDiscoverer.IsDiscoveryInProgress || _serSystemDiscoverer.IsDiscoveryInProgress)
					return;

				listBoxDetectedSystems.Items.Clear();

				_ethSystemDiscoverer.Discover();
				_serSystemDiscoverer.Discover();
			}
			finally
			{
				RefreshGui();
			}
		}

        public void Trigger()
        {
            try
            {
                for (int i = -1; i < Properties.Settings.Default.CognexTriggerCnt; ++i)
                {
                    _system.SendCommand("TRIGGER ON");
                    System.Threading.Thread.Sleep(50);
                    _system.SendCommand("TRIGGER OFF");
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send TRIGGER Pulse command: " + ex.ToString());
                throw;
            }
            
        }

		private void btnTrigger_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				_system.SendCommand("TRIGGER ON");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to send TRIGGER ON command: " + ex.ToString());
			}
		}

		private void btnTrigger_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				_system.SendCommand("TRIGGER OFF");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to send TRIGGER OFF command: " + ex.ToString());
			}
		}

      

		private void btnTrigger_Click(object sender, EventArgs e)
		{
#if WindowsCE
            // Note: WindowsCE does not provide MouseDown/Up events for buttons, so 
            // we have to simulate it on this platform.
            try
            {
                _system.SendCommand("TRIGGER ON");
                Thread.Sleep(2000);
                _system.SendCommand("TRIGGER OFF");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send TRIGGER ON/OFF commands: " + ex.ToString());
            }
#else
            //Trigger(); 
#endif
        }

		private void cbLoggingEnabled_CheckedChanged(object sender, EventArgs e)
		{
			if (_connector != null && _connector.Logger != null)
			{
				_connector.Logger.Enabled = _logger.Enabled = cbLoggingEnabled.Checked;
				_logger.Log("Logging", _connector.Logger.Enabled ? "enabled" : "disabled");
			}
		}

		private void Log(string function, string message)
		{
			if (_logger != null)
				_logger.Log(function, message);
		}

		#region Device Discovery Events

		private void OnEthSystemDiscovered(EthSystemDiscoverer.SystemInfo systemInfo)
		{
			_syncContext.Post(
				new SendOrPostCallback(
					delegate
					{
						listBoxDetectedSystems.Items.Add(systemInfo);
                        listBoxDetectedSystems.SelectedIndex = 0;
                        Connect();
                    }),
					null);
		}

		private void OnSerSystemDiscovered(SerSystemDiscoverer.SystemInfo systemInfo)
		{
			_syncContext.Post(
				new SendOrPostCallback(
					delegate
					{
						listBoxDetectedSystems.Items.Add(systemInfo);
                        
					}),
					null);
		}

		#endregion

		#region Device Events

		private void OnSystemConnected(object sender, EventArgs args)
		{
			_syncContext.Post(
				delegate
				{
					AddListItem("System connected");
					RefreshGui();
				},
				null);
		}

		private void OnSystemDisconnected(object sender, EventArgs args)
		{
			_syncContext.Post(
				delegate
				{
					AddListItem("System disconnected");
					bool reset_gui = false;

					if (!_closing && _autoconnect && cbAutoReconnect.Checked)
					{
						frmReconnecting frm = new frmReconnecting(this, _system);

						if (frm.ShowDialog() == DialogResult.Cancel)
							reset_gui = true;
					}
					else
					{
						reset_gui = true;
					}

					if (reset_gui)
					{
						btnConnect.Enabled = true;
						btnDisconnect.Enabled = false;
						btnTrigger.Enabled = false;
						cbLiveDisplay.Enabled = false;

						picResultImage.Image = null;
						lbReadString.Text = "";
					}
				},
				null);
		}

		private void OnKeepAliveResponseMissed(object sender, EventArgs args)
		{
			_syncContext.Post(
				delegate
				{
					AddListItem("Keep-alive response missed");
				},
				null);
		}

		private void OnSystemWentOnline(object sender, EventArgs args)
		{
			_syncContext.Post(
				delegate
				{
					AddListItem("System went online");
				},
				null);
		}

		private void OnSystemWentOffline(object sender, EventArgs args)
		{
			_syncContext.Post(
				delegate
				{
					AddListItem("System went offline");
				},
				null);
		}

		private void OnBinaryDataTransferProgress(object sender, BinaryDataTransferProgressEventArgs args)
		{
			Log("OnBinaryDataTransferProgress", string.Format("{0}: {1}% of {2} bytes (Type={3}, Id={4})", args.Direction == TransferDirection.Incoming ? "Receiving" : "Sending", args.TotalDataSize > 0 ? (int)(100 * (args.BytesTransferred / (double)args.TotalDataSize)) : -1, args.TotalDataSize, args.ResultType.ToString(), args.ResponseId));
		}

		private void OffProtocolByteReceived(object sender, OffProtocolByteReceivedEventArgs args)
		{
			Log("OffProtocolByteReceived", string.Format("{0}", (char)args.Byte));
		}

		private void AutomaticResponseArrived(object sender, AutomaticResponseArrivedEventArgs args)
		{
			Log("AutomaticResponseArrived", string.Format("Type={0}, Id={1}, Data={2} bytes", args.DataType.ToString(), args.ResponseId, args.Data != null ? args.Data.Length : 0));
		}

		#endregion

		#region Auxiliary Methods

		private void CleanupConnection()
		{
			if (null != _system)
			{
				_system.SystemConnected -= OnSystemConnected;
				_system.SystemDisconnected -= OnSystemDisconnected;
				_system.SystemWentOnline -= OnSystemWentOnline;
				_system.SystemWentOffline -= OnSystemWentOffline;
				_system.KeepAliveResponseMissed -= OnKeepAliveResponseMissed;
				_system.BinaryDataTransferProgress -= OnBinaryDataTransferProgress;
				_system.OffProtocolByteReceived -= OffProtocolByteReceived;
				_system.AutomaticResponseArrived -= AutomaticResponseArrived;
			}

			_connector = null;
			_system = null;
		}

		private void OnLiveImageArrived(IAsyncResult result)
		{
			try
			{
				Image image = _system.EndGetLiveImage(result);

				_syncContext.Post(
					delegate
					{
						Size image_size = Gui.FitImageInControl(image.Size, picResultImage.Size);
						Image fitted_image = Gui.ResizeImageToBitmap(image, image_size);
						picResultImage.Image = fitted_image;
						picResultImage.Invalidate();

						if (cbLiveDisplay.Checked)
						{
							_system.BeginGetLiveImage(
								ImageFormat.jpeg,
								ImageSize.Sixteenth,
								ImageQuality.Medium,
								OnLiveImageArrived,
								null);
						}
					},
				null);
			}
			catch
			{
			}
		}

		private string GetReadStringFromResultXml(string resultXml)
		{
			try
			{
				XmlDocument doc = new XmlDocument();

				doc.LoadXml(resultXml);

				XmlNode full_string_node = doc.SelectSingleNode("result/general/full_string");

				if (full_string_node != null && _system != null && _system.State == Cognex.DataMan.SDK.ConnectionState.Connected)
				{
					XmlAttribute encoding = full_string_node.Attributes["encoding"];
					if (encoding != null && encoding.InnerText == "base64")
					{
						if (!string.IsNullOrEmpty(full_string_node.InnerText))
						{
							byte[] code = Convert.FromBase64String(full_string_node.InnerText);
							return _system.Encoding.GetString(code, 0, code.Length);
						}
						else
						{
							return "";
						}
					}

					return full_string_node.InnerText;
				}
			}
			catch
			{
			}

			return "";
		}

		private void ShowResult(ComplexResult complexResult)
		{
			List<Image> images = new List<Image>();
			List<string> image_graphics = new List<string>();
			string read_result = null;
			int result_id = -1;
			ResultTypes collected_results = ResultTypes.None;

			// Take a reference or copy values from the locked result info object. This is done
			// so that the lock is used only for a short period of time.
			lock (_currentResultInfoSyncLock)
			{
				foreach (var simple_result in complexResult.SimpleResults)
				{
					collected_results |= simple_result.Id.Type;

					switch (simple_result.Id.Type)
					{
						case ResultTypes.Image:
							Image image = ImageArrivedEventArgs.GetImageFromImageBytes(simple_result.Data);
							if (image != null)
								images.Add(image);
							break;

						case ResultTypes.ImageGraphics:
							image_graphics.Add(simple_result.GetDataAsString());
							break;

						case ResultTypes.ReadXml:
							read_result = GetReadStringFromResultXml(simple_result.GetDataAsString());
							result_id = simple_result.Id.Id;
							break;

						case ResultTypes.ReadString:
							read_result = simple_result.GetDataAsString();
							result_id = simple_result.Id.Id;
							break;
					}
				}
			}

			AddListItem(string.Format("Complex result arrived: resultId = {0}, read result = {1}", result_id, read_result));
			Log("Complex result contains", string.Format("{0}", collected_results.ToString()));

			if (images.Count > 0)
			{
				Image first_image = images[0];

				Size image_size = Gui.FitImageInControl(first_image.Size, picResultImage.Size);
				Image fitted_image = Gui.ResizeImageToBitmap(first_image, image_size);

				if (image_graphics.Count > 0)
				{
					using (Graphics g = Graphics.FromImage(fitted_image))
					{
						foreach (var graphics in image_graphics)
						{
							ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, image_size.Width, image_size.Height));
							ResultGraphicsRenderer.PaintResults(g, rg);
						}
					}
				}

				if (picResultImage.Image != null)
				{
					var image = picResultImage.Image;
					picResultImage.Image = null;
					image.Dispose();
				}

				picResultImage.Image = fitted_image;
				picResultImage.Invalidate();
			}

			if (read_result != null && read_result.Contains("NG") == false )
            {
                BankHost_main.ReaderData = lbReadString.Text = read_result;
                Result_string = read_result;
            }
            else
            {
                
            }
				
		}

        public string Read_Result()
        {
            return Result_string;
        }


        public void Result_Empty()
        {
            Result_string = "";
        }

		private void AddListItem(object item)
		{
			lock (_listAddItemLock)
			{
				listBox1.Items.Add(item);

				if (listBox1.Items.Count > 500)
					listBox1.Items.RemoveAt(0);

				if (listBox1.Items.Count > 0)
					listBox1.SelectedIndex = listBox1.Items.Count - 1;
			}
		}



        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
