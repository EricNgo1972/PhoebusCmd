Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class TableViewService
        Implements pbs.Helper.UIServices.ITableViewService

        Public Sub Show(Args As pbsCmdArgs) Implements ITableViewService.Show

            If Args IsNot Nothing AndAlso Args._bo IsNot Nothing Then
                Dim dt = TryCast(Args._bo, DataTable)
                If dt IsNot Nothing Then
                    Dim content = dt.Dump
                    Console.Write(content)
                    Console.WriteLine()
                End If
            End If

        End Sub

        'Public Function ShowDialog(Args As pbsCmdArgs) As Windows.Forms.DialogResult Implements ITableViewService.ShowDialog
        '    Show(Args)
        'End Function

        Private Function ITableViewService_ShowDialog(Args As pbsCmdArgs) As Integer Implements ITableViewService.ShowDialog
            Show(Args)
            Return 1
        End Function
    End Class

End Namespace
