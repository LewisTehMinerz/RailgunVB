Imports System.IO
Imports System.Text
Imports AudioChord
Imports Discord
Imports Discord.Commands
Imports RailgunVB.Core
Imports RailgunVB.Core.Managers
Imports RailgunVB.Core.Music
Imports RailgunVB.Core.Preconditions

Namespace Commands.Music

    Partial Public Class Music
    
        <Group("root"), BotAdmin>
        Public Class MusicRoot
            Inherits SystemBase
            
            Private ReadOnly _playerManager As PlayerManager

            Public Sub New(playerManager As PlayerManager)
                _playerManager = playerManager
            End Sub
            
            <Command("active"), BotPerms(ChannelPermission.AttachFiles)>
            Public Async Function ActiveAsync() As Task
                If _playerManager.PlayerContainers.Count < 1
                    await ReplyAsync("There are no active music streams at this time.")
                    Return
                End If
                
                Dim output As New StringBuilder
                
                output.AppendFormat("Active Music Streams ({0} Total):", 
                                    _playerManager.PlayerContainers.Count).AppendLine().AppendLine()
                
                For Each info In _playerManager.PlayerContainers
                    Dim player As Player = info.Player
                    Dim song As ISong = player.GetFirstSongRequest()
                    
                    output.AppendFormat("Id : {0} || Spawned At : {1} || Status : {2}", info.GuildId, player.CreatedAt, 
                                        player.Status).AppendLine() _ 
                        .AppendFormat("\\--> Latency : {0}ms || Playing : {1} || Since : {2}", 
                                      player.Latency, 
                                      If(song Is Nothing, "Searching...", song.Id.ToString()), 
                                      player.SongStartedAt).AppendLine().AppendLine()
                Next
                
                If output.Length < 1950
                    await ReplyAsync(Format.Code(output.ToString()))
                    Return
                End If
                
                Const filename As String = "ActivePlayers.txt"
                
                await File.WriteAllTextAsync(filename, output.ToString())
                await Context.Channel.SendFileAsync(filename)
                File.Delete(filename)
            End Function
            
            <Command("kill")>
            Public Async Function KillAsync(id As ULong) As Task
                _playerManager.DisconnectPlayer(id)
                await ReplyAsync($"Sent 'Kill Code' to Player ID {id}.")
            End Function
            
        End Class
    
    End Class
    
End NameSpace