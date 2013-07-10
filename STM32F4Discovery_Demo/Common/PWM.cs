/* Copyright (C) 2010 Secret Labs LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System;
using Microsoft.SPOT.Hardware;

namespace Common
{
    public class PWM : IDisposable
    {
        readonly Microsoft.SPOT.Hardware.PWM _pwm;

        public Boolean Disposed = false;

        public PWM(Cpu.Pin pin)
        {
            var channel = GetChannelFromPin(pin);
            _pwm = new Microsoft.SPOT.Hardware.PWM(channel, 100, 0, Microsoft.SPOT.Hardware.PWM.ScaleFactor.Microseconds, false);
        }

        ~PWM()
        {
            Dispose();
        }

        public void Dispose()
        {
            _pwm.Dispose();
        }

        static private Cpu.PWMChannel GetChannelFromPin(Cpu.Pin pin)
        {
            switch ((uint)pin)
            {
                case 60:
                    return Cpu.PWMChannel.PWM_0;
                case 61:
                    return Cpu.PWMChannel.PWM_1;
                case 62:
                    return Cpu.PWMChannel.PWM_2;
                case 63:
                    return Cpu.PWMChannel.PWM_3;
                case 73:
                    return Cpu.PWMChannel.PWM_4;
                case 75:
                    return Cpu.PWMChannel.PWM_5;
                case 77:
                    return Cpu.PWMChannel.PWM_6;
                case 78:
                    return Cpu.PWMChannel.PWM_7;
                default:
                    return Cpu.PWMChannel.PWM_NONE;
            }
        }

        public void SetDutyCycle(UInt32 dutyCycle)
        {
            if (Disposed)
                throw new ObjectDisposedException();

            _pwm.DutyCycle = dutyCycle / 100.0;
        }

        public void SetPulse(UInt32 period, UInt32 duration)
        {
            if (Disposed)
                throw new ObjectDisposedException();

            _pwm.Period = period;
            _pwm.Duration = duration;
        }
    }
}