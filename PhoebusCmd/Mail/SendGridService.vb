Imports System.IO
Imports System.Net
Imports pbs.BO.Mail
Imports pbs.Helper
Imports pbs.Helper.Extensions
Imports pbs.Helper.Mail
Imports SendGrid
Imports SendGrid.Helpers.Mail

Namespace Mail
    ''' <summary>
    ''' Using SendGrid API
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SendGridMailService
        Public Shared spc_supportEmail As String = "support@spc-technology.com"
        Private Shared sg_username As String = "tungphoebus"
        Private Shared sg_psw As String = "phoebus2015"

        Public Shared Sub SendMSGO(_msgo As MSGO)
            If Not _msgo.MsgStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase) Then ExceptionThower.BusinessRuleStop("Only approved message can be sent")

            Try
                Dim msg = CreateSendGridMessageFromMSGO(_msgo)
                If msg IsNot Nothing Then

                    Dim client = New SendGridClient(apiKey)

                    client.SendEmailAsync(msg).FireAndForget

                    _msgo._msgStatus = "Sent"
                    _msgo._sentDate = Now()
                    _msgo.MarkAsDirty()
                    _msgo.Save()
                End If

            Catch ex As Exception
                TextLogger.Log(ex)
            End Try

        End Sub

        Public Shared Function CreateSendGridMessageFromMSGO(_msgo As MSGO) As SendGridMessage

            Dim msg = New SendGridMessage

            Dim sender = MCDInfoList.GetMCDInfo(_msgo.MsgType).Sender
            sender = If(String.IsNullOrEmpty(sender), pbs.UsrMan.ODInfoList.GetODInfo(Context.CurrentUserCode).Email, sender)
            sender = Nz(sender, "no-reply@phoebus.app")
            'From ------------------
            If EmailValidator.Valid(sender) Then
                msg.From = New EmailAddress(sender)
            Else
                msg.From = New EmailAddress("no-reply@phoebus.app")
            End If

            TextLogger.Log("Send from : {0}", msg.From.ToString)
            'To --------------------
            For Each _r In _msgo.Receipient.ToMailAddresses
                msg.AddTo(_r.Address)
            Next
            ' msg.AddTo("tung@spc-technology.com")

            msg.Subject = _msgo.Title
            msg.HtmlContent = _msgo.Body '.Replace("<img src='Images/", "<img src='cid:")

            'embeded images ------------
            Try
                Dim resources = _msgo.LinkedResources
                If Not String.IsNullOrEmpty(resources) Then
                    Dim ele = XElement.Parse(resources)
                    For Each node In ele.Descendants("itm")
                        If Not String.IsNullOrEmpty(node.Value) Then
                            Dim contentId = node.@key

                            Dim data = node.Value.Decompress2Bytes

                            msg.AddAttachment(contentId, Convert.ToBase64String(data))

                            'Dim temporaryfile = My.Computer.FileSystem.GetTempFileName & ".png"
                            'My.Computer.FileSystem.WriteAllBytes(temporaryfile, data, False)

                            'Dim ctt = New ContentType("image/png")
                            'Dim att = New Attachment(temporaryfile, ctt)
                            'Dim lnkRes = New LinkedResource(temporaryfile, ctt)

                            'msg.AddAttachment(att.ContentStream, att.Name)
                            'msg.EmbedImage(att.Name, lnkRes.ContentId)

                            'msg.Html = msg.Html.Replace("src='Images/" & contentId, "src='cid:" & lnkRes.ContentId)

                        End If
                    Next
                End If

            Catch ex As Exception
                TextLogger.Log("parse embeded images failed")
                TextLogger.Log(ex)
            End Try

            'attachment ------------
            Try
                For Each rawStr In _msgo.Attachments

                    If Not String.IsNullOrEmpty(rawStr) Then
                        Dim ele = XElement.Parse(rawStr)
                        Dim attchName = ele.@name

                        If String.IsNullOrEmpty(ele.Value) Then Exit Try

                        Dim Data = ele.Value.Decompress2Bytes

                        'save the data to a memory stream
                        Dim ms As New MemoryStream(Data)

                        'create the attachment from a stream. Be sure to name the data with a file and 
                        'media type that is respective of the data
                        ' msg.AddAttachment(ms, attchName)

                        msg.AddAttachmentAsync(attchName, ms).FireAndForget

                    End If
                Next

            Catch ex As Exception
                TextLogger.Log("parse attachment failed")
                TextLogger.Log(ex)
            End Try

            Return msg
        End Function

        Public Const apiKey = "----"

        Public Shared Sub Send(msg As SendGridMessage)
            Try

                Dim client = New SendGridClient(apiKey)

                client.SendEmailAsync(msg).FireAndForget

                'Dim credentials = New NetworkCredential(sg_username, sg_psw)
                'Dim transportWeb = New Web(credentials)

                'transportWeb.Deliver(msg)

            Catch ex As Exception
                TextLogger.Log(ex)
            End Try
        End Sub


    End Class
End Namespace

