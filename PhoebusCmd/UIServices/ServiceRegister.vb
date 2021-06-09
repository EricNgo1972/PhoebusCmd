Imports pbs.Helper
Class ServiceRegister

    Shared Sub RegisterServices()

        pbs.UsrMan.ModuleSettings._register = True
        pbs.BO.Azure.Settings.RegisterModule()

        pbs.BO.SQLBuilder.ModuleSettings._register = True
        pbs.BO.Authentication.CloudLogin._azureToken = Nothing ' add assembly to the list
        pbs.BO.Settings.RegisterModule()

        pbs.BO.Apps.Setting.RegisterModule()

        pbs.BO.TBOSettings._register = True ' add assembly to the list
        pbs.BO.PO.Settings.RegisterModule()
        pbs.BO.SO.Settings.RegisterModule()
        pbs.BO.ClientDocuments.Settings.RegisterModule()
        pbs.BO.eInvoice.Settings.RegisterModule()
        pbs.BO.CRM.Settings.RegisterModule()
        pbs.BO.EXT.VUS2018.Settings.RegisterModule()
        pbs.BO.EXT.VUS2019.Settings.RegisterModule()
        pbs.BO.SM.Settings.RegisterModule()
        pbs.BO.FI.Settings.RegisterModule()
        pbs.BO.PM.Settings.RegisterModule()
        pbs.BO.LA.Settings.RegisterModule()
        pbs.BO.PI.Settings.RegisterModule()
        pbs.BO.RE.SettingsRE.RegisterModule()
        pbs.BO.MC.Settings.RegisterModule()
        pbs.BO.HR.Settings.RegisterModule()
        pbs.BO.LM.Settings.RegisterModule()
        '  pbs.BO.MXM.Settings.RegisterModule()
        ' pbs.BO.Sun5.Settings.RegisterModule()
        pbs.BO.EAM.Settings.RegisterModule()
        pbs.BO.HSP.Settings.RegisterModule()
        pbs.BO.FAST.Settings.RegisterModule()
        pbs.BO.MISA.Settings.RegisterModule()
        pbs.BO.MIMOSA.Settings.RegisterModule()

        pbs.Helper.UIServices.WaitingPanelService.RegisterUIService(New UI.WaitingMessageService)

        RegisterAddins()

        RegisterAzureServices()
        RegisterDataSelectorServices()
        RegisterUIServices()

        pbs.BO.Expressions.RegisterUDF.RegisterUDFExpression()

    End Sub

    Private Shared Sub RegisterAddins()
        Try
            Dim loadedAssy = pbs.Helper.PhoebusAssemblies.GetBOAssemblyNames

            Dim addinFiles = My.Computer.FileSystem.GetFiles(pbs.Helper.FileRepository.GetAddInsFolder, FileIO.SearchOption.SearchTopLevelOnly)

            Dim _notLoadeds As New List(Of String)
            Dim loaded As Integer = 0

            For Each addinfile In addinFiles
                Try
                    If loadedAssy.Contains(addinfile.FileName, StringComparer.OrdinalIgnoreCase) Then
                        Dim msg = String.Format("Assembly '{0}' is a built-in. Add-ins can not have this name", addinfile.FileName)
                        _notLoadeds.Add(msg)
                        Continue For
                    End If

                    Dim info = My.Computer.FileSystem.GetFileInfo(addinfile)
                    If System.IO.Path.GetExtension(addinfile).Equals(".dll", StringComparison.OrdinalIgnoreCase) Then

                        Dim ai_assy = System.Reflection.Assembly.LoadFile(addinfile)
                        For Each cl In ai_assy.DefinedTypes
                            If cl.Name = "Settings" Then
                                pbs.Helper.CallSharedMethodIfImplemented(cl, "RegisterModule")
                                Exit For
                            End If
                        Next

                        loaded += 1

                    End If

                Catch ex As Exception
                    Dim msg As String = String.Empty
                    Dim rtle = TryCast(ex, Reflection.ReflectionTypeLoadException)
                    If rtle IsNot Nothing Then

                        For Each txt In rtle.LoaderExceptions
                            msg += txt.Message
                        Next

                    Else
                        msg = ex.Message
                    End If
                    _notLoadeds.Add(String.Format(ResStr("Can not load addin {0}. {1}"), addinfile, msg))
                End Try
            Next

            AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf CheckLoaded

            If _notLoadeds.Count > 0 Then
                Console.WriteLine(String.Join(Environment.NewLine, _notLoadeds.ToArray))
            End If

            PhoebusAssemblies.InvalidateCahed()

            PhoebusAssemblies.GetBOAssemblies()

        Catch ex As Exception

        End Try
    End Sub


    ''' <summary>
    ''' This Assembly Resolver is used for fixing the Deserialize a object in the Addins. Why it can't be loaded by BinaryFormater
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Private Shared Function CheckLoaded(sender As Object, args As ResolveEventArgs) As System.Reflection.Assembly
        For Each asm In AppDomain.CurrentDomain.GetAssemblies
            If asm.FullName.Equals(args.Name) Then
                Return asm
            End If
        Next
        Return Nothing
    End Function


    Friend Shared Sub RegisterAzureServices()

        'pbs.Helper.DataServices.GetRegisteredUserService.RegisterService(New pbs.BO.Azure.Users.GetRegisteredUserService)

        pbs.Helper.AzureServices.AzureBlobUploadService.RegisterUIService(New pbs.BO.Azure.AzureStorage)
        pbs.Helper.AzureServices.BlobService.RegisterService(New pbs.BO.Azure.CloudBlobService)


        pbs.Helper.AzureServices.TemplateService.RegisterUIService(New pbs.BO.Azure.CloudTemplateService)
        pbs.Helper.AzureServices.FormStorageService.RegisterUIService(New pbs.BO.Azure.CloudFormStorageService)
        pbs.Helper.AzureServices.LayoutService.RegisterUIService(New pbs.BO.Azure.CloudLayoutService)

        pbs.Helper.AzureServices.TableSeachService.RegisterService(New pbs.BO.Azure.TableSearch)

        pbs.Helper.eInvoice.InvoiceNoService.RegisterService(New pbs.BO.Azure.eInvoice.InvoiceNoService)


        ' pbs.Helper.UIServices.CloudAuthenticationDialogService.RegisterUIService(New pbs.UI.Authentication.CloudAuthenticationDialogService)

        pbs.Helper.ConfigService.DBConnectionService.RegisterService(New pbs.BO.DB.DBConnectionStringService)

        pbs.Helper.DataServices.RunNonUIURLService.RegisterService(New pbs.BO.NonUIActionRunner)

        ChangeNotificationService()


    End Sub

    ''' <summary>
    ''' Call this right after switching database
    ''' </summary>
    Private Shared Sub ChangeNotificationService()
        Try
            Dim theService = pbs.BO.Mail.MailSetup.GetMailSetup.NotificationService

            Dim msinfo As pbs.BO.Mail.MailServiceInfo = Nothing
            If Not String.IsNullOrEmpty(theService) AndAlso pbs.BO.Mail.MailServiceInfoList.ContainsCode(theService, msinfo) Then
                pbs.Helper.MessageServices.SendMailService.RegisterService(msinfo)
            Else
                pbs.Helper.MessageServices.SendMailService.RegisterService(New pbs.BO.Mail.SendGridMailService)
            End If

        Catch ex As Exception
            pbs.Helper.TextLogger.Log(ex)
        End Try
    End Sub

    Friend Shared Sub RegisterDataSelectorServices()

        'pbs.Helper.UIServices.OpenFileService.RegisterUIService(New pbs.UI.OpenUrlService)
        pbs.Helper.UIServices.ValueSelectorService.RegisterUIService(New pbs.UI.ValueSelectorUIService)
        pbs.Helper.UIServices.MultiValueSelectorService.RegisterUIService(New pbs.UI.MultiValueSelectorUIService)
        pbs.Helper.UIServices.RangeValueSelectorService.RegisterUIService(New pbs.UI.RangeValueSelectorUIService)

        pbs.Helper.UIServices.ConfirmService.RegisterUIService(New UI.ConfirmService)

    End Sub

    Shared Sub RegisterUIServices()

        pbs.Helper.UIServices.AlertService.RegisterUIService(New UI.AlertService)

        pbs.Helper.UIServices.FileUploadService.RegisterUIService(New UI.ConsoleFileUploader)
        pbs.Helper.UIServices.UserInputService.RegisterUIService(New UI.ConsoleUserInput)
        pbs.Helper.UIServices.TableViewService.RegisterUIService(New pbs.UI.TableViewService)

        'pbs.Helper.UIServices.PrintPreviewService.RegisterUIService(New pbs.UI.PrintPreviewService45)

        'pbs.Helper.UIServices.SurveyService.RegisterUIService(New Forms.SurveyService)

        'pbs.Helper.UIServices.BuildDictionaryService.RegisterUIService(New pbs.UI.Forms.DictionaryEditor)

        'pbs.Helper.AzureServices.HelpViewerService.RegisterUIService(New pbs.UI.HelpViewerService)
        'pbs.Helper.AzureServices.HelpWriterService.RegisterUIService(New pbs.UI.HelpWriterService)

        pbs.Helper.DataServices.VariablesService.RegisterService(New pbs.BO.VariablesService)
        pbs.Helper.DataServices.FormulaService.RegisterService(New pbs.BO.PhoebusFormulaService)
        pbs.Helper.DataServices.LogService.RegisterService(New pbs.BO.PS.LogService)
        'pbs.Helper.DataServices.GetRegisteredUserService.RegisterService(New pbs.BO.Azure.Users.GetRegisteredUserService)

        pbs.Helper.ConfigService.DBConnectionService.RegisterService(New pbs.BO.DB.DBConnectionStringService)

        pbs.Helper.UIServices.RunURLService.RegisterUIService(New UI.RunURLService)

        'pbs.Helper.UIServices.ExpressionEditorService.RegisterUIService(New pbs.UI.Expressions.ExpressionEditorService)
        'pbs.Helper.UIServices.SplitValueService.RegisterUIService(New pbs.UI.SplitValueService)
        'pbs.Helper.UIServices.QDFiltersService.RegisterUIService(New pbs.UI.QDFilterSelectorService)
        'pbs.Helper.UIServices.ObjectEditService.RegisterUIService(New pbs.UI.Forms.ObjectDialogService)
        pbs.Helper.UIServices.ApprovalService.RegisterUIService(New pbs.UI.ApprovalDialogService)

        Csla.Workflow.WorkflowStatusService.RegisterService(New pbs.BO.WF.WorkflowStatusService)
        Csla.Rules.TriggerService.RegisterService(New pbs.BO.Rules.TriggerRuleService)
        Csla.Rules.IndexingService.RegisterService(New pbs.BO.Rules.IndexingService)

    End Sub

End Class
