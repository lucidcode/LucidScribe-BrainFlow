using System;
using System.Threading;
using System.Windows.Forms;
using brainflow;
using brainflow.math;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    public static class Device
    {
        static bool initialized;
        static bool initError;
        static bool disposing;

        static BoardShim boardShim;
        static int[] eegChannels;
        static Thread boardThread;

        static double[] eegValues;
        static double[] eegTicks;
        static bool[] clearValues;

        public static EventHandler<EventArgs> Channel1Changed;
        public static EventHandler<EventArgs> Channel2Changed;
        public static EventHandler<EventArgs> Channel3Changed;
        public static EventHandler<EventArgs> Channel4Changed;
        public static EventHandler<EventArgs> Channel5Changed;
        public static EventHandler<EventArgs> Channel6Changed;
        public static EventHandler<EventArgs> Channel7Changed;
        public static EventHandler<EventArgs> Channel8Changed;
        public static EventHandler<EventArgs> Channel9Changed;
        public static EventHandler<EventArgs> Channel10Changed;
        public static EventHandler<EventArgs> Channel11Changed;
        public static EventHandler<EventArgs> Channel12Changed;
        public static EventHandler<EventArgs> Channel13Changed;
        public static EventHandler<EventArgs> Channel14Changed;
        public static EventHandler<EventArgs> Channel15Changed;
        public static EventHandler<EventArgs> Channel16Changed;

        public static Boolean Initialize()
        {
            if (!initialized & !initError)
            {
                ConnectForm connectForm = new ConnectForm();
                if (connectForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int boardId = Convert.ToInt32(connectForm.Settings.BoardId);
                        BrainFlowInputParams inputParams = new BrainFlowInputParams();
                        inputParams.ip_address = connectForm.Settings.IpAddress ?? "";
                        inputParams.ip_port = connectForm.Settings.IpPort;
                        inputParams.ip_protocol = connectForm.Settings.IpProtocol;
                        inputParams.mac_address = connectForm.Settings.MacAddress ?? "";
                        inputParams.serial_port = connectForm.Settings.SerialPort ?? "";
                        inputParams.serial_number = connectForm.Settings.SerialNumber ?? "";
                        inputParams.timeout = connectForm.Settings.Timeout;
                        inputParams.other_info = connectForm.Settings.OtherInfo ?? "";
                        inputParams.file = connectForm.Settings.File ?? "";

                        boardThread = new Thread(() => GetBoardData(boardId, inputParams));
                        boardThread.Start();

                        initialized = true;
                    }
                    catch (Exception ex)
                    {
                        if (!initError)
                        {
                            initError = true;
                            MessageBox.Show(ex.Message, "LucidScribe.InitializePlugin()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    initError = true;
                    return false;
                }
            }
            return true;
        }

        static void GetBoardData(int board_id, BrainFlowInputParams input_params)
        {
            boardShim = new BoardShim(board_id, input_params);

            boardShim.prepare_session();
            boardShim.start_stream();

            eegChannels = BoardShim.get_eeg_channels(board_id);
            eegValues = new double[eegChannels.Length];
            eegTicks = new double[eegChannels.Length];
            clearValues = new bool[eegChannels.Length];

            do
            {
                double[,] unprocessed_data = boardShim.get_board_data(20);
                foreach (var index in eegChannels)
                {
                    double[] rows = unprocessed_data.GetRow(index);

                    foreach (var row in rows)
                    {
                        eegValues[index - 1] += row;
                        eegTicks[index - 1]++;
                    }

                    if (index == 1 && Channel1Changed != null) Channel1Changed(string.Join(",", rows), null);
                    if (index == 2 && Channel2Changed != null) Channel2Changed(string.Join(",", rows), null);
                    if (index == 3 && Channel3Changed != null) Channel3Changed(string.Join(",", rows), null);
                    if (index == 4 && Channel4Changed != null) Channel4Changed(string.Join(",", rows), null);
                    if (index == 5 && Channel5Changed != null) Channel5Changed(string.Join(",", rows), null);
                    if (index == 6 && Channel6Changed != null) Channel6Changed(string.Join(",", rows), null);
                    if (index == 7 && Channel7Changed != null) Channel7Changed(string.Join(",", rows), null);
                    if (index == 8 && Channel8Changed != null) Channel8Changed(string.Join(",", rows), null);
                    if (index == 9 && Channel9Changed != null) Channel9Changed(string.Join(",", rows), null);
                    if (index == 10 && Channel10Changed != null) Channel10Changed(string.Join(",", rows), null);
                    if (index == 11 && Channel11Changed != null) Channel11Changed(string.Join(",", rows), null);
                    if (index == 12 && Channel12Changed != null) Channel12Changed(string.Join(",", rows), null);
                    if (index == 13 && Channel13Changed != null) Channel13Changed(string.Join(",", rows), null);
                    if (index == 14 && Channel14Changed != null) Channel14Changed(string.Join(",", rows), null);
                    if (index == 15 && Channel15Changed != null) Channel15Changed(string.Join(",", rows), null);
                    if (index == 16 && Channel16Changed != null) Channel16Changed(string.Join(",", rows), null);
                }

                foreach (var index in eegChannels)
                {
                    if (clearValues[index -1])
                    {
                        eegValues[index - 1] = 0;
                        eegTicks[index - 1] = 0;
                        clearValues[index - 1] = false;
                    }
                }
            }
            while (!disposing);
        }

        public static void Dispose()
        {
            disposing = true;
            if (initialized)
            {
                boardShim.stop_stream();
                boardShim.release_session();
                initialized = false;
            }
        }

        public static double GetEEG(int index)
        {
            if (!initialized) return 0;
            if (eegTicks.Length <= index - 1) return 0;
            if (eegTicks[index - 1] == 0) return 0;
            double average = eegValues[index - 1] / eegTicks[index - 1];
            return average;
        }

        public static void ClearEEG(int index)
        {
            if (!initialized) return;
            if (clearValues.Length <= index - 1) return;
            clearValues[index - 1] = true;
        }
    }
}
