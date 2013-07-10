using System;
using System.Collections;
using System.Threading;
using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Math = System.Math;

namespace DemoPwmLedPhase
{
    public class Program
    {
        private static Boolean _paused = true;
        private static readonly object SPauseLock = new object();
        public const Int64 TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

        public static double Map(double x, double inMin, double inMax, double outMin, double outMax)
        {
            return (x - inMin)*(outMax - outMin)/(inMax - inMin) + outMin;
        }

        public static void Pause()
        {
            lock (SPauseLock)
            {
                _paused = true;
            }
        }

        public static void UnPause()
        {
            lock (SPauseLock)
            {
                _paused = false;
            }
        }

        public static void TogglePause()
        {
            lock (SPauseLock)
            {
                _paused = !_paused;
            }
        }

        public static Boolean Paused()
        {
            Boolean result;
            lock (SPauseLock)
            {
                result = _paused;
            }
            return result;
        }

        public static void IntButton_OnInterrupt(uint port, uint state, DateTime time)
        {
            if (state == 0) return;
            TogglePause();
        }

        public static void Main()
        {
            var intButton =
                new InterruptPort(Stm32F4Discovery.ButtonPins.User, true,
                Port.ResistorMode.PullDown,
                Port.InterruptMode.InterruptEdgeBoth);
            intButton.OnInterrupt += IntButton_OnInterrupt;

            Pause();

            var accelerometerThread = new Thread(AccelerometerThread);
            accelerometerThread.Start();
            Debug.Print("accelerometerThread Started");

            var pwmThreadHandler = new Thread(PWMThread);
            pwmThreadHandler.Start();
            Debug.Print("PWMThread Started");

            UnPause();

            Thread.Sleep(Timeout.Infinite);
        }

        // ReSharper disable FunctionNeverReturns
        public static void AccelerometerThread()
        {
            var accelerometer = new LIS302DL(Stm32F4Discovery.Pins.PE3);
            while (true)
            {
                Debug.Print("X: " + accelerometer.Xvalue + " Y: " + accelerometer.Yvalue + " Z: " +
                            accelerometer.Zvalue);
                Thread.Sleep(100);
            }
        }
        // ReSharper restore FunctionNeverReturns

        public static double GetBrightnessPercent(int point, int head, int length)
        {
            var tail = head - length;
            var invTail = 360 - length + head;
            if (point > head && point < invTail)
            {
                return 0;
            }
            var mapPoint = Map(point, tail, head, 0, 100);
            return mapPoint;
        }

        public static double GetOffsetDegrees(double point, double head, double taillength) {
            // return the distance from the head in degrees 
            // or -1 if outside
		    var tail = head - taillength;
		    if (tail < 0) tail += 360;
		    double offset = -1;

		    if (head < tail) {
			    if (point <= head) {
				    offset = (head - point);
			    }
			    if (point >= tail) {
				    offset = (head + (360 - point));
			    }
		    } else {
			    if (point >= tail & point <= head) {
				    offset = (head - point);
			    }
		    }
		    return offset;
	    }

        public static long GetCurrentTimeInTicks()
        {
            return Utility.GetMachineTime().Ticks;
        }

        public static double GetOffsetPercent(double point, double head, double taillength)
        {
            // return the distance from the tail in percent
            // or the offset if the offset is < 0
            var offset = GetOffsetDegrees(point, head, taillength);
            var result = offset >= 0 ? Map(offset, 0, taillength, 1, 0) : offset;
            if (result > 1) return 1;
            return ( result < 0 ) && ( result != -1D ) ? 0.0 : result;
        }
        
        public static void PauseForMilliseconds(Int64 milliseconds)
        {
            PauseForMicroseconds(milliseconds * 1000);
        }

        public static void PauseForMicroseconds(Int64 microseconds)
        {
            var ts = new SpinWaitTimer();
            ts.WaitMicroseconds(microseconds);
        }

        public static double GetBrightnessCurve(double percent, int example = 0)
        {
            // P1 = 0,0
            // C1 = 0.49,0.06
            // P2 = 1,1
            // C2 = 0.44,0.98
            double brightness;

            // Some examples of calculating the brightness curve
            switch (example)
            {
                case 0:
                    brightness = -3*Math.Pow((percent - .66), 4) - 1.3653*(percent - 1.029);
                    break;
                case 1:
                    brightness = -5*Math.Pow((percent - .79), 4) - 2.5*(percent - 0.79);
                    break;
                case 2:
                    brightness = -1.4 * Math.Pow((percent - 1),12) - 1.37 * (percent - 1);
                    break;
                case 3:
                    {
                    var points = new[]
                        {
                            new Point(1, 1),
                            new Point(0.44, 0.98),
                            new Point(0.49, 0.06),
                            new Point(0, 0)
                        };
                    brightness = Curves.GetBezierPoint(percent, points).Y;
                    }
                    break;
                case 4:
                    {
                        var points = new[]
                            {
                                // \draw (0,0) .. controls (0.5,0.0) and (1,2.2) .. (1,0)
                                new Point(0.0, 0.0),
                                new Point(0.5,0.0),
                                new Point(1.0,2.2),
                                new Point(1.0,0.0)
                            };
                        brightness = Curves.GetBezierPoint(percent, points).Y;
                    }
                    break;
                case 5:
                    {
                        var points = new[]
                            {
                                new Point(0, 0),
                                new Point(0,1.3),
                                new Point(1,1.3),
                                new Point(1,0)
                            };
                        brightness = Curves.GetBezierPoint(percent, points).Y;
                    }
                    break;
                case 6:
                    {
                        // \draw (0,0) .. controls (0.6,0.1) and (1,2.2) .. (1,0);
                        var points = new[]
                            {
                                new Point(0.0, 0.0),
                                new Point(0.6, 0.1),
                                new Point(1.0, 2.2),
                                new Point(1.0, 0.0)
                            };
                        brightness = Curves.GetBezierPoint(percent, points).Y;
                        if (brightness > 1 || brightness < 0)
                        {
                            Debug.Print("Something went wrong...");
                        }
                    }
                    break;
                case 7:
                    {
                        // \draw (1,0) .. controls (-0.3,0) and (0.2,1) .. (0,1);
                        var points = new[]
                            {
                                new Point(1,0),
                                new Point(-0.3,0),
                                new Point(0.2,1),
                                new Point(0,1)
                            };
                        brightness = Curves.GetBezierPoint(percent, points).Y;
                    }
                    break;
                case 99:
                    {
                        var points = new[]
                            {
                                new Point(1, 0.75),
                                new Point(1, 1),
                                new Point(0.44, 0.98),
                                new Point(0.49, 0.06),
                                new Point(0, 0)
                            };
                        brightness = Curves.GetBezierPoint(percent, points).Y;
                    }
                    break;
                default:
                    brightness = percent;
                    break;
            }
            if (brightness > 1) brightness = 1;
            if (brightness < 0) brightness = 0;
            return brightness;
        }

        // ReSharper disable FunctionNeverReturns
        public static void PWMThread()
        {
            var pwmPins = new ArrayList
                {
                    new PWMStruct(new Microsoft.SPOT.Hardware.PWM(Stm32F4Discovery.PWMChannels.BlueLED, 300, 0, false), 0),
                    new PWMStruct(new Microsoft.SPOT.Hardware.PWM(Stm32F4Discovery.PWMChannels.GreenLED, 300, 0, false), 90),
                    new PWMStruct(new Microsoft.SPOT.Hardware.PWM(Stm32F4Discovery.PWMChannels.OrangeLED, 300, 0, false), 180),
                    new PWMStruct(new Microsoft.SPOT.Hardware.PWM(Stm32F4Discovery.PWMChannels.RedLED, 300, 0, false), 270)
                };

            foreach (var pwmPin in pwmPins)
            {
                ((PWMStruct) pwmPin).Pwm.Start();
            }

            var head = 0.0;
            const double maxTailLength = 300;
            var tail = maxTailLength;
            const int maxDegrees = 360;
            const double step = 1.0;
            const int delay = 0; // us
            const int animationSelector = 7;

            while (true)
            {
                // check if user button was pressed
                if (Paused())
                {
                    if (tail <= step) continue;
                    foreach (var pwmPin in pwmPins)
                    {
                        var pwm = (PWMStruct)pwmPin;
                        pwm.LastOffset = GetOffsetPercent(pwm.Position, head, tail);
                        var brightness = GetBrightnessCurve(pwm.LastOffset, animationSelector);
                        //brightness = Math.Pow(10, (2.55*brightness - 1)/84.33 - 1);
                        pwm.Pwm.DutyCycle = pwm.LastOffset < 0 ? 0 : brightness;
                    }
                    tail -= step;
                    PauseForMicroseconds(delay);
                    continue;
                }
                tail += step;
                head += step;
                if (tail > maxTailLength) tail = maxTailLength;
                // reset to 0 if passed 359
                if (head >= maxDegrees) head = 0;
                foreach (var pwmPin in pwmPins)
                {
                    var pwm = (PWMStruct) pwmPin;
                    pwm.LastOffset = GetOffsetPercent(pwm.Position, head, tail);
                    var brightness = GetBrightnessCurve(pwm.LastOffset, animationSelector);
                    //brightness = Math.Pow(10, (2.55 * brightness - 1) / 84.33 - 1);
                    pwm.Pwm.DutyCycle = pwm.LastOffset < 0 ? 0 : brightness;
                }
                PauseForMicroseconds(delay);
            }
        }
        // ReSharper restore FunctionNeverReturns

        public class PWMStruct
        {
            public Microsoft.SPOT.Hardware.PWM Pwm;
            public int Position;
            public double LastOffset;

            public PWMStruct(Microsoft.SPOT.Hardware.PWM pwm, int position)
            {
                Pwm = pwm;
                Position = position;
                LastOffset = 0;
            }

        }

    }




}
