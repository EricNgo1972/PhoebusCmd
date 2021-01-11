Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class ApprovalDialogService
        Implements IApprovalService
        Public Function GetApproval(Title As String, Notes As String) As String Implements IApprovalService.GetApproval
            Console.Write("{0}. Need approval", Title)
            Return String.Format("Console Approval with user id {0}. {1}", Context.CurrentUserCode, Context.Email)
        End Function
    End Class

End Namespace
