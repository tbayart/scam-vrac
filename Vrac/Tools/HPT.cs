using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

/// <summary>Fournit un compteur de performance avec une précision allant jusqu'à la <b>graduation</b>.</summary>
/// <remarks>La <b>graduation</b> est la plus petite unité de temps disponible et vaut 100 nanosecondes.</remarks>
[Serializable()]
public class HighPerfTimer
{
    [DllImport("kernel32")]
    private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
    [DllImport("kernel32")]
    private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    public static object SyncRoot= new object();

    private long m_lStart;
    private long m_lStop;
    private long m_lFrequency;

    /// <summary>Initialise une nouvelle instance de l'objet <see cref="HighPerfTimer"/>.</summary>
    public HighPerfTimer()
    {
        this.m_lStart = 0;
        this.m_lStop = 0;
        if (QueryPerformanceFrequency(out this.m_lFrequency) == false)
        {
            throw new Win32Exception(0, "Fréquence non supportée.");
        }
    }

    /// <summary>Démarre le compteur pour l'instance courante de l'objet <see cref="HighPerfTimer"/>.</summary>
    public void start()
    {
        QueryPerformanceCounter(out this.m_lStart);
    }

    /// <summary>Arrête le compteur pour l'instance courante de l'objet <see cref="HighPerfTimer"/>.</summary>
    public void stop()
    {
        QueryPerformanceCounter(out this.m_lStop);
    }

    /// <summary>Renvoie la durée écoulée en secondes, entre les appels aux méthodes <see cref="start"/> et <see cref="stop"/> pour l'instance courante de l'objet <see cref="HighPerfTimer"/>.</summary>
    public double gs_duration
    {
        get { return ((double)this.m_lStop - (double)this.m_lStart) / (double)this.m_lFrequency; }
    }

    /// <summary>Renvoie la durée écoulée en graduations, entre les appels aux méthodes <see cref="start"/> et <see cref="stop"/> pour l'instance courante de l'objet <see cref="HighPerfTimer"/>.</summary>
    public long gs_ticks
    {
        get { return (long)(((double)this.m_lStop - (double)this.m_lStart) / (double)this.m_lFrequency * 10000000); }
    }

    private static void trace(string message)
    {
        lock (SyncRoot)
        {
            File.AppendAllText(@".\temp\Trace.csv", message + Environment.NewLine);
        }
    }

    private static void addField(ref string message, string field)
    {
        message += ";" + field;
    }

    public static void timePoint(object desc)
    {
        DateTime d = DateTime.Now;
        string time = d.Hour+":"+d.Minute+":"+d.Second+"."+d.Millisecond;
        addField(ref time, desc.ToString());
        addField(ref time, "");
        trace(time);
    }

    public static T timeFunc<T, P1, P2>(Func<P1, P2, T> a, P1 p1, P2 p2, string desc)
    {
        HighPerfTimer hpt = new HighPerfTimer();
        hpt.start();
        T toRet = a(p1, p2);
        hpt.stop();
        TimeSpan ts = new TimeSpan(hpt.gs_ticks);
        string time = DateTime.Now.ToShortTimeString();
        addField(ref time, desc);
        addField(ref time, ts.TotalMilliseconds.ToString());
        trace(time);
        return toRet;
    }

    public static TimeSpan time<T>(Action<T> a, T p, string desc)
    {
        HighPerfTimer hpt = new HighPerfTimer();
        hpt.start();
        a(p);
        hpt.stop();
        TimeSpan ts = new TimeSpan(hpt.gs_ticks);
        string time = DateTime.Now.ToShortTimeString();
        addField(ref time, desc);
        addField(ref time, ts.TotalMilliseconds.ToString());
        trace(time);
        return ts;
    }
    public static TimeSpan time<P1, P2>(Action<P1, P2> a, P1 p1, P2 p2, string desc)
    {
        HighPerfTimer hpt = new HighPerfTimer();
        hpt.start();
        a(p1,p2);
        hpt.stop();
        TimeSpan ts = new TimeSpan(hpt.gs_ticks);
        string time = DateTime.Now.ToShortTimeString();
        addField(ref time, desc);
        addField(ref time, ts.TotalMilliseconds.ToString());
        trace(time);
        return ts;
    }
    public static TimeSpan time(Action a, string desc)
    {
        HighPerfTimer hpt = new HighPerfTimer();
        hpt.start();
        a();
        hpt.stop();
        TimeSpan ts = new TimeSpan(hpt.gs_ticks);
        string time = DateTime.Now.ToShortTimeString();
        addField(ref time, desc);
        addField(ref time, ts.TotalMilliseconds.ToString());
        trace(time);
        return ts;
    }
    public static TimeSpan time(string desc)
    {
        string time = DateTime.Now.ToShortTimeString();
        addField(ref time, desc);
        addField(ref time, "");
        trace(time);
        return new TimeSpan(0);
    }

}