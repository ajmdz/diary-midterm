Imports MySql.Data.MySqlClient

Public Class main
    Public Property token As Integer    ' ID OF LOGGED-IN USER
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim READER As MySqlDataReader       'READS SQL OUTPUT
    Dim thisDate As Date                'DATE CLASS    
    Dim query As String

    'FUNCTION FOR FETCHING USER'S FIRST NAME VIA USER_ID
    Private Function fetchName(ByVal uid As Integer) As String
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"
        Dim READER As MySqlDataReader
        Dim name As String

        Try
            mysqlconn.Open()
            query = "SELECT fname FROM diary.users WHERE user_id= '" & uid & "'"
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            'GET DATA FROM FNAME COLUMN
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

    'FUNCTION FOR REFRESHING COMBOBOX
    Public Sub cbRefresh()
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        'Clear the combobox
        cbTitles.SelectedIndex = -1
        cbTitles.DataSource = Nothing
        cbTitles.Items.Clear()

        Try
            mysqlconn.Open()

            'ONLY ADD ENTRIES THAT ARE OWNED BY CURRENT USER TO THE COMBOBOX
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

    'FUNCTION FOR FETCHING ENTRY_ID VIA TITLE AND USER_ID
    Private Function fetchEID(ByVal title As String, ByVal uid As Integer)
        Dim EID As Integer
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        Try
            mysqlconn.Open()

            'GET ENTRY_ID FIRST
            query = "SELECT entry_id FROM diary.entries where title= '" & cbTitles.Text & "' and user_id= '" & token & "' "
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            While READER.Read
                EID = READER.GetUInt32("entry_id")
                'DEBUGGING: MessageBox.Show(EID)
            End While

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
        Return EID
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

            'ONLY ADD ENTRIES THAT ARE OWNED BY CURRENT USER TO THE COMBOBOX
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

            ' FILTER COMBOBOX TO ENTRIES BY SESSION USER ONLY
            query = "SELECT * FROM diary.entries where title= '" & cbTitles.Text & "' "
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

    ' THIS IS AN "INSERT" FUNCTIONALITY
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dateToday As String
        dateToday = Format(Now, "yyyy-M-dd")
        'dateToday = "2021-11-08"

        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        Try
            mysqlconn.Open()

            'INSERT FORM DATA TO DB
            query = "INSERT INTO diary.entries (user_id, title, content, modified) VALUES('" & token & "', '" & tbTitle.Text & "', '" & tbContent.Text & "','" & dateToday & "')"
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader
            MessageBox.Show("Entry Added Successfully")

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
        cbRefresh()
    End Sub

    ' DELETE FUNCTIONALITY
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        'GET ENTRY_ID FIRST
        Dim entryID As Integer
        entryID = fetchEID(cbTitles.Text, token)

        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        Try
            mysqlconn.Open()

            'DELETE ENTRY
            query = "DELETE FROM diary.entries where entry_id= '" & entryID & "' "
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            MessageBox.Show("Entry has been deleted")

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
        cbRefresh()
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        'GET ENTRY_ID FIRST
        Dim entryID As Integer
        entryID = fetchEID(cbTitles.Text, token)

        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString =
            "server=localhost;userid=root;database=diary"

        ' ALWAYS TEST CONNECTION
        Try
            mysqlconn.Open()

            ' UPDATE TITLE AND/OR CONTENT OF AN ENTRY
            query = "UPDATE diary.entries SET title='" & tbTitle.Text & "', content='" & tbContent.Text & "' WHERE entry_id='" & entryID & "'" 'WHERE CLAUSE IS IMPORTANT
            command = New MySqlCommand(query, mysqlconn)
            READER = command.ExecuteReader

            MessageBox.Show("Changes Saved")

            mysqlconn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try
        cbRefresh()
    End Sub


End Class