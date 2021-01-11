Imports pbs.BO.PI

Module LedgerInterface

    Sub RunLedgerInterface(dic As Dictionary(Of String, String))
        If dic("LI") = "IC" Then
            IC2LA(dic)
            'ElseIf dic("LI") = "SI" Then
            '    SI2LA(dic)
        End If
    End Sub

    Sub IC2LA(dic As Dictionary(Of String, String))

        Dim PostTo = If(dic.ContainsKey("C"), dic("C"), "R")

        Dim _List = MInfoList.GetInterfaceableMovements

        For Each line In _List
            Console_WriteLine("->Batch#{0} Ref{1}:", line.BatchNo, line.MvmntRef)
            Dim _ji As pbs.BO.LA.JI = pbs.BO.LA.LI.IC2LA(line.BatchNo)

            If _ji.Count = 0 Then
                Console_WriteLine("... batch#{0} generates empty journal. Process it manually", line.BatchNo)
            ElseIf _ji.IsValid Then
                Try
                    Console_WriteLine("... Posting batch#{0} to ledger", line.BatchNo)

                    Dim jn As Integer

                    If PostTo = "P" Then _ji.Post() Else _ji.PostRough()

                    pbs.BO.LA.LI.MarkICAsInterfaced(New String() {line.BatchNo})
                    Console_WriteLine("... Journal#{0} posted", jn)
                Catch ex As Exception
                    Console_WriteLine(ex.Message)
                End Try

            Else
                Console_WriteLine("... batch#{0} generates invalid journal. Process it manually", line.BatchNo)
            End If

        Next

    End Sub

    'Sub SI2LA(dic As Dictionary(Of String, String))

    '    Dim PostTo = If(dic.ContainsKey("C"), dic("C"), "R")

    '    Dim _List = pbs.BO.SO.SOHInfoList.GetInterfaceableSI

    '    For Each line In _List
    '        Console_WriteLine("->TransRef#{0}", line.TransRef)
    '        Dim _ji As pbs.BO.LA.JI = pbs.BO.LA.LI.SI2LA(line.TransRef)

    '        If _ji.Count = 0 Then
    '            Console_WriteLine("... Sales Invoice#{0} generates empty journal. Process it manually", line.OrderNo)
    '        ElseIf _ji.IsValid Then
    '            Try
    '                Console_WriteLine("... Posting Sales Invoice#{0} to ledger", line.OrderNo)

    '                Dim jn As Integer

    '                If PostTo = "P" Then _ji.Post() Else _ji.PostRough()

    '                pbs.BO.LA.LI.MarkSIAsInterfaced(New String() {line.TransRef})
    '                Console_WriteLine("... Journal#{0} posted", jn)
    '            Catch ex As Exception
    '                Console_WriteLine(ex.Message)
    '            End Try

    '        Else
    '            Console_WriteLine("... batch#{0} generates invalid journal. Process it manually", line.TransRef)
    '        End If

    '    Next

    'End Sub

End Module
