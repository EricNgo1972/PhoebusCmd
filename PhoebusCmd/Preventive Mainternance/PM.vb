Imports pbs.Helper
Imports pbs.UsrMan
Imports pbs.BO
Imports pbs.BO.EAM

Module PM
    <Obsolete("Not use anymore")>
    Friend Sub RunPM(dic As Dictionary(Of String, String))

        Dim ok As Boolean = True

        Dim _pmProfile = "All"
        Dim _pmProfiles As String() = New String() {}

        If Not dic.ContainsKey("PM") Then
            Console_WriteLine("Preventive Maintenance Profile is not provided. Process all profile")
            ok = False
        Else
            _pmProfile = dic("PM")
            _pmProfiles = _pmProfile.Split(New String() {",", "|"}, StringSplitOptions.RemoveEmptyEntries)
        End If

        Dim _slackTime = 30

        If Not dic.ContainsKey("S") Then
            Console_WriteLine("Slack Date = 30 by default")
        Else
            _slackTime = dic("S").ToInteger
        End If

        If Not ok Then
            'ShowTips()
            Exit Sub
        End If

        For Each info In PMInfoList.GetPMInfoList
            Dim wocount = 0
            Try
                'PMInfo.UpdatePMProfile(info.PmNum)

                If _pmProfile.MatchesRegExp("^All$") OrElse _pmProfile = info.PmNum OrElse _pmProfiles.Contains(info.PmNum) Then

                    PMInfo.UpdateCompletedDate(info.PmNum)
                    PMInfo.UpdateTimeBasedParameters(info.PmNum)
                    Console_WriteLine("Generate WO for PM profile {0} until {1:yyyy-MM-dd}.", info.PmNum, ToDay.AddDays(_slackTime))

                    Dim _woplans = info.Planning_Work_Orders(_slackTime)


                    For Each plan In _woplans

                        Dim _wos = info.Generate_WO_ByPlan(plan)

                        For Each _wo In _wos
                            If _wo.IsSavable Then
                                _wo.SetWONumber()
                                _wo.Save()
                                wocount += 1
                                Console_WriteLine("Work Order {0}-{1} created in {2}", _wo.WoNum, _wo.Descriptn, _wo.TargStartDate)
                            Else
                                Console_WriteLine("Work Order {0} is not valid : {1}", _wo.WoNum, _wo.BrokenRulesMsg)
                            End If
                        Next

                    Next

                    Console_WriteLine("{0} Work Order(s) has been created for profile {1}-{2}", wocount, info.Code, info.Description)

                    PMInfo.UpdateCompletedDate(info.PmNum)

                    PMInfo.UpdateTimeBasedParameters(info.PmNum)
                End If

            Catch ex As Exception
                TextLogger.Log(ex)
            End Try

        Next

    End Sub

End Module
