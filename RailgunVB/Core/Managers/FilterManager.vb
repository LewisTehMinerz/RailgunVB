Imports Discord
Imports Microsoft.Extensions.DependencyInjection
Imports RailgunVB.Core.Filters
Imports TreeDiagram

Namespace Core.Managers
    
    Public Class FilterManager
    
        Private ReadOnly _filters As New List(Of IMessageFilter)()
        Private ReadOnly _services As IServiceProvider
        
        Public Sub New(services As IServiceProvider)
            _services = services
        End Sub
        
        Public Sub RegisterFilter(filter As IMessageFilter)
            _filters.Add(filter)
        End Sub
        
        Public Async Function ApplyFilterAsync(msg As IUserMessage) As Task(Of IUserMessage)
            Dim result As IUserMessage = Nothing
            
            Using db As TreeDiagramContext = _services.GetService(Of TreeDiagramContext)
                For Each filter As IMessageFilter In _filters
                    result = Await filter.FilterAsync(msg, db)
                
                    If result IsNot Nothing Then Exit For
                Next
            End Using
            
            Return result
        End Function
        
    End Class
    
End NameSpace