Public Class View
    Inherits System.Web.UI.Page
    Dim Query As New QueryList
    Dim MyControl As ControlClass


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            
            Next

        End If

    End Sub	
	
    Private Sub btmView_Click(sender As Object, e As EventArgs) Handles btmView.Click
        Try
            Query.ExecSPViewWithError("Phone", 1, grdViewMain, PnlHeader.Controls)
            udpPanelMain.Update()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Attention", "javascript:alert('Error : " + Replace(Replace(Replace(ex.Message, "'", ""), Chr(13), ""), Chr(10), "") + "');", True)
        End Try
    End Sub

	
	Private Sub btmSubmit_Click(sender As Object, e As EventArgs) Handles btmSubmit.Click
        Try
            Query.ExecSPViewWithError("Phone", 2, grdViewMain, PnlHeader.Controls)
            udpPanelMain.Update()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Attention", "javascript:alert('Error : " + Replace(Replace(Replace(ex.Message, "'", ""), Chr(13), ""), Chr(10), "") + "');", True)
        End Try

    End Sub

	
	Private Sub btmExport_Click(sender As Object, e As EventArgs) Handles btmExport.Click
        Try
            Query.ExecSPViewWithError("Phone", 3, grdViewMain, PnlHeader.Controls)
            udpPanelMain.Update()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Attention", "javascript:alert('Error : " + Replace(Replace(Replace(ex.Message, "'", ""), Chr(13), ""), Chr(10), "") + "');", True)
        End Try

    End Sub


