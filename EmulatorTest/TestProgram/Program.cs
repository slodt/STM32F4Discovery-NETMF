using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace TestProgram
{
    public class Program
    {
        private static readonly OutputPort[] Leds = new[]
                           {
                               new OutputPort((Cpu.Pin) 3, false),
                               new OutputPort((Cpu.Pin) 4, false),
                               new OutputPort((Cpu.Pin) 5, false),
                               new OutputPort((Cpu.Pin) 6, false),
                               new OutputPort((Cpu.Pin) 7, false)
                           };

        private static OutputPort _prevLed;
        private static int _ledIndex;
        private static InterruptPort _prevButton;
        private static InterruptPort _nextButton;
        private static SerialPort _com1;

        public static void Main()
        {
            using (_com1 = new SerialPort(Serial.COM1))
            {
                _com1.Open();
                Send("Hello\r\n");

                _prevButton = new InterruptPort((Cpu.Pin) 18, false, Port.ResistorMode.PullDown,
                                                Port.InterruptMode.InterruptEdgeLevelHigh);
                _nextButton = new InterruptPort((Cpu.Pin) 19, false, Port.ResistorMode.PullDown,
                                                Port.InterruptMode.InterruptEdgeLevelHigh);

                _prevButton.OnInterrupt += prevButton_OnInterrupt;
                _nextButton.OnInterrupt += nextButton_OnInterrupt;

                _ledIndex = 2;
                _prevLed = Leds[_ledIndex];
                _prevLed.Write(true);

                Thread.Sleep(Timeout.Infinite);
            }
        }

        static void nextButton_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            _ledIndex++;
            ChangeLed();
            Send(_ledIndex.ToString()+"\r\n");
            _nextButton.ClearInterrupt();
        }

        private static void Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _com1.Write(data, 0, data.Length);
        }

        static void prevButton_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            _ledIndex--;
            ChangeLed();
            Send(_ledIndex.ToString() + "\r\n");
            _prevButton.ClearInterrupt();
        }

        private static void ChangeLed()
        {
            if (_ledIndex >= Leds.Length)
                _ledIndex = 0;
            else
            {
                if (_ledIndex < 0)
                    _ledIndex = Leds.Length - 1;
            }

            _prevLed.Write(false);
            _prevLed = Leds[_ledIndex];
            _prevLed.Write(true);
        }
    }
}
