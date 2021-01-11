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


        RegisterAzureServices()
        RegisterDataSelectorServices()
        RegisterUIServices()

        pbs.BO.Expressions.RegisterUDF.RegisterUDFExpression()

    End Sub

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
