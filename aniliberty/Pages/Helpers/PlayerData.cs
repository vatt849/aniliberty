using aniliberty.Api.Data.Releases;
using aniliberty.Api.Data.Releases.Episodes;
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

    public Episode? GetNextEpisode()
    {
        return Playlist.SkipWhile((ep) => ep.Ordinal == Current).Skip(1).FirstOrDefault();
    }

    public Episode? GetPrevEpisode()
    {
        var currentItem = GetCurrentEpisode();
        if (currentItem == null) return null;

        int currentIndex = Playlist.IndexOf(currentItem);
        if (currentIndex <= 0) return null;

        return Playlist[currentIndex - 1];
    }

    public bool CanGoNext()
    {
        return GetNextEpisode() != null;
    }

    public bool CanGoPrev()
    {
        return GetPrevEpisode() != null;
    }

    public void GoNext()
    {
        var epNext = GetNextEpisode();
        if (epNext is not null)
        {
            Current = epNext.Ordinal;
        }
    }

    public void GoPrev()
    {
        var epPrev = GetPrevEpisode();
        if (epPrev is not null)
        {
            Current = epPrev.Ordinal;
        }
    }

    public void Go(decimal epNum)
    {
        if (Playlist.Where((ep) => ep.Ordinal == epNum).FirstOrDefault() != null)
        {
            Current = epNum;
        }
    }
}
