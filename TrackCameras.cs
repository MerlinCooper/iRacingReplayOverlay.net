﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRacingReplayOverlay
{
    public class TrackCameras : List<TrackCamera>
    {

    }

    public class TrackCamera
    {
        public TrackCamera()
        {

        }
        public string TrackName;
        public string CameraName;
        public int Ratio;
    }
}