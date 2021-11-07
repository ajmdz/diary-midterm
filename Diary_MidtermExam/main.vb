Imports MySql.Data.MySqlClient

Public Class main
    Public Property token As Integer  ' ID OF LOGGED-IN USER
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim READER As MySqlDataReader   'READS SQL OUTPUT
    Dim thisDate As Date            'DATE CLASS    


    Private Function fetchName(ByVal uid As Integer) As String
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"
        Dim READER As MySqlDataReader
        Dim name As String

        Try
            mysqlconn.Open()
            Dim query As String
            query = "SELECT fname FROM diary.users WHERE user_id= '" & uid & "'"
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            While READER.Read
                name = READER.GetString("fname")
            End While

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try

        Return name
    End Function

    Private Sub btnLogOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOut.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' GREET SESSION USER
        Dim sUserFNAME As String
        sUserFNAME = fetchName(token)
        labelGreet.Text = "Welcome, " & sUserFNAME

        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        Try
            mysqlconn.Open()
            Dim query As String
            query = "SELECT * FROM diary.entries where user_id= '" & token & "'"
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            While READER.Read
                Dim sTitle = READER.GetString("title")
                cbTitles.Items.Add(sTitle)
            End While

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
    End Sub

    Private Sub cbTitles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTitles.SelectedIndexChanged
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        Try
            mysqlconn.Open()
            Dim query As String

            ' FILTER COMBOBOX TO ENTRIES BY SESSION USER ONLY
            query = "SELECT * FROM diary.entries where title= '" & cbTitles.Text & "' and user_id= '" & token & "'"
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            While READER.Read
                tbTitle.Text = READER.GetString("title")
                tbContent.Text = READER.GetString("content")
            End While

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            mysqlconn.Open()
            Dim query As String

            query = "INSERT INTO diary.entries (user_id, title, content, modified)  VALUES ('" & token & "', '" & tbTitle.Text & "', '" & tbContent.Text & "', '" & Format(Now, "yyyy-M-dd") & "' "
            command = New MySqlCommand(query, mysqlconn)

            mysqlconn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try

    End Sub
End Class