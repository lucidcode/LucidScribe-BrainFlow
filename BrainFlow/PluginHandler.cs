﻿using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using brainflow;
using brainflow.math;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    public static class Device
    {
        static bool Initialized;
        static bool InitError;
        static bool Disposing;

        static BoardShim boardShim;
        static int[] eegChannels;
        static Thread boardThread;

        static double[] eegValues;
        static double[] eegTicks;
        static bool[] clearValues;

        static int channels = 2;
        public static string Algorithm = "REM Detection";
        public static int Threshold = 600;
        public static int BlinkInterval = 28;

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
            eegChannels = new int[channels];

            if (!Initialized & !InitError)
            {
                ConnectForm connectForm = new ConnectForm();
                if (connectForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Algorithm = connectForm.Algorithm;
                        Threshold = connectForm.Threshold;

                        int boardId = Convert.ToInt32(connectForm.BoardId);
                        BrainFlowInputParams inputParams = new BrainFlowInputParams();
                        inputParams.ip_address = connectForm.IpAddress;
                        inputParams.ip_port = Convert.ToInt32(connectForm.IpPort);
                        inputParams.ip_protocol = Convert.ToInt32(connectForm.IpProtocol);
                        inputParams.mac_address = connectForm.MacAddress;
                        inputParams.serial_port = connectForm.SerialPort;
                        inputParams.serial_number = connectForm.SerialNumber;
                        inputParams.timeout = Convert.ToInt32(connectForm.Timeout);
                        inputParams.other_info = connectForm.OtherInfo;
                        inputParams.file = connectForm.FileInput;

                        boardThread = new Thread(() => GetBoardData(boardId, inputParams));
                        boardThread.Start();
                    }
                    catch (Exception ex)
                    {
                        if (!InitError)
                        {
                            InitError = true;
                            MessageBox.Show(ex.Message, "LucidScribe.InitializePlugin()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    InitError = true;
                    return false;
                }

                Initialized = true;
            }
            return true;
        }

        static void GetBoardData(int board_id, BrainFlowInputParams input_params)
        {
            boardShim = new BoardShim(board_id, input_params);

            boardShim.prepare_session();
            boardShim.start_stream();
            Thread.Sleep(640);

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
            while (!Disposing);
        }

        public static void Dispose()
        {
            Disposing = true;
            if (Initialized)
            {
                boardShim.stop_stream();
                boardShim.release_session();
                Initialized = false;
            }
        }

        public static double GetEEG(int index)
        {
            if (eegTicks.Length <= index - 1) return 0;
            if (eegTicks[index - 1] == 0) return 0;
            double average = eegValues[index - 1] / eegTicks[index - 1];
            return average;
        }

        public static void ClearEEG(int index)
        {
            if (clearValues.Length <= index - 1) return;
            clearValues[index - 1] = true;
        }
    }
}