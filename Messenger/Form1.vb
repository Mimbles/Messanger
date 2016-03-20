Imports System
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Public Class Messanger
    Public listener As TcpListener 'The listener that looks for incoming connnections
    Public recieveTCP As New TcpClient 'The TCP client used for recieving data

    Sub Send(ByVal encryptedBytes() As Byte) 'Sub to send the encrypted data 
        Dim client As New TcpClient(clientIP, 6001) 'starts a new TCP client used to send the data
        Dim stream As NetworkStream = client.GetStream 'Starts a network stream linked to the TCPClient to be written to
        stream.Write(encryptedBytes, 0, encryptedBytes.Length) 'Writes the encrypted data to the network stream
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        If connected = True Then
            txtHistory.Text = txtHistory.Text & Time() & userName & ": " & txtSend.Text & System.Environment.NewLine 'Updates the txthistory with: the current time, Who sent it, the message then it adds a new line
            Send(Encrypt()) 'Runs the send sub and sends it the value from Encrypt()
            txtSend.Clear() 'deletes all text in txtsend
        Else
            MsgBox("Not connected")
        End If
    End Sub

    Function Time() 'returns the current time (hours:minutes)
        Return TimeOfDay.Hour & ":" & TimeOfDay.Minute & " "
    End Function
    Function Encrypt()
        Dim bytesToEncrypt() As Byte
        bytesToEncrypt = ASCIIEncoding.ASCII.GetBytes(txtSend.Text) 'Converts the entered string into bytes
        Encrypt = rsaEncrpytor.Encrypt(bytesToEncrypt, False) 'Encrypts the Bytes with the clients Public key
        Return Encrypt 'Returns the encrypted data
    End Function

    Function Decrpyt(ByVal recievedBytes() As Byte)
        Decrpyt = rsaDecryptor.Decrypt(recievedBytes, False) 'Decrypts the recieved bytes with the Private key
        Return Decrpyt 'Returns the decrypted data
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtHistory.ScrollBars = ScrollBars.Vertical 'Adds a scroll bar to txthistory

        Dim file As FileStream = New FileStream(FileName, FileMode.Open)
        Using reader As New StreamReader(file)
            userName = reader.ReadLine() 'Opens "Name.txt" and reads the name in
        End Using

        If connected = True Then 'Checks if it's connected to a client
            Label2.Text = clientName 'Changes the label to the name of the client
            listener = New TcpListener(6001) 'Binds a listener to the prot 6001
            listener.Start() 'Starts listening on the port for incoming connections
            tmrRecieveMessage.Start() 'starts the timer
        End If
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        If connected = False Then 'Checks if it's not connected to a client
            Dim Connect As New Connect
            Connect.Show() 'opens the connect form
            Me.Hide() 'hides the current form
        Else
            MsgBox("Already connected please click disconnect first")
        End If
    End Sub

    Private Sub tmrRecieveMessage_Tick(sender As Object, e As EventArgs) Handles tmrRecieveMessage.Tick 'Timer code is executed every 500ms
        If listener.Pending Then 'Checks if there is an incoming connection on the port
            recieveTCP = listener.AcceptTcpClient 'gives the recieveTCP the connection
            Dim stream As NetworkStream = recieveTCP.GetStream 'starts a network stream
            Dim recievedBytes(255) As Byte
            stream.Read(recievedBytes, 0, recievedBytes.Length) 'reads from the network stream adn saves it into the variable bytes
            txtHistory.Text = txtHistory.Text & Time() & clientName & ": " & ASCIIEncoding.ASCII.GetString(Decrpyt(recievedBytes)) & System.Environment.NewLine 'updates txthistory with: the time recieved, client name, received message and adds a new line
            stream.Close() 'closes the netowrk stream
            recieveTCP.Close() 'closes the TCPClient
        End If
    End Sub

    Private Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        If connected = True Then 'checks if it's connected
            listener.Stop() 'stops the listener 
            tmrRecieveMessage.Stop() 'stops the timer
            connected = False 'changes the state to disconnected
            Label2.Text = "Not Connected" 'updated label2 to show it's no longer connected
        Else
            MsgBox("Not connected please connect first")
        End If
    End Sub


End Class
