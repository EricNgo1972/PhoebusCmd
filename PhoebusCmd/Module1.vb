Imports pbs.UsrMan
Imports pbs.BO.EAM
Imports System.Text.RegularExpressions
Imports pbs.Helper
Imports pbs.BO
Imports System.Threading

Module Module1

    Private Sub SayHi()
        Console_WriteLine("------------------------------------")
        Console_WriteLine("Welcome to Phoebus Command Line, {0}", Csla.ApplicationContext.User.Identity.Name)
        Console_WriteLine("ver {0}", My.Application.Info.Version.ToString)
        Console_WriteLine("Schedule this command line to run a NonUI phoebusURL")
        Console_WriteLine("Send email, Run ledger interface, Create Preventive Maintenace Work Orders, send SMS, SendDueReminders, auto approval workflow and any other non ui command)")
        Console_WriteLine("Copyright by SPC Technology 2007-2017")
        Console_WriteLine("http://www.spc-technology.com")
        Console_WriteLine("------------------------------------")
    End Sub

    Private Sub SayGoodbye(dic As Dictionary(Of String, String))

        If dic.GetValueByKey("h", String.Empty).ToBoolean OrElse dic.Count = 0 Then
            ShowTips()
        End If

        If String.IsNullOrEmpty(dic.GetValueByKey("c", String.Empty)) AndAlso _
           String.IsNullOrEmpty(dic.GetValueByKey("pm", String.Empty)) AndAlso _
           String.IsNullOrEmpty(dic.GetValueByKey("mail", String.Empty)) AndAlso _
           String.IsNullOrEmpty(dic.GetValueByKey("sms", String.Empty)) AndAlso _
           String.IsNullOrEmpty(dic.GetValueByKey("LI", String.Empty)) Then

            Console_WriteLine("Please set atleast one Non-UI Command for PhoebusCmd. Need more help? Go to https://phoebusfiles.blob.core.windows.net/help/phoebuscmd.html")
            Console_WriteLine("Goodbye {0}", Csla.ApplicationContext.User.Identity.Name)

            Console.ReadKey()
        Else
            Console_WriteLine("Goodbye {0}", Csla.ApplicationContext.User.Identity.Name)

        End If

    End Sub

    Public Sub ShowTips()
        Console_WriteLine("Syntax :")
        Console_WriteLine("Preventive Maintenace syntax:")
        Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity con=ConnectionCode pm=PM_Profile s=30")
        Console_WriteLine("--------------------------------------------------------------------------------")
        Console_WriteLine("Phoebus mailer syntax:")
        Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity con=ConnectionCode mail=mailprofile c=G|S|W")
        Console_WriteLine("G= generate mail to outbox and waiting  for approval. S = Send approved mails. W= Send approved mails via Web API")
        Console_WriteLine("--------------------------------------------------------------------------------")
        Console_WriteLine("Phoebus Sms syntax:(obsolet)")
        Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity con=ConnectionCode sms=smsprofile c=G|S")
        Console_WriteLine("G= generate sms to outbox and waiting for approval. S = Send approved sms")
        Console_WriteLine("--------------------------------------------------------------------------------")
        Console_WriteLine("Ledger Interface:")
        Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity con=ConnectionCode LI=IC|SI c=P|R")
        Console_WriteLine("Run ledger interface. P=Post, R= Rough")

        Console_WriteLine("--------------------------------------------------------------------------------")
        Console_WriteLine("Send Due Reminders:")
        Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity con=ConnectionCode c=SendDueReminder")
        Console_WriteLine("Run command pbs.BO.WF.SendDueReminder")

        Console_WriteLine("--------------------------------------------------------------------------------")
        Console_WriteLine("Run NonUI Phoebus Command:")
        Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity con=ConnectionCode c=''pbs.BO.WF.SendDueReminders?...key=value...''")
        Console_WriteLine("Run URL command pbs.BO.WF.SendDueReminders")

        'Console_WriteLine("--------------------------------------------------------------------------------")
        'Console_WriteLine("Workflow Background Approval:")
        'Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity WF")
        'Console_WriteLine("Run back ground approval process.")

        'Console_WriteLine("--------------------------------------------------------------------------------")
        'Console_WriteLine("Transfer Manager (TRN):")
        'Console_WriteLine("PhoebusCmd u=userid p=pass d=PhoebusEntity TRN=Profile")
        'Console_WriteLine("Run transfer manager profile")

    End Sub

    Private _mutex As Mutex = New Mutex(True, "{5EA164F9-E4B2-4FD6-B7A3-467D2E726791}")

    Private Function GetArguments() As Dictionary(Of String, String)
        Dim args = My.Application.CommandLineArgs

        'If args.Count < 3 Then
        '    ShowTips()
        'End If

        Dim dic As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        For Each itm In args
            Dim key = Regex.Match(itm, "^[^\=]+").Value.ToUpper
            Dim val = itm.RegExpReplace("^[^\=]+=", String.Empty) 'Regex.Match(itm, "[^\=]+$", RegexOptions.RightToLeft).Value
            If Not dic.ContainsKey(key) Then dic.Add(key, val)
        Next

        Return dic

    End Function

    Sub Main()

        If Not _mutex.WaitOne(TimeSpan.Zero, True) Then
            Console_WriteLine("Another PhoebusCmd instance is running. Try again later")
            Exit Sub
        End If

        pbs.BO.LA.NAInfoList.InvalidateCache()

        ServiceRegister.RegisterServices()

        SayHi()

        Dim dic = GetArguments()

        Try


            Dim _user = dic.GetValueByKey("U", String.Empty)
            Dim _pass = dic.GetValueByKey("P", String.Empty)
            Dim _DTB = dic.GetValueByKey("D", String.Empty)

            Dim _connection = dic.GetValueByKey("CON", String.Empty)
            If Not String.IsNullOrEmpty(_connection) Then
                pbs.Helper.Connections._forceConnectionCode = _connection
            End If

            If String.IsNullOrEmpty(_user) Then Console_WriteLine("UserId is not provided")
            If String.IsNullOrEmpty(_DTB) Then Console_WriteLine("Phoebus Entity is not provided")

            Console_WriteLine("Login with user {0}", _user)

            If Not pbsPrincipal.Login(_user, _pass) Then

                Console_WriteLine("Login failed for user {0}", _user)
                Exit Try

            End If

            Console_WriteLine("{0} login successful.", _user)

            SetLocalContext()

            Console_WriteLine("Go to entity {0}.", _DTB)

            Dim dt = pbs.BO.CD.ChangeDB(_DTB)
            If String.IsNullOrEmpty(dt) Then
                Console_WriteLine("Can not switch entity to '{0}'.", _DTB)
                Exit Try
            End If

            Context.DebugMode = dic.ContainsKey("DEBUG") OrElse dic.ContainsKey("DEBUGMODE")

            If dic.ContainsKey("MAIL") Then
                Console.WriteLine("Please consider the new syntax: ")
                Console.WriteLine("PhoebusCmd u=user p=password d=entity c=''PhoebusURLCommand?...parameters...''")

                RunMailer(dic)

                'ElseIf dic.ContainsKey("SMS") Then
                '    Console.WriteLine("Please consider the new syntax: ")
                '    Console.WriteLine("PhoebusCmd u=user p=password d=entity c=''PhoebusURLCommand?...parameters...''")

                '    RunSMS(dic)

            ElseIf dic.ContainsKey("PM") Then
                Console.WriteLine("Please consider the new syntax: ")
                Console.WriteLine("PhoebusCmd u=user p=password d=entity c=''PhoebusURLCommand?...parameters...''")

                RunPM(dic)

            ElseIf dic.ContainsKey("LI") Then
                Console.WriteLine("Please consider the new syntax: ")
                Console.WriteLine("PhoebusCmd u=user p=password d=entity c=''PhoebusURLCommand?...parameters...''")

                RunLedgerInterface(dic)

                'ElseIf dic.ContainsKey("WF") Then
                '    RunAutoApproval()

                'ElseIf dic.ContainsKey("TRN") Then
                '    RunUserDefinedCommand(dic)

            Else

                Dim thePhoebusURL = dic.GetValueByKey("c", String.Empty)
                Dim arg = New pbsCmdArgs(thePhoebusURL)

                Console.WriteLine("Lauching PhoebusURL: {0}", thePhoebusURL)

                pbs.Helper.UIServices.RunURLService.Run(arg)

                Console.WriteLine("Command End.")
            End If

        Catch ex As Exception
            Console_WriteLine(ex.Message)
        Finally
            pbs.Helper.TextLogger.Flush()
        End Try

        SayGoodbye(dic)

    End Sub

    Public Sub SetLocalContext()

        Try
            Csla.ApplicationContext.ClientContext("_ConsoleApplication_") = True

            'load user email and employee code
            Dim uid = pbs.Helper.Context.CurrentUserCode
            Dim info = ODInfoList.GetODInfo(uid)

            Csla.ApplicationContext.ClientContext(pbs.Helper.EmplCode) = info.EmplCode
            Csla.ApplicationContext.ClientContext(pbs.Helper.Email) = info.Email

            Dim serial = pbs.Helper.PhoebusSerialInfo.FetchSerialInfo

            ' Csla.ApplicationContext.ClientContext(pbs.Helper.SerialInfo) = serial

        Catch ex As Exception
            pbs.Helper.UIServices.ConfirmService.ShowError(ex)
            'Csla.ApplicationContext.ClientContext(pbs.Helper.SerialInfo) = Nothing
        End Try

    End Sub

    Public Sub Console_WriteLine(ByVal formattext As String, ByVal ParamArray Args() As Object)

        'pbs.Helper.TextLogger.Log(formattext, Args)

        Console.WriteLine(formattext, Args)

    End Sub

    Public Sub Console_WriteLine(ByVal msg As String)


        Console.WriteLine(msg)

    End Sub

    Public Sub Console_WriteLine()

        pbs.Helper.TextLogger.Log(Environment.NewLine)

        Console.WriteLine()

    End Sub
End Module
