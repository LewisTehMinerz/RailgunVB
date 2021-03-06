using System.Collections.Generic;
using MongoDB.Bson;

namespace TreeDiagram.Models.Server
{
    public class ServerMusic : ConfigBase
    {
        public ulong AutoTextChannel { get; set; } = 0;
        public ulong AutoVoiceChannel { get; set; } = 0;
        public bool AutoSkip { get; set; } = false;
        public bool AutoDownload { get; set; } = false;
        public ObjectId PlaylistId { get; set; } = ObjectId.Empty;
        public bool VoteSkipEnabled { get; set; } = false;
        public int VoteSkipLimit { get; set; } = 50;
        public bool SilentNowPlaying { get; set; } = false;
        public bool SilentSongProcessing { get; set; } = false;
        public ulong NowPlayingChannel { get; set; } = 0;
        public virtual List<AllowedRole> AllowedRoles { get; private set; } = new List<AllowedRole>();

        public ServerMusic(ulong id) : base(id) { }
    }
}