Imports pbs.BO.SMS

Module SMS

    Sub RunSMS(dic As Dictionary(Of String, String))
        Dim _smsProfile = dic("SMS")
        Dim _command = If(dic.ContainsKey("C"), dic("C"), "S")

        Select Case _command
            Case "G", "g"
                GenerateSMS(_smsProfile)

            Case "S", "s"
                SendSMS()

        End Select

    End Sub

    Private Sub GenerateSMS(ByVal _smsProfile As String)
        Dim info As SMDInfo = Nothing
        If pbs.BO.SMS.SMDInfoList.ContainsCode(_smsProfile, info) Then
            Console_WriteLine("Running SMS profile {0}:", _smsProfile)

            Dim ret = info.Run()

            Console_WriteLine(ret)

            Console_WriteLine("Done!!!")
        Else
            Console_WriteLine("SMS profile {0} not found. Exit", _smsProfile)
        End If
    End Sub

    Private Sub SendSMS()
        Dim pendings = SMSOInfoList.GetSMSOInfoList

        Dim sentCount As Integer
        Dim sentlog As String = String.Empty

        If pendings.Count > 0 Then

            sentCount = SMSSetup.SendSMSOInfos(pendings.ToList)

            Console_WriteLine("{0} SMS has been sent.{1}", sentCount, Environment.NewLine)
        Else
            Console_WriteLine("No pending SMS. Exit")
        End If

    End Sub

End Module
