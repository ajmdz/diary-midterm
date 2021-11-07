Imports MySql.Data.MySqlClient

Public Class Form1
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim READER As MySqlDataReader 'READS SQL OUTPUT

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"
        Dim READER As MySqlDataReader


        Try
            mysqlconn.Open()
            Dim query As String
            query = "SELECT * FROM diary.users WHERE username= '" & tbUser.Text & "' and password = '" & tbPass.Text & "'"

            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader
            Dim count As Integer
            Dim id As Integer

            count = 0
            While READER.Read
                count = count + 1
                id = READER.GetUInt32("user_id")
            End While

            If count >= 1 Then      ' LOGIN SUCCESSFUL
                ' PASS USER ID TO INSTANCE OF MAIN FORM
                Dim OBJ As New main
                OBJ.token = id
                OBJ.Show()
                Me.Hide()

            Else                    ' LOGIN FAILED
                MessageBox.Show("Invalid username/password")
            End If


            mysqlconn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try

        
    End Sub


    Private Sub btnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        register.Show()
        Me.Hide()
    End Sub
End Class
