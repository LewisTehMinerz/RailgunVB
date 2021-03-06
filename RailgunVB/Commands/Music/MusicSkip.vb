Imports Discord
Imports Discord.Commands
Imports RailgunVB.Core
Imports RailgunVB.Core.Managers
Imports RailgunVB.Core.Music
Imports RailgunVB.Core.Preconditions
Imports RailgunVB.Core.Utilities
Imports TreeDiagram
Imports TreeDiagram.Models.Server

Namespace Commands.Music
    
    Partial Public Class Music
        
        <Group("skip")>
        Public Class MusicSkip
            Inherits SystemBase
            
            Private ReadOnly _commandUtils As CommandUtils
            Private ReadOnly _playerManager AS PlayerManager

            Public Sub New(commandUtils As CommandUtils, playerManager As PlayerManager)
                _commandUtils = commandUtils
                _playerManager = playerManager
            End Sub
            
            <Command>
            Public Async Function SkipAsync As Task
                Dim data As ServerMusic = Await Context.Database.ServerMusics.GetAsync(Context.Guild.Id)
                Dim container As PlayerContainer = _playerManager.GetPlayer(Context.Guild.Id)
                
                If data Is Nothing OrElse container Is Nothing
                    await ReplyAsync("Can not skip current song because I am not in voice channel.")
                    Return
                ElseIf Not (data.VoteSkipEnabled)
                    await ReplyAsync("Skipping music now...")
                    container.Player.CancelMusic()
                    Return
                End If
                
                Dim player As Player = container.Player
                Dim userCount As Integer = await player.GetUserCountAsync()
                Dim voteSkipResult As Integer = player.VoteSkip(Context.User.Id)
                Dim percent As Integer = (player.VoteSkipped.Count / userCount) * 100
                
                If percent < data.VoteSkipLimit
                    Dim name As String = Await _commandUtils.GetUsernameOrMentionAsync(Context.User)
                    await ReplyAsync($"{Format.Bold(name)} has voted to skip the current song!")
                    Return
                ElseIf Not (voteSkipResult)
                    Await ReplyAsync("You've already voted to skip!")
                    Return
                End If
                
                await ReplyAsync("Vote-Skipping music now...")
                player.CancelMusic()
            End Function
            
            <Command("force"), UserPerms(GuildPermission.ManageGuild)>
            Public Async Function ForceAsync() As Task
                Dim data As ServerMusic = Await Context.Database.ServerMusics.GetAsync(Context.Guild.Id)
                Dim container As PlayerContainer = _playerManager.GetPlayer(Context.Guild.Id)
                
                If data Is Nothing OrElse Not (data.VoteSkipEnabled)
                    await ReplyAsync("This command is not available due to Music Vote-Skip being disabled.")
                    Return
                ElseIf container Is Nothing
                    await ReplyAsync("Can not skip current song because I am not in voice channel.")
                    Return
                End If
                
                await ReplyAsync("Force-Skipping music now...")
                container.Player.CancelMusic()
            End Function

        End Class
        
    End Class
    
End NameSpace