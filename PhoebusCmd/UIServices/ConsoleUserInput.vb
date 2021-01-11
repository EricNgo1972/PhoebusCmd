Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Class ConsoleUserInput
        Implements IUserInputService

        Public Function GetStringMatchRegex(RegexRule As String, Optional Tips As String = "", Optional question As String = "", Optional defaultValue As String = "") As String Implements IUserInputService.GetStringMatchRegex

            Do
                Console.Write("{0} (default value={1})", question, defaultValue)
                Dim ret = Console.ReadLine()
                ' Dim ret = defaultValue
                If ret.MatchesRegExp(RegexRule) Then
                    Return ret
                Else
                    Console.WriteLine("Your input is not valid. The regex rule is {0}", RegexRule)
                End If
            Loop

        End Function
    End Class

End Namespace
