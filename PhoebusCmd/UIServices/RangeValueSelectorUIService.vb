Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class RangeValueSelectorUIService
        Implements IRangeValueSelectorService

        Public Function SelectValue(SourceCode As String, Optional prompt As String = "", Optional pFormValue As String = "", Optional pToValue As String = "", Optional filters As Dictionary(Of String, String) = Nothing) As String Implements IRangeValueSelectorService.SelectValue
            Console.Write(String.Format("{0}. From value?", prompt, pFormValue))
            Dim fromValue = Console.ReadLine()
            Console.Write(String.Format("{0}. To value?", prompt, pToValue))
            Dim toValue = Console.ReadLine()

            If String.IsNullOrEmpty(toValue) OrElse fromValue.Equals(toValue) Then
                Return fromValue
            Else
                Return String.Format("<<{0}..{1}", fromValue, toValue)
            End If

        End Function
    End Class

End Namespace
