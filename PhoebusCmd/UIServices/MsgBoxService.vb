Imports pbs.Helper
Imports pbs.Helper.Extensions
Imports pbs.Helper.UIServices

Namespace UI
    Class ConfirmService
        Implements IConfirmService

        Public Function Confirm(pFormatString As String, ParamArray params() As Object) As Boolean Implements IConfirmService.Confirm
            Console_WriteLine(pFormatString, params)
            'Console_WriteLine("Confirmation for command line : Y")

            Return True
            'Dim ret = Console.ReadLine()
            'Return ret.ToBoolean
        End Function

        Public Sub ShowError(ex As Exception) Implements IConfirmService.ShowError
            Dim msg As String = String.Empty
            ex.Dump(msg)
            Console_WriteLine(msg)
        End Sub
    End Class

End Namespace
