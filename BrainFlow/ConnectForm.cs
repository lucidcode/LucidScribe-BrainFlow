using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    public partial class ConnectForm : Form
    {
        public string Algorithm = "REM Detection";
        public string BoardId;
        public string IpAddress;
        public string IpPort;
        public string IpProtocol;
        public string MacAddress;
        public string SerialPort;
        public string SerialNumber;
        public string OtherInfo;
        public string Timeout;
        public string FileInput;

        public int Threshold = 600;

        private List<Board> Boards;

        private string lucidScribePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\lucidcode\\Lucid Scribe\\";

        public ConnectForm()
        {
            InitializeComponent();
        }

        private void ConnectForm_Load(object sender, EventArgs e)
        {
            LoadBoards();
            LoadSettings();
        }

        private void LoadBoards()
        {
            if (!File.Exists("brainflow_boards.json"))
            {
                return;
            }

            var json = File.ReadAllText("brainflow_boards.json");
            Boards = JsonConvert.DeserializeObject<List<Board>>(json);

            foreach (var board in Boards)
            {
                boardComboBox.Items.Add($"{board.Type} - {board.Name}");
            }
        }

        private void LoadSettings()
        {
            XmlDocument xmlSettings = new XmlDocument();

            if (!File.Exists(lucidScribePath + "Plugins\\BrainFlow.User.lsd"))
            {
                String defaultSettings = "<LucidScribeData>\r\n";
                defaultSettings += "  <Plugin>\r\n";
                defaultSettings += "    <Algorithm>REM Detection</Algorithm>\r\n";
                defaultSettings += "    <Threshold>600</Threshold>\r\n";
                defaultSettings += "    <Board>BrainFlow - Synthetic</Board>\r\n";
                defaultSettings += "    <BoardId>-1</BoardId>\r\n";
                defaultSettings += "    <IpAddress></IpAddress>\r\n";
                defaultSettings += "    <IpPort></IpPort>\r\n";
                defaultSettings += "    <IpProtocol></IpProtocol>\r\n";
                defaultSettings += "    <MacAddress></MacAddress>\r\n";
                defaultSettings += "    <SerialPort></SerialPort>\r\n";
                defaultSettings += "    <SerialNumber></SerialNumber>\r\n";
                defaultSettings += "    <OtherInfo></OtherInfo>\r\n";
                defaultSettings += "    <Timeout></Timeout>\r\n";
                defaultSettings += "    <File></File>\r\n";
                defaultSettings += "  </Plugin>\r\n";
                defaultSettings += "</LucidScribeData>";
                File.WriteAllText(lucidScribePath + "Plugins\\BrainFlow.User.lsd", defaultSettings);
            }

            xmlSettings.Load(lucidScribePath + "Plugins\\BrainFlow.User.lsd");

            if (xmlSettings.DocumentElement.SelectSingleNode("//Board") != null)
            {
                boardComboBox.Text = xmlSettings.DocumentElement.SelectSingleNode("//Board").InnerText;
            }

            cmbAlgorithm.Text = xmlSettings.DocumentElement.SelectSingleNode("//Algorithm").InnerText;
            thresholdText.Text = xmlSettings.DocumentElement.SelectSingleNode("//Threshold").InnerText;
            boardIdText.Text = xmlSettings.DocumentElement.SelectSingleNode("//BoardId").InnerText;
            ipAddressText.Text = xmlSettings.DocumentElement.SelectSingleNode("//IpAddress").InnerText;
            ipPortText.Text = xmlSettings.DocumentElement.SelectSingleNode("//IpPort").InnerText;
            ipProtocolText.Text = xmlSettings.DocumentElement.SelectSingleNode("//IpProtocol").InnerText;
            macAddressText.Text = xmlSettings.DocumentElement.SelectSingleNode("//MacAddress").InnerText;
            serialPortText.Text = xmlSettings.DocumentElement.SelectSingleNode("//SerialPort").InnerText;
            serialNumberText.Text = xmlSettings.DocumentElement.SelectSingleNode("//SerialNumber").InnerText;
            otherInfoText.Text = xmlSettings.DocumentElement.SelectSingleNode("//OtherInfo").InnerText;
            timeoutText.Text = xmlSettings.DocumentElement.SelectSingleNode("//Timeout").InnerText;
            fileText.Text = xmlSettings.DocumentElement.SelectSingleNode("//File").InnerText;

            BoardId = boardIdText.Text;
            
            Threshold = Convert.ToInt32(thresholdText.Text);
            IpAddress = ipAddressText.Text;
            IpPort = ipPortText.Text;
            IpProtocol = ipProtocolText.Text;
            MacAddress = macAddressText.Text;
            SerialPort = serialPortText.Text;
            SerialNumber = serialNumberText.Text;
            OtherInfo = otherInfoText.Text;
            Timeout = timeoutText.Text;
            FileInput = fileText.Text;
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            Algorithm = cmbAlgorithm.Text;
        }

        private void SaveSettings()
        {
            String settings = "<LucidScribeData>\r\n";
            settings += "  <Plugin>\r\n";
            settings += "    <Algorithm>" + cmbAlgorithm.Text + "</Algorithm>\r\n";
            settings += "    <Threshold>" + thresholdText.Text + "</Threshold>\r\n";
            settings += "    <Board>" + boardComboBox.Text + "</Board>\r\n";
            settings += "    <BoardId>" + boardIdText.Text + "</BoardId>\r\n";
            settings += "    <IpAddress>" + ipAddressText.Text + "</IpAddress>\r\n";
            settings += "    <IpPort>" + ipPortText.Text + "</IpPort>\r\n";
            settings += "    <IpProtocol>" + ipProtocolText.Text + "</IpProtocol>\r\n";
            settings += "    <MacAddress>" + macAddressText.Text + "</MacAddress>\r\n";
            settings += "    <SerialPort>" + serialPortText.Text + "</SerialPort>\r\n";
            settings += "    <SerialNumber>" + serialNumberText.Text + "</SerialNumber>\r\n";
            settings += "    <OtherInfo>" + otherInfoText.Text + "</OtherInfo>\r\n";
            settings += "    <Timeout>" + timeoutText.Text + "</Timeout>\r\n";
            settings += "    <File>" + fileText.Text + "</File>\r\n";
            settings += "  </Plugin>\r\n";
            settings += "</LucidScribeData>";
            File.WriteAllText(lucidScribePath + "Plugins\\BrainFlow.User.lsd", settings);
        }

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            Threshold = Convert.ToInt32(thresholdText.Text);
        }

        private void boardIdText_TextChanged(object sender, EventArgs e)
        {
            BoardId = boardIdText.Text;
        }

        private void ipAddressText_TextChanged(object sender, EventArgs e)
        {
            IpAddress = ipAddressText.Text;
        }

        private void ipPortText_TextChanged(object sender, EventArgs e)
        {
            IpPort = ipPortText.Text;
        }

        private void ipProtocolText_TextChanged(object sender, EventArgs e)
        {
            IpProtocol = ipProtocolText.Text;
        }

        private void macAddressText_TextChanged(object sender, EventArgs e)
        {
            MacAddress = macAddressText.Text;
        }

        private void serialPortText_TextChanged(object sender, EventArgs e)
        {
            SerialPort = serialPortText.Text;
        }

        private void serialNumberText_TextChanged(object sender, EventArgs e)
        {
            SerialNumber = serialNumberText.Text;
        }

        private void otherInfoText_TextChanged(object sender, EventArgs e)
        {
            OtherInfo = otherInfoText.Text;
        }

        private void timeoutText_TextChanged(object sender, EventArgs e)
        {
            Timeout = timeoutText.Text;
        }

        private void fileText_TextChanged(object sender, EventArgs e)
        {
            FileInput = fileText.Text;
        }

        private void boardComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var board = Boards.FirstOrDefault(b => ($"{b.Type} - {b.Name}") == boardComboBox.Text);

            if (board != null)
            {
                boardIdText.Text = board.Id.ToString();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
