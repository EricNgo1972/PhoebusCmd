Imports pbs.BO.Mail

Module Mailer

    Sub RunMailer(dic As Dictionary(Of String, String))

        Dim _mailProfile = dic("MAIL")
        Dim _command = If(dic.ContainsKey("C"), dic("C"), String.Empty)

        Select Case _command
            Case "G", "g"
                GenerateMail(_mailProfile)

            Case "S", "s"
                SendMail_3_5()

            Case "W", "w"
                SendMail()

            Case Else
                GenerateMail(_mailProfile)
                SendMail_3_5()
        End Select

    End Sub

    Private Sub GenerateMail(ByVal _mailProfile As String)
        Dim info As MCDInfo = Nothing
        If pbs.BO.Mail.MCDInfoList.ContainsCode(_mailProfile, info) Then
            Console_WriteLine("Running mailing profile {0}:", _mailProfile)

            Dim ret = info.Run()
            For Each itm In ret
                Console_WriteLine(itm)
            Next

            Console_WriteLine("Done!!!. Generated {0} mail(s)", ret.Count)
        Else
            Console_WriteLine("Mailing profile {0} not found. Exit", _mailProfile)
        End If
    End Sub

    Private Sub SendMail()
        Dim pendings = MSGOInfoList.GetMSGOInfoList

        Dim sentCount As Integer
        Dim sentlog As String = String.Empty

        If pendings.Count > 0 Then
            For Each m In pendings
                Try
                    Console_WriteLine("=>Sending : {0} to {1}", m.Description, m.Receipient)

                    Dim _msgo = MSGO.GetMSGO(m.MsgId)

                    Mail.SendGridMailService.SendMSGO(_msgo)

                    sentCount += 1
                Catch ex As Exception
                    Console_WriteLine(ex.Message)
                End Try

            Next
            Console_WriteLine("{0} mails has been sent.{1}", sentCount, Environment.NewLine)
        Else
            Console_WriteLine("No pending mails. Exit")
        End If

    End Sub

    Private Sub SendMail_3_5()
        Dim pendings = MSGOInfoList.GetMSGOInfoList

        Dim sentCount As Integer
        Dim sentlog As String = String.Empty

        If pendings.Count > 0 Then
            For Each m In pendings
                Try
                    Console_WriteLine("=>Sending : {0} to {1}", m.Description, m.Receipient)

                    m.Send()

                    Console_WriteLine("Sent!!!")
                    sentCount += 1
                Catch ex As Exception
                    Console_WriteLine(ex.Message)
                End Try

            Next
            Console_WriteLine("{0} mails has been sent.{1}", sentCount, Environment.NewLine)
        Else
            Console_WriteLine("No pending mails. Exit")
        End If

    End Sub

End Module
