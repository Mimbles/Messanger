Imports System.IO
Imports System.Text.RegularExpressions
Public Class EnterName

    Private Sub btnName_Click(sender As Object, e As EventArgs) Handles btnName.Click
        If Not Regex.Match(txtName.Text, "^[a-zA-Z]+(?: [a-zA-Z]+)*$", RegexOptions.IgnoreCase).Success Then 'validates the input so that only letters can be entered
            MsgBox("Please enter a valid name (Only Letters and Spaces)") 'output error message
        Else
            userName = txtName.Text 'saves the name entered by the user
            Dim fileWrite As FileStream = New FileStream(FileName, FileMode.Create) 'opens a new filstream that creates a textfile
            Using writer As New StreamWriter(fileWrite) 'starts a streamwriter limked to the filestream
                writer.Write(userName) 'writes the username to the textfile
            End Using
            fileWrite.Close() 'closes the file stream
            Timer1.Start() 'starts the timer
        End If
    End Sub

    Private Sub EnterName_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim fileOpen As FileStream = New FileStream(FileName, FileMode.Open) 'starts a filestream that opens the file
            Using reader As New StreamReader(fileOpen) 'starts a reader that can read from the filestream
                userName = reader.ReadLine() 'reads from the text file
            End Using
            Timer1.Start() 'starts the timer
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Hide() 'hides the form 
        Dim Messanger As New Messanger
        Messanger.Show() 'shows form1
        Timer1.Stop() 'stops the timer
    End Sub
End Class