Imports MySql.Data.MySqlClient

Public Class register
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim READER As MySqlDataReader 'READS SQL OUTPUT

    ' FUNCTION FOR CHECKING IF USER ALREADY EXISTS
    Private Function isRegistered(ByVal username As String) As Boolean
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"
        Dim READER As MySqlDataReader
        Dim flag As Boolean

        Try
            mysqlconn.Open()
            Dim query As String
            query = "SELECT * FROM diary.users WHERE username= '" & tbUser.Text & "'"
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader
            Dim count As Integer

            count = 0
            While READER.Read
                count = count + 1
            End While

            If count = 1 Then
                flag = True
            Else
                flag = False
            End If

            mysqlconn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try

        Return flag
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub btnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegister.Click

        ' Check if username already exists
        If isRegistered(tbUser.Text) Then
            MessageBox.Show("Username is already taken.")
        Else
            mysqlconn.Open()
            Dim query As String
            query = "INSERT INTO diary.users (fname, lname, dob, email, username, password)  VALUES ('" & tbFName.Text & "', '" & tbLName.Text & "', '" & tbDOB.Text & "', '" & tbEmail.Text & "', '" & tbUser.Text & "', '" & tbPass.Text & "')"
            command = New MySqlCommand(query, mysqlconn)
            ' READER = command.ExecuteReader

            ' DOUBLE CHECK IF SUCCESSFULLY REGISTERED
            If isRegistered(tbUser.Text) Then
                MessageBox.Show("Successfully Registered")
                Form1.Show()
                Me.Hide()

            End If
        End If

        
    End Sub
End Class