﻿using Darc_Euphoria.Euphoric;
using Darc_Euphoria.Euphoric.Config;
using Darc_Euphoria.Euphoric.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Darc_Euphoria.Hacks
{
    class FakeLag
    {
        private static int _lastLag = 0;
        public static void Start()
        {
            while(true)
            {
                if (gvar.isShuttingDown)
                {
                    while (gvar.SHUTDOWN != 3)
                        Thread.Sleep(1);

                    gvar.SHUTDOWN++;
                    Local.SendPackets = true;
                    break;
                }

                Thread.Sleep(1);
                if (!Local.InGame)
                {
                    if (Local.SendPackets == false)
                        Local.SendPackets = true;

                    Thread.Sleep(100);
                    continue;
                }

                if (!Settings.userSettings.MiscSettings.FakeLag)
                {
                    if (Local.SendPackets == false)
                        Local.SendPackets = true;

                    Thread.Sleep(100);
                    continue;
                }

                if ((WinAPI.GetAsyncKeyState(0x1) & 0x8000) > 0 || WinAPI.GetAsyncKeyState(0x1) > 0)
                {
                    if (Local.SendPackets == false)
                        Local.SendPackets = true;
                    continue;
                }

                var endLag = _lastLag + Settings.userSettings.MiscSettings.FakeLagAmount;
                if (endLag > Environment.TickCount)
                {
                    Local.SendPackets = false;
                    continue;
                }
                if (endLag + 20 > Environment.TickCount)
                {
                    Local.SendPackets = true;
                    continue;
                }
                _lastLag = Environment.TickCount;
            }
        }
    }
}
