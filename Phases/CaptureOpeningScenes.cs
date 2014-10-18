﻿// This file is part of iRacingReplayOverlay.
//
// Copyright 2014 Dean Netherton
// https://github.com/vipoo/iRacingReplayOverlay.net
//
// iRacingReplayOverlay is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// iRacingReplayOverlay is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with iRacingReplayOverlay.  If not, see <http://www.gnu.org/licenses/>.
//

using iRacingReplayOverlay.Phases.Direction;
using iRacingSDK;
using iRacingSDK.Support;
using System;
using System.Linq;
using System.Threading;

namespace iRacingReplayOverlay.Phases
{
    public partial class IRacingReplay
    {
        void _CaptureOpeningScenes(Action onComplete)
        {
            var data = iRacing.GetDataFeed().First();
            var session = data.SessionData.SessionInfo.Sessions.Qualifying();
            if (session == null || session.ResultsPositions == null)
                return;

            if (!iRacing.Replay.AttempToMoveToQualifyingSection())
                return;
            
            data = iRacing.GetDataFeed().First();
            var f = data.Telemetry.ReplayFrameNum;
            iRacing.Replay.MoveToFrame(f + 60 * 4);
            iRacing.Replay.SetSpeed(1);

            var camera = data.SessionData.CameraInfo.Groups.FirstOrDefault( c => c.GroupName == "Scenic");
            if( camera == null )
                camera = data.SessionData.CameraInfo.Groups.FirstOrDefault( c => c.GroupName == "Pit Lane 2");
            if( camera == null )
                camera = data.SessionData.CameraInfo.Groups.FirstOrDefault( c => c.GroupName == "Chopper");
            if( camera == null)
                camera = data.SessionData.CameraInfo.Groups.FirstOrDefault( c => c.GroupName == "TV3");
            if( camera == null)
                throw new Exception("Cant find a camera to use for pre-race capture");

            var scenicCameras = camera.GroupNum;
            var aCar = data.SessionData.DriverInfo.Drivers[1].CarNumberRaw;
            iRacing.Replay.CameraOnDriver((short)aCar, (short)scenicCameras);

            var videoCapture = new VideoCapture();

            videoCapture.Activate(workingFolder);

            Thread.Sleep(shortTestOnly ? 5000 : 20000);

            var fileName = videoCapture.Deactivate();

            TraceInfo.WriteLine("Captured intro video into file {0}", fileName);
            _WithIntroVideo(fileName);
        }
    }
}
