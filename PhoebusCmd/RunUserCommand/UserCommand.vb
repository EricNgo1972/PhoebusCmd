Module UserCommand
    Sub RunUserDefinedCommand(dic As Dictionary(Of String, String))
        Dim cmdCode = dic("CMD")
        RunCommand(cmdCode)
    End Sub

    Private Sub RunCommand(ByVal commandId As String)
        'Dim info As pbs.BO.CMDInfo = Nothing
        'If pbs.BO.CMDInfoList.ContainsCode(commandId, info) Then
        '    Console_WriteLine("Running Phoebus command {0}:", commandId)

        '    Dim ret = info.Run
        '    For Each itm In ret
        '        Console_WriteLine(itm)
        '    Next

        '    Console_WriteLine("Done!!!")
        'Else
        '    Console_WriteLine("command {0} not found. Exit", commandId)
        'End If
    End Sub
End Module
