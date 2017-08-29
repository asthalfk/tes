Imports System.Globalization

Public Class QueryList

    Dim MyDBCore As New DBCore

    Public Function GetErrorFromDbCore() As String
        Return MyDBCore.ErrorDescription()
    End Function

    Private Function GetControlValues(ByVal ctrl As WebControl) As String
        Dim vals As String = ""
        GetControlValues = ""
        If ctrl.GetType.ToString = "System.Web.UI.WebControls.TextBox" Then
            Return DirectCast(ctrl, TextBox).Text
            Exit Function
        End If
    End Function

    Public Function getScalar(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As ControlCollection = Nothing, Optional parameter As String = "") As String
        Dim str As String = QueryList(modulename, modID, parameter)
        Return MyDBCore.getScalarValue(str, Ctrl)
    End Function

    Public Function ExecSP(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As ControlCollection = Nothing, Optional par1 As String = "") As Boolean
        Dim Str As String = QueryList(modulename, modID)
        If par1 <> "" Then
            Str = Str & " '" & par1 & "'"
        End If
        Return MyDBCore.EXECSP(Str, Ctrl)
    End Function

    Public Function ExecSPWithError(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As ControlCollection = Nothing, Optional par1 As String = "") As String
        Dim Str As String = QueryList(modulename, modID)
        If par1 <> "" Then
            Str = Str & " '" & par1 & "'"
        End If
        Try
            Return MyDBCore.EXECSPWithErrorReturn(Str, Ctrl)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Sub SaveErrorLog(ByVal modulename As String, ByVal err As String)
        MyDBCore.InsertError(err, modulename)
    End Sub

    Public Function ExecSPView(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As WebControl = Nothing, Optional MyCtrl As ControlCollection = Nothing, Optional OneCtrl As WebControl = Nothing) As Boolean
        Dim Str As String = QueryList(modulename, modID)
        Return MyDBCore.ExecSPToGridView(Str, Ctrl, MyCtrl)
    End Function

    Public Function ExecSPViewEX(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As WebControl = Nothing, Optional MyCtrl As ControlCollection = Nothing, Optional MyParameter As String = "") As Boolean
        Dim Str As String = QueryList(modulename, modID)
        If MyParameter <> "" Then Str += "'" + MyParameter + "',"
        Return MyDBCore.ExecSPToGridViewEX(Str, Ctrl, MyCtrl)
    End Function

    Public Function ExecSPViewWithError(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As WebControl = Nothing, Optional MyCtrl As ControlCollection = Nothing, Optional OneCtrl As WebControl = Nothing) As Boolean
        Dim Str As String = QueryList(modulename, modID)
        Try
            Return MyDBCore.ExecSPToGridViewWithError(Str, Ctrl, MyCtrl)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function ExecGetQueryData(ByVal modulename As String, ByVal modID As Integer, Optional MyCtrl As ControlCollection = Nothing) As Data.DataTable
        Dim Str As String = QueryList(modulename, modID)
        Return MyDBCore.GetQueryData(Str, MyCtrl)
    End Function

    Public Function ExecGetQueryDataEX(ByVal modulename As String, ByVal modID As Integer, Optional MyCtrl As ControlCollection = Nothing, Optional MyParameter As String = "") As Data.DataTable
        Dim Str As String = QueryList(modulename, modID)

        If MyParameter <> "" Then
            'Str = Str & " '" & MyParameter & "',"
            Str = Str & " '" & MyParameter & "'"
        End If

        Return MyDBCore.GetNullableQueryData(Str, MyCtrl)
    End Function

    Public Function ExecGetQueryDataWithErrorDesc(ByVal modulename As String, ByVal modID As Integer, Optional MyCtrl As ControlCollection = Nothing) As Data.DataTable
        Dim Str As String = QueryList(modulename, modID)

        Return MyDBCore.GetQueryDataWithErrorDesc(Str, MyCtrl)
    End Function

    Public Function GetDataSet(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As WebControl = Nothing, Optional MyCtrl As ControlCollection = Nothing) As Data.DataSet
        Dim Str As String = QueryList(modulename, modID)
        Return MyDBCore.DBDataSet(Str, MyCtrl)
    End Function

    Public Function GetDataTable(ByVal modulename As String, ByVal modID As Integer, Optional Ctrl As WebControl = Nothing, Optional MyCtrl As ControlCollection = Nothing, Optional MyParameter As String = "") As Data.DataTable
        Dim Str As String = QueryList(modulename, modID)
        If MyParameter <> "" Then Str += " '" + MyParameter + "',"
        If (IsNothing(MyCtrl) And Right(Str, 1) = ",") Then Str = Left(Str, Len(Str) - 1)
        Return MyDBCore.GetQueryDataEX(Str, MyCtrl)
    End Function

    Private Function QueryList(ByVal modulename As String, ByVal id As Int16, Optional parID As String = "") As String
        Dim SQL As String = ""
        Select Case modulename
            Case "Phone"
                If id = 1 Then
                    SQL = "SP_View"
                ElseIf id = 2 Then
                    SQL = "[SP_Submit]"
                ElseIf id = 3 Then
                    SQL = "[SP_Export]"
                End If
            
        End Select

        Return SQL
    End Function
End Class
