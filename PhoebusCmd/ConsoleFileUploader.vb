Imports pbs.Helper
Imports pbs.Helper.UIServices

Namespace UI
    Friend Class ConsoleFileUploader
        Implements UIServices.IFileUploadService

        Public Function OpenFile(GroupName As String, Extension As String, Optional pTitle As String = "Select the file you want to import from", Optional pPath As String = "") As String Implements IFileUploadService.OpenFile

            Console.Write("{0} (extension={1})", pTitle, Extension)
            Dim ret = Console.ReadLine()

            If System.IO.File.Exists(ret) Then
                Return ret
            End If

            Return String.Empty

        End Function

        Public Function OpenFiles(GroupName As String, Extension As String, Optional pTitle As String = "Select the file you want to import from", Optional pPath As String = "") As List(Of String) Implements IFileUploadService.OpenFiles

            Dim rets = New List(Of String)

            Dim conti As Boolean = True

            Do
                Console.Write("{0} (extension={1})", pTitle, Extension)

                Dim ret = Console.ReadLine()

                If System.IO.File.Exists(ret) Then
                    rets.Add(ret)
                End If

                Console.Write("Enter other file? Y/N")
                conti = Console.ReadLine.ToBoolean

            Loop Until conti = False

            Return rets

        End Function
    End Class
End Namespace

