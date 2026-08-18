using aniliberty.Api.Data.Releases;
using aniliberty.Api.Data.Releases.Episodes;
using FlyleafLib.MediaPlayer;
using System.Collections.ObjectModel;
using System.Linq;

namespace aniliberty.Pages.Helpers;

internal class PlayerData
{
    public ReleaseDetail? Release { get; set; }
    public Episode? Episode { get; set; }
    //public string HLS;
    //public int Timecode;

    public bool IsEpisodeLast { get { return Release?.Episodes.FindLastIndex((e) => e.Ordinal == Episode?.Ordinal) == Release.Episodes.Count - 1; } }
    public bool IsEpisodeFirst { get { return Release?.Episodes.FindIndex((e) => e.Ordinal == Episode?.Ordinal) == 0; } }

    public bool IsEpisodeNotLast { get { return !IsEpisodeLast; } }
    public bool IsEpisodeNotFirst { get { return !IsEpisodeFirst; } }
}

internal class PlaylistData
{
    public decimal Current { get; set; } = 0;
    public ObservableCollection<Episode> Playlist { get; set; } = [];

    public Episode? GetCurrentEpisode()
    {
        return Playlist.Where((ep) => ep.Ordinal == Current).FirstOrDefault();
    }

    public bool CanGoNext()
    {
        return Playlist.Where((ep) => ep.Ordinal == Current + 1).FirstOrDefault() != null;
    }

    public bool CanGoPrev()
    {
        return Playlist.Where((ep) => ep.Ordinal == Current - 1).FirstOrDefault() != null;
    }

    public void GoNext()
    {
        if (CanGoNext())
        {
            Current++;
        }
    }

    public void GoPrev()
    {
        if (CanGoPrev())
        {
            Current--;
        }
    }
}
