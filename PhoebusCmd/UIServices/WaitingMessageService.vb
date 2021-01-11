Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class WaitingMessageService
        Implements IWaitingPanelService

        Public Sub Done() Implements IWaitingPanelService.Done
            'Console_WriteLine(" Done!!!")
        End Sub

        Public Sub WaitMore(pTitle As String, Msg As String) Implements IWaitingPanelService.WaitMore
            Console_WriteLine()
            Console_WriteLine(String.Format("... {0} : {1}", pTitle, Msg))
        End Sub
    End Class

End Namespace
