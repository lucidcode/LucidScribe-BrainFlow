using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    public partial class ConnectForm : Form
    {
        public UserSettings Settings = new UserSettings() {
            Board = "BrainFlow - Synthetic"
        };
        private List<Board> boards = new List<Board>() { 
            new Board { 
                Id = -1, 
                Type = "BrainFlow", 
                Name = "Synthetic" }
        };
        private string lucidScribePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\lucidcode\Lucid Scribe\";

        public ConnectForm()
        {
            InitializeComponent();
        }

        private void ConnectForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadBoards();
                LoadSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LucidScribe.BrainFlow.Load()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBoards()
        {
            if (!File.Exists(lucidScribePath + @"Plugins\BrainFlow.Boards.lsd"))
            {
                return;
            }

            var json = File.ReadAllText(lucidScribePath + @"Plugins\BrainFlow.Boards.lsd");

            try
            {
                boards = JsonConvert.DeserializeObject<List<Board>>(json);

                foreach (var board in boards)
                {
                    boardComboBox.Items.Add($"{board.Type} - {board.Name}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LucidScribe.BrainFlow.LoadBoards()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(lucidScribePath + @"Plugins\BrainFlow.User.lsd"))
                {
                    var json = File.ReadAllText(lucidScribePath + @"Plugins\BrainFlow.User.lsd");
                    Settings = JsonConvert.DeserializeObject<UserSettings>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LucidScribe.BrainFlow.LoadSettings()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            boardComboBox.Text = Settings.Board;

            boardIdText.Text = Settings.BoardId.ToString();
            ipAddressText.Text = Settings.IpAddress;
            ipPortText.Text = Settings.IpPort.ToString();
            ipProtocolText.Text = Settings.IpProtocol.ToString();
            macAddressText.Text = Settings.MacAddress;
            serialPortText.Text = Settings.SerialPort;
            serialNumberText.Text = Settings.SerialNumber;
            otherInfoText.Text = Settings.OtherInfo;
            timeoutText.Text = Settings.Timeout.ToString();
            fileText.Text = Settings.File;
        }

        private void SaveSettings()
        {
            var settings = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(lucidScribePath + "Plugins\\BrainFlow.User.lsd", settings);
        }

        private void boardComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var board = boards.FirstOrDefault(b => ($"{b.Type} - {b.Name}") == boardComboBox.Text);

            if (board != null)
            {
                Settings.Board = $"{board.Type} - {board.Name}";
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

        private void boardIdText_TextChanged(object sender, EventArgs e)
        {
            Settings.BoardId = Convert.ToInt32(boardIdText.Text);
        }

        private void ipAddressText_TextChanged(object sender, EventArgs e)
        {
            Settings.IpAddress = ipAddressText.Text;
        }

        private void ipPortText_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(ipPortText.Text, out value);
            Settings.IpPort = value;
        }

        private void ipProtocolText_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(ipProtocolText.Text, out value);
            Settings.IpProtocol = value;
        }

        private void macAddressText_TextChanged(object sender, EventArgs e)
        {
            Settings.MacAddress = macAddressText.Text;
        }

        private void serialPortText_TextChanged(object sender, EventArgs e)
        {
            Settings.SerialPort = serialPortText.Text;
        }

        private void serialNumberText_TextChanged(object sender, EventArgs e)
        {
            Settings.SerialNumber = serialNumberText.Text;
        }

        private void otherInfoText_TextChanged(object sender, EventArgs e)
        {
            Settings.OtherInfo = otherInfoText.Text;
        }

        private void timeoutText_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(timeoutText.Text, out value);
            Settings.Timeout = value;
        }

        private void fileText_TextChanged(object sender, EventArgs e)
        {
            Settings.File = fileText.Text;
        }
    }
}
