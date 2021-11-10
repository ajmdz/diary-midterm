Imports MySql.Data.MySqlClient

Public Class register
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim READER As MySqlDataReader
    Dim query As String

    'FUNCTION TO CHECK IF A USERNAME ALREADY EXISTS
    Private Function isRegistered(ByVal username As String) As Boolean
        Dim found As Boolean
        Dim count As Integer
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        ' ALWAYS TEST CONNECTION
        Try
            mysqlconn.Open()

            'CHECK IF USERNAME IS IN THE DB
            query = "SELECT * FROM diary.users WHERE username='" & username & "' "
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            'COUNT IF THERE IS A MATCH
            count = 0
            While READER.Read
                count = count + 1
            End While

            'RETURN 
            If count >= 1 Then
                found = True
            Else
                found = False
            End If

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
        Return found
    End Function

    ' REGISTER - INSERT FORM FIELD DATA TO DB
    Private Sub btnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"


        If isRegistered(tbUser.Text) = True Then    'USERNAME IS TAKEN
            MessageBox.Show("Username is already taken")
        Else 'USERNAME IS AVAILABLE -> INSERT THE DATA

            Try
                mysqlconn.Open()
                query = "INSERT INTO diary.users (fname,lname,dob,email,username,password) VALUES('" & tbFName.Text & "', '" & tbLName.Text & "', '" & tbDOB.Text & "','" & tbEmail.Text & "', '" & tbUser.Text & "', '" & tbPass.Text & "')"
                command = New MySqlCommand(query, mysqlconn)
                READER = command.ExecuteReader

                MessageBox.Show("Registration Successful!")

                mysqlconn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                mysqlconn.Dispose()
            End Try
        End If
    End Sub

    ' EDIT BUTTON INCOMPLETE
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        ' ALWAYS TEST CONNECTION
        Try
            mysqlconn.Open()
            query = "UPDATE diary.users SET fname='" & tbFName.Text & "', lname='" & tbLName.Text & "', dob='" & tbDOB.Text & "', email='" & tbEmail.Text & "', username='" & tbUser.Text & "', password='" & tbPass.Text & "'  " 'ADD WHERE CLAUSE
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            MessageBox.Show("Registration Successful!")

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Form1.Show()
        Me.Hide()
    End Sub
End Class