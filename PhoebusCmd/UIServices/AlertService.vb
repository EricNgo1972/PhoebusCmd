Imports pbs.Helper
Imports pbs.Helper.Extensions
Imports pbs.Helper.UIServices

Namespace UI
    Class AlertService
        Implements IAlertService

        Public Sub Alert(pFormatString As String, ParamArray params() As Object) Implements IAlertService.Alert
            Console_WriteLine(pFormatString, params)
        End Sub

        Public Sub Alert(Args As pbsCmdArgs) Implements IAlertService.Alert
            Alert(Args._AlertText)
        End Sub

        Public Sub QuickAlert(pFormatString As String, ParamArray params() As Object) Implements IAlertService.QuickAlert
            Console_WriteLine(pFormatString, params)
        End Sub

        Public Sub ShowError1(ex As Exception) Implements IAlertService.ShowError
            Dim msg = String.Empty
            ex.Dump(msg)
            Console_WriteLine(msg)
        End Sub

        Public Function GetChildAlertable() As IAlertService Implements IAlertService.GetChildAlertable

            Return Nothing
        End Function
    End Class

End Namespace
