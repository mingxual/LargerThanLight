using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class EventTime
{
    public static string GetTime(DateTime from)
    {
        return GetPacificStandardTime(from).ToString("MM/dd HH:mm:ss");
    }
    public static DateTime GetPacificStandardTime(DateTime from)
    {
        //find TimeZoneInfo of PST
        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        //Convert time from Local to PST
        return TimeZoneInfo.ConvertTime(from, TimeZoneInfo.Local, tzi);
    }
}
