using System;
using System.Collections.Generic;

namespace Lucuma
{
    public interface IGeoCodeResult
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        bool HasValue { get; }
        string Library { get; set; }
        List<Location> Locations { get; }
        int Count { get; }
        String Error { get; set; }
        String request { get; set; }
        String response { get; set; }
    }
}
