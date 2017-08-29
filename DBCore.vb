Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography

Public Class DBCore
    Public sConn As String = System.Configuration.ConfigurationManager.ConnectionStrings("Db1ConnectionString").ConnectionString
    Dim ctr As Int16 = 0
    Dim Errstr As String = ""
    Dim ConnDB As New SqlConnection
    Dim TmpTable As New DataTable("Data")
    Dim MyEncrypt As New System.Security.Cryptography.RijndaelManaged

    Public varSession As String
    Public plainText As String
    Public passPhrase As String = "Pas5pr@se"        ' can be any string
    Public saltValue As String = "s@1tValue"        ' can be any string
    Public hashAlgorithm As String = "SHA1"             ' can be "MD5"
    Public passwordIterations As Int16 = 2                  ' can be any number
    Public initVector As String = "@1B2c3D4e5F6g7H8" ' must be 16 bytes
    Public keySize As Int16 = 256                ' can be 192 or 128

    
    
    Public Function EXECSP(ByVal str As String, Optional ByVal MyCtrl As ControlCollection = Nothing) As Boolean
        EXECSP = False
        Dim objCon As New SqlConnection(sConn)
        Dim str1 As String

        Try
            If (IsNothing(MyCtrl) = False) Then
                Try
                    Dim ParamSQL As [String] = GetParamSQL(str, MyCtrl)

                    str1 = (str & Convert.ToString(" ")) + ParamSQL
                    Dim objCmd As New SqlCommand(str1, objCon)
                    objCmd.CommandType = CommandType.Text
                    objCmd.CommandTimeout = 2147483647
                    objCon.Open()

                    objCmd.ExecuteNonQuery()
    
                Catch ex As Exception
                    EXECSP = False
                    objCon.Dispose()
                    Throw New Exception(ex.Message)
                End Try


            Else
                str1 = ReplaceString(str)
                Dim objCmd As New SqlCommand(str, objCon)
                objCmd.CommandType = CommandType.Text
                objCmd.CommandTimeout = 2147483647
                objCon.Open()
                objCmd.ExecuteNonQuery()
            End If

            EXECSP = True
            objCon.Dispose()

        Catch ex As Exception
            Errstr = ex.ToString
            'InsertError(str1, ex.Message)
            objCon.Dispose()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function EXECSPWithErrorReturn(ByVal str As String, Optional ByVal MyCtrl As ControlCollection = Nothing) As String
        Dim objCon As New SqlConnection(sConn)
        Dim str1 As String

        Try
            If (IsNothing(MyCtrl) = False) Then
                Try
                    Dim ParamSQL As [String] = GetParamSQL(str, MyCtrl)

                    str1 = (str & Convert.ToString(" ")) + ParamSQL
                    Dim objCmd As New SqlCommand(str1, objCon)
                    objCmd.CommandType = CommandType.Text
                    objCmd.CommandTimeout = 2147483647
                    objCon.Open()

                    objCmd.ExecuteNonQuery()

                Catch ex As Exception
                    objCon.Dispose()
                    Throw New Exception(ex.Message)
                End Try


            Else
                str1 = ReplaceString(str)
                Dim objCmd As New SqlCommand(str, objCon)
                objCmd.CommandType = CommandType.Text
                objCmd.CommandTimeout = 2147483647
                objCon.Open()
                objCmd.ExecuteNonQuery()
            End If

            objCon.Dispose()
            Return "SUCCESS"
        Catch ex As Exception
            'InsertError(str1, ex.Message)
            objCon.Dispose()
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function InsertError(ByVal Sql As String, ByVal ErrorMessage As String)
        Dim objCon As New SqlConnection(sConn)
        InsertError = ""

        Dim objCmd As New SqlCommand("SP_ErrorLogInsert '" & Replace(ErrorMessage, "'", "''") & "' , '" & Replace(Sql, "'", "''") & "','" & HttpContext.Current.Request.Cookies("UserInfo")("FcBa") & "'", objCon)
        objCon.Open()
        objCmd.ExecuteNonQuery()

        objCon.Dispose()
    End Function

    Public Function GetQueryData(ByVal Sql As String, Optional ByVal MyCtrl As ControlCollection = Nothing) As Data.DataTable
        Dim objCon As New SqlConnection(sConn)
        Dim objDa As New SqlDataAdapter()
        Dim objDt As New DataTable("Data")
        Dim str1 As String

        Try
            If MyCtrl IsNot Nothing Then
                Dim ParamSQL As [String] = GetParamSQL(Sql, MyCtrl)
                str1 = (Sql & Convert.ToString(" ")) + ParamSQL
                Dim objCmd As New SqlCommand(str1, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            Else
                str1 = ReplaceString(Sql)
                Dim objCmd As New SqlCommand(Sql, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            End If
            objCon.Dispose()
        Catch ex As Exception
            'InsertError(str1, ex.Message)
            Throw New Exception(ex.Message)
            objCon.Dispose()
        End Try
        Return objDt
    End Function

    Public Function GetQueryDataEX(ByVal Sql As String, Optional ByVal MyCtrl As ControlCollection = Nothing) As Data.DataTable
        Dim objCon As New SqlConnection(sConn)
        Dim objDa As New SqlDataAdapter()
        Dim objDt As New DataTable("Data")
        Dim str1 As String = Sql

        Try
            If MyCtrl IsNot Nothing Then
                Dim ParamSQL As [String] = GetNullableParamSQL(Sql, MyCtrl)
                str1 = (Sql & Convert.ToString(" ")) + ParamSQL
                Dim objCmd As New SqlCommand(str1, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            Else
                str1 = ReplaceString(Sql)
                Dim objCmd As New SqlCommand(Sql, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            End If
            objCon.Dispose()
        Catch ex As Exception
            'InsertError(str1, ex.Message)
            Throw New Exception(ex.Message)
            objCon.Dispose()
        End Try
        Return objDt
    End Function

    Public Function GetQueryDataWithErrorDesc(ByVal Sql As String, Optional ByVal MyCtrl As ControlCollection = Nothing) As Data.DataTable
        Dim objCon As New SqlConnection(sConn)
        Dim objDa As New SqlDataAdapter()
        Dim objDt As New DataTable("Data")
        Dim str1 As String = String.Empty

        Try
            If MyCtrl IsNot Nothing Then
                Dim ParamSQL As [String] = GetParamSQL(Sql, MyCtrl)
                str1 = (Sql & Convert.ToString(" ")) + ParamSQL
                Dim objCmd As New SqlCommand(str1, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            Else
                str1 = ReplaceString(Sql)
                Dim objCmd As New SqlCommand(Sql, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            End If
            objCon.Dispose()
        Catch ex As Exception
            'InsertError(str1, ex.Message)
            Throw New Exception(ex.Message)
            objCon.Dispose()
        End Try
        Return objDt
    End Function

    
    End Function

#Region "FillControl"

    
    Public Function ExecSPToGridView(ByVal SP As String, ByVal GridName As GridView, Optional MyCtrl As ControlCollection = Nothing, Optional MyCtrl2 As ControlCollection = Nothing) As Boolean
        ExecSPToGridView = False
        Dim MyDSet As New DataSet
        Dim objCon As New SqlConnection(sConn)
        Dim str1 As String = ""
        If MyCtrl IsNot Nothing Then
            Dim ParamSQL As [String] = GetParamSQL(SP, MyCtrl)
            If IsNothing(MyCtrl2) = False Then
                Dim ParamSQL2 As [String] = GetParamSQL(SP, MyCtrl2)
            End If
            str1 = (SP & Convert.ToString(" ")) + ParamSQL
        Else
            str1 = SP
        End If
        Dim objAdpt As New SqlDataAdapter(str1, objCon)
        Try
            objCon.Open()
            objAdpt.Fill(MyDSet)
            GridName.DataSource = MyDSet
            GridName.DataBind()
            ExecSPToGridView = True
        Catch ex As Exception
            Errstr = ex.ToString
            InsertError(str1, ex.Message)
            objCon.Dispose()
            'Throw New Exception(ex.Message)
        End Try
        objCon.Dispose()
    End Function

    Public Function ExecSPToGridViewEX(ByVal SP As String, ByVal GridName As GridView, Optional MyCtrl As ControlCollection = Nothing, Optional MyCtrl2 As ControlCollection = Nothing) As Boolean
        ExecSPToGridViewEX = False
        Dim MyDSet As New DataSet
        Dim objCon As New SqlConnection(sConn)
        Dim str1 As String = ""
        If MyCtrl IsNot Nothing Then
            Dim ParamSQL As [String] = GetNullableParamSQL(SP, MyCtrl)
            If IsNothing(MyCtrl2) = False Then
                Dim ParamSQL2 As [String] = GetNullableParamSQL(SP, MyCtrl2)
            End If
            str1 = (SP & Convert.ToString(" ")) + ParamSQL
        Else
            str1 = SP
        End If
        Dim objAdpt As New SqlDataAdapter(str1, objCon)
        Try
            objCon.Open()
            objAdpt.Fill(MyDSet)
            GridName.DataSource = MyDSet
            GridName.DataBind()
            ExecSPToGridViewEX = True
        Catch ex As Exception
            Errstr = ex.ToString
            InsertError(str1, ex.Message)
            objCon.Dispose()
            'Throw New Exception(ex.Message)
        End Try
        objCon.Dispose()
    End Function

    Public Function ExecSPToGridViewWithError(ByVal SP As String, ByVal GridName As GridView, Optional MyCtrl As ControlCollection = Nothing, Optional MyCtrl2 As ControlCollection = Nothing) As Boolean
        ExecSPToGridViewWithError = False
        Dim MyDSet As New DataSet
        Dim objCon As New SqlConnection(sConn)
        Dim str1 As String = ""
        If MyCtrl IsNot Nothing Then
            Dim ParamSQL As [String] = GetParamSQL(SP, MyCtrl)
            If IsNothing(MyCtrl2) = False Then
                Dim ParamSQL2 As [String] = GetParamSQL(SP, MyCtrl2)
            End If
            str1 = (SP & Convert.ToString(" ")) + ParamSQL
        Else
            str1 = SP
        End If
        Dim objAdpt As New SqlDataAdapter(str1, objCon)
        Try
            objCon.Open()
            objAdpt.Fill(MyDSet)
            GridName.DataSource = MyDSet
            GridName.DataBind()
            ExecSPToGridViewWithError = True
        Catch ex As Exception
            Errstr = ex.ToString
            InsertError(str1, ex.Message)
            objCon.Dispose()
            Throw New Exception(ex.Message)
        End Try
        objCon.Dispose()
    End Function

    Public Function DBDataSet(ByVal sp As String, Optional MyCtrl As ControlCollection = Nothing) As Data.DataSet
        Dim objCon As New SqlConnection(sConn)
        Dim str1 As String = ""
        If MyCtrl IsNot Nothing Then
            Dim ParamSQL As [String] = GetParamSQL(sp, MyCtrl)

            str1 = (sp & Convert.ToString(" ")) + ParamSQL
        Else
            str1 = sp
        End If
        Dim mySqlCommand As New SqlCommand(str1, objCon)
        Dim mySqlAdapter As New SqlDataAdapter(mySqlCommand)
        Dim myDataSet As New DataSet
        mySqlAdapter.Fill(myDataSet)
        Return myDataSet
    End Function
    Public Function ExecSPSaclar(ByVal sp As String, Optional MyCtrl As ControlCollection = Nothing) As String
        Dim Cmd As New SqlCommand
        Dim objCon As New SqlConnection(sConn)
        Dim Hsl As SqlDataReader
        Dim str1 As String = ""
        If MyCtrl IsNot Nothing Then
            Dim ParamSQL As [String] = GetParamSQL(sp, MyCtrl)

            str1 = (sp & Convert.ToString(" ")) + ParamSQL
        Else
            str1 = sp
        End If
        Cmd.Connection = objCon
        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.CommandText = str1
        Cmd.Parameters("@HeadTxtfcCode").Direction = ParameterDirection.Output
        Cmd.ExecuteScalar()
        Hsl = Cmd.Parameters("@HeadTxtfcCode").Value
        Return Hsl.ToString
    End Function
#End Region

    
    
    Public Function InsertIntoDR(ByVal strSQL As String) As SqlDataReader
        'Cmd.Dispose()
        Try
            ConnDB.Close()
        Catch ex As Exception

        End Try

        Dim Cmd As New SqlCommand
        If ConnDB.State <> ConnectionState.Open Then
            ConnDB.ConnectionString = sConn
            ConnDB.Open()
        End If
        Cmd.Connection = ConnDB
        Cmd.CommandText = strSQL
        Cmd.CommandType = CommandType.Text
        InsertIntoDR = Cmd.ExecuteReader()
        ConnDB.Close()
    End Function


    
    Public Function ExecSPWithControlList(SP As String, Optional i As Integer = 0, Optional Ctrl As ControlCollection = Nothing) As [Boolean]
        Dim Sts As [Boolean] = False
        Dim Str1 As String = ""
        Dim Str2 As String = ""

        Try
            If Ctrl IsNot Nothing Then
                Dim ParamSQL As [String] = GetParamSQL(SP, Ctrl)
                Str1 = (SP & Convert.ToString(" ")) + ParamSQL
            Else
                Str1 = ReplaceString(SP)
            End If
            Sts = EXECSP(Str1)
        Catch ex As Exception
            Throw New Exception("public DataTable DisplayData() Error : " + Environment.NewLine + ex.Message.ToString())
        End Try
        Return Sts

    End Function

    Public Function GetParamSQL(SP As String, Ctrl As ControlCollection) As String
        Dim SQL1 As String = ""
        If InStr(SP, "'", CompareMethod.Text) Then
            SP = Mid(SP, 1, InStr(SP, "'", CompareMethod.Text) - 1)
        End If
        Dim sConn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("Db1ConnectionString").ConnectionString)
        Dim sqlCommand As SqlCommand = sConn.CreateCommand()
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.CommandText = SP
        sqlCommand.CommandTimeout = 10
        Try
            sConn.Open()
            Dim CachedParams As Object = System.Web.HttpContext.Current.Cache(SP & Convert.ToString("_Parameters"))
            If CachedParams Is Nothing Then
                SqlCommandBuilder.DeriveParameters(sqlCommand)
                System.Web.HttpContext.Current.Cache.Insert(SP & Convert.ToString("_Parameters"), sqlCommand.Parameters)

                For i As Integer = 1 To sqlCommand.Parameters.Count - 1
                    Dim ctr As String = sqlCommand.Parameters(i).ParameterName
                    Dim val1 As String = GetControlValue(ctr, Ctrl)
                    SQL1 = (Convert.ToString(SQL1 & Convert.ToString(",'")) & val1) + "'"
                Next
            Else

                SqlCommandBuilder.DeriveParameters(sqlCommand)
                System.Web.HttpContext.Current.Cache.Insert(SP & Convert.ToString("_Parameters"), sqlCommand.Parameters)

                For i As Integer = 1 To sqlCommand.Parameters.Count - 1
                    Dim ctr As String = sqlCommand.Parameters(i).ParameterName
                    Dim val1 As String = GetControlValue(ctr, Ctrl)
                    SQL1 = (Convert.ToString(SQL1 & Convert.ToString(",'")) & val1) + "'"
                Next

            End If

        Catch ex As Exception

            SQL1 = ex.ToString()
        Finally
            sqlCommand.Dispose()
            sConn.Close()
            sConn.Dispose()
        End Try
        SQL1 = SQL1.Substring(1, SQL1.Length - 1)
        Return SQL1
    End Function

    
   
    Public Function GetNullableParamSQL(SP As String, Ctrl As ControlCollection) As String
        Dim SQL1 As String = ""
        If InStr(SP, "'", CompareMethod.Text) Then
            SP = Mid(SP, 1, InStr(SP, "'", CompareMethod.Text) - 1)
        End If
        Dim sConn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("Db1ConnectionString").ConnectionString)
        Dim sqlCommand As SqlCommand = sConn.CreateCommand()
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.CommandText = SP
        sqlCommand.CommandTimeout = 10
        Try
            sConn.Open()
            Dim CachedParams As Object = System.Web.HttpContext.Current.Cache(SP & Convert.ToString("_Parameters"))
            If CachedParams Is Nothing Then
                SqlCommandBuilder.DeriveParameters(sqlCommand)
                System.Web.HttpContext.Current.Cache.Insert(SP & Convert.ToString("_Parameters"), sqlCommand.Parameters)

                For i As Integer = 1 To sqlCommand.Parameters.Count - 1
                    Dim ctr As String = sqlCommand.Parameters(i).ParameterName
                    Dim val1 As String = GetControlValue(ctr, Ctrl)
                    If val1 <> "" Then SQL1 = (Convert.ToString(SQL1 & Convert.ToString(",'")) & val1) + "'"
                Next
            Else

                SqlCommandBuilder.DeriveParameters(sqlCommand)
                System.Web.HttpContext.Current.Cache.Insert(SP & Convert.ToString("_Parameters"), sqlCommand.Parameters)

                For i As Integer = 1 To sqlCommand.Parameters.Count - 1
                    Dim ctr As String = sqlCommand.Parameters(i).ParameterName
                    Dim val1 As String = GetControlValue(ctr, Ctrl)
                    If val1 <> "" Then SQL1 = (Convert.ToString(SQL1 & Convert.ToString(",'")) & val1) + "'"
                Next
            End If
        Catch ex As Exception
            SQL1 = ex.ToString()
        Finally
            sqlCommand.Dispose()
            sConn.Close()
            sConn.Dispose()
        End Try
        SQL1 = SQL1.Substring(1, SQL1.Length - 1)
        Return SQL1
    End Function

    Public Function GetNullableQueryData(ByVal Sql As String, Optional ByVal MyCtrl As ControlCollection = Nothing) As Data.DataTable
        Dim objCon As New SqlConnection(sConn)
        Dim objDa As New SqlDataAdapter()
        Dim objDt As New DataTable("Data")
        Dim str1 As String = ""

        Try
            If MyCtrl IsNot Nothing Then
                Dim ParamSQL As [String] = GetNullableParamSQL(Sql, MyCtrl)
                str1 = (Sql & Convert.ToString(" ")) + ParamSQL
                Dim objCmd As New SqlCommand(str1, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            Else
                str1 = ReplaceString(Sql)
                Dim objCmd As New SqlCommand(Sql, objCon)
                objCon.Open()
                objDa.SelectCommand = objCmd
                objDa.Fill(objDt)
            End If

        Catch ex As Exception
            InsertError(str1, ex.Message)
            'Throw New Exception(ex.Message)
        End Try
        Return objDt
    End Function

    
End Class

