Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class MultiValueSelectorUIService
        Implements IMultiValueSelectorService

        Public Function SelectIDs(InfoList As IEnumerable, Optional prompt As String = "", Optional pStartValue As String = "", Optional filters As Dictionary(Of String, String) = Nothing) As IEnumerable Implements IMultiValueSelectorService.SelectIDs
            Console.Write(String.Format("{0} (default value={1})?", prompt, pStartValue))
            Dim ret = Console.ReadLine()
            Return ret
        End Function

        Public Function SelectValues(SourceCode As String, Optional prompt As String = "", Optional pStartingValue As String = "", Optional filters As Dictionary(Of String, String) = Nothing) As String Implements IMultiValueSelectorService.SelectValues
            Console.Write(String.Format("{0} (default value={1})?", prompt, pStartingValue))
            Dim ret = Console.ReadLine()
            Return ret
        End Function
    End Class

End Namespace
