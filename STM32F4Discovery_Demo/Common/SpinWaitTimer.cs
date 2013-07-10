using System;
using Microsoft.SPOT;

namespace Common
{
    public class SpinWaitTimer
    {
        double _cyclesPerSecond = 112262.2255516001;

        public double CyclesPerSecond
        {
            get { return _cyclesPerSecond; }
            set { _cyclesPerSecond = value; }
        }

        public SpinWaitTimer()
        {
            // Calibrate();
        }

        public void Calibrate()
        {
            const int cycleCount = 1048576;
            var dummyValue = 0;
            var startTime = DateTime.Now;
            for (var i = 0; i < cycleCount; ++i)
            {
                ++dummyValue;
            }
            var endTime = DateTime.Now;

            var timeDifference = endTime.Subtract(startTime);

            _cyclesPerSecond = (cycleCount / (double)timeDifference.Ticks) * 10000000d;

        }

        public void WaitSeconds(double sec)
        {
            var cycleCount = (int)((sec * CyclesPerSecond));
            var dummyValue = 0;
            for (var i = 0; i < cycleCount; ++i)
            {
                ++dummyValue;
            }
        }

        public void WaitMilliseconds(double milliseconds)
        {
            var cycleCount = (int)(milliseconds * CyclesPerSecond / 1000d);
            var dummyValue = 0;
            for (var i = 0; i < cycleCount; ++i)
            {
                ++dummyValue;
            }
        }

        public void WaitMicroseconds(double microseconds)
        {
            var cycleCount = (int)(microseconds * CyclesPerSecond / 1000000d);
            var dummyValue = 0;
            for (var i = 0; i < cycleCount; ++i)
            {
                ++dummyValue;
            }
        }
    }
}
