Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class RunURLService
        Implements IRunURLService

        Public Sub Run(Args As pbsCmdArgs) Implements IRunURLService.Run
            If Args.GetShortCutSegment(0).Equals("SendDueReminders", StringComparison.OrdinalIgnoreCase) Then
                pbs.BO.WF.RMDInfoList.SendDueReminders(Nothing)
            Else
                pbs.BO.NonUIActionRunner.RunURLCommand(Args)
            End If

        End Sub

    End Class

End Namespace
