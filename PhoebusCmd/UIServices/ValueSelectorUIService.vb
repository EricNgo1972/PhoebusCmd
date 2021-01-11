Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class ValueSelectorUIService
        Implements IValueSelectorService

        Public Function SelectID(InfoList As IEnumerable, Optional prompt As String = "", Optional pStartValue As String = "", Optional filters As Dictionary(Of String, String) = Nothing) As String Implements IValueSelectorService.SelectID
            Console.Write(String.Format("{0} (default value={1})?", prompt, pStartValue))
            Dim ret = Console.ReadLine()
            Return ret
        End Function

        Public Function SelectRow(pTable As DataTable, Optional prompt As String = "", Optional pStartRowId As String = "", Optional pViewOptions As Dictionary(Of String, String) = Nothing) As Dictionary(Of String, Object) Implements IValueSelectorService.SelectRow
            Return Nothing
        End Function

        Public Function SelectValue(SourceCode As String, Optional prompt As String = "", Optional pStartingValue As String = "", Optional filters As Dictionary(Of String, String) = Nothing) As String Implements IValueSelectorService.SelectValue
            Console.Write(String.Format("{0} (default value={1})?", prompt, pStartingValue))
            Dim ret = Console.ReadLine()
            Return ret
        End Function
    End Class

End Namespace
