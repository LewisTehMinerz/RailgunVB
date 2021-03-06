Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports TreeDiagram

Namespace Core.Utilities
    
    Public Class EvalUtils
    
        Public ReadOnly Client As DiscordShardedClient
        Public ReadOnly IClient As IDiscordClient
        Public ReadOnly Context As ICommandContext
        Public ReadOnly DbContext As TreeDiagramContext

        Public Sub New(client As DiscordShardedClient, context As ICommandContext, dbContext As TreeDiagramContext)
            Client = client
            IClient = Client
            Context = context
            DbContext = dbContext
        End Sub
        
    End Class
    
End NameSpace