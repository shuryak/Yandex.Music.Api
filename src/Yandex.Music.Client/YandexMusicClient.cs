using System.Collections.Generic;
using System.Linq;

using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Account;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Feed;
using Yandex.Music.Api.Models.Landing;
using Yandex.Music.Api.Models.Library;
using Yandex.Music.Api.Models.Playlist;
using Yandex.Music.Api.Models.Radio;
using Yandex.Music.Api.Models.Search;
using Yandex.Music.Api.Models.Track;

namespace Yandex.Music.Client
{
    public class YandexMusicClient
    {
        #region Поля

        private YandexMusicApi api;
        private AuthStorage storage;

        #endregion Поля

        #region Свойства

        /// <summary>
        /// Аккаунт
        /// </summary>
        public YAccount Account => storage.User;

        /// <summary>
        /// Флаг авторизации
        /// </summary>
        public bool IsAuthorized => storage.IsAuthorized;

        #endregion Свойства

        #region Основные функции

        public YandexMusicClient(DebugSettings settings = null)
        {
            api = new YandexMusicApi();
            storage = new AuthStorage(settings);
        }

        #region Авторизация

        public bool Authorize(string login, string password)
        {
            api.User.Authorize(storage, login, password);

            return storage.IsAuthorized;
        }

        public bool Authorize(string token)
        {
            api.User.Authorize(storage, token);

            return storage.IsAuthorized;
        }

        #endregion Авторизация

        #region Треки

        public YTrack GetTrack(string id)
        {
            return api.Track.Get(storage, id).Result.FirstOrDefault();
        }

        public List<YTrack> GetTracks(params string[] ids)
        {
            return api.Track.Get(storage, ids).Result;
        }

        #endregion Треки

        #region Альбомы

        public YAlbum GetAlbum(string id)
        {
            return api.Album.Get(storage, id).Result;
        }

        #endregion Альбомы

        #region Главная страница

        public YLanding GetLanding(params YLandingBlockType[] blocks)
        {
            return api.Landing.Get(storage, blocks).Result;
        }

        public YFeed Feed()
        {
            return api.Landing.GetFeed(storage).Result;
        }

        #endregion Главная страница

        #region Исполнители

        public YArtistBriefInfo GetArtist(string id)
        {
            return api.Artist.Get(storage, id).Result;
        }

        #endregion Исполнители

        #region Плейлисты

        public YPlaylist GetPlaylist(string user, string id)
        {
            return api.Playlist.Get(storage, user,id).Result;
        }

        public List<YPlaylist> GetPersonalPlaylists()
        {
            return api.Playlist.GetPersonalPlaylists(storage)
                .Select(r => r.Result)
                .ToList();
        }

        public List<YPlaylist> GetFavorites()
        {
            return api.Playlist.Favorites(storage).Result;
        }

        public YPlaylist GetAlice()
        {
            return api.Playlist.Alice(storage).Result;
        }

        public YPlaylist GetDejaVu()
        {
            return api.Playlist.DejaVu(storage).Result;
        }

        public YPlaylist GetMissed()
        {
            return api.Playlist.Missed(storage).Result;
        }

        public YPlaylist GetOfTheDay()
        {
            return api.Playlist.OfTheDay(storage).Result;
        }

        public YPlaylist GetPodcasts()
        {
            return api.Playlist.Podcasts(storage).Result;
        }

        public YPlaylist GetRewind()
        {
            return api.Playlist.Kinopoisk(storage).Result;
        }

        public YPlaylist GetPremiere()
        {
            return api.Playlist.Premiere(storage).Result;
        }

        public YPlaylist CreatePlaylist(string name)
        {
            return api.Playlist.Create(storage, name).Result;
        }

        #endregion Плейлисты

        #region Поиск

        public YSearch Search(string searchText, YSearchType searchType, int page = 0)
        {
            return api.Search.Search(storage, searchText, searchType, page).Result;
        }

        public YSearchSuggest GetSearchSuggestions(string searchText)
        {
            return api.Search.Suggest(storage, searchText).Result;
        }

        #endregion Поиск

        #region Библиотека

        public List<YTrack> GetLikedTracks()
        {
            string[] ids = api.Library.GetLikedTracks(storage)
                .Result
                .Library
                .Tracks
                .Select(t => t.Id)
                .ToArray();

            return api.Track.Get(storage, ids).Result;
        }

        public List<YTrack> GetDislikedTracks()
        {
            string[] ids = api.Library.GetDislikedTracks(storage)
                .Result
                .Library
                .Tracks
                .Select(t => t.Id)
                .ToArray();

            return api.Track.Get(storage, ids).Result;
        }

        public List<YAlbum> GetLikedAlbums()
        {
            string[] ids = api.Library.GetLikedAlbums(storage)
                .Result
                .Select(a => a.Id)
                .ToArray();

            return api.Album.GetList(storage, ids).Result;
        }

        #endregion Библиотека

        #region Радио

        public List<YStation> GetRadioDashboard()
        {
            return api.Radio.GetStationsDashboard(storage).Result.Stations;
        }

        public List<YStation> GetRadioStations()
        {
            return api.Radio.GetStations(storage).Result;
        }

        public YStation GetRadioStation(YStationId id)
        {
            return api.Radio.GetStation(storage, id).Result.FirstOrDefault();
        }

        #endregion Радио

        #endregion Основные функции
    }
}
