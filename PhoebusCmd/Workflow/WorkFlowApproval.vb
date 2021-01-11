Imports pbs.BO.WF
Imports pbs.BO

Module WorkFlowApproval

    <Obsolete("Not use anymore")>
    Sub RunAutoApproval()
        Try
            Console_WriteLine("Workflow auto approval.")

            Dim selected = WFTInfoList.GetWFTInfoList
            If selected.Count > 0 Then
                Console_WriteLine("Processing auto approval for {0} pending tasks", selected.Count)

                For Each info In selected

                    Console_WriteLine("Process task {0}", info.TaskId)

                    Dim msg = info.AutoApproval

                    Console_WriteLine(msg)
                Next

            Else
                Console_WriteLine("No pending task")
            End If

            Console_WriteLine("Done.")

        Catch ex As Exception

        End Try
    End Sub



End Module
