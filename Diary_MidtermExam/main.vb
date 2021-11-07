Imports MySql.Data.MySqlClient

Public Class main
    Public Property token As Integer  ' ID OF LOGGED-IN USER
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim READER As MySqlDataReader 'READS SQL OUTPUT

    Private Sub btnLogOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOut.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' GREET SESSION USER
        labelGreet.Text = "Welcome, '" & token & "'"

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
            ' STILL NEED TO FILTER ENTRIES HERE
            '  and username= '" & cbUser.Text & "' 
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
End Class