Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Public Class Connect
    Public tcpClient As TcpClient
    Public listener As New TcpListener(6000)
    Public recieveClient As New TcpClient
    Public publicKey As String
    Public clientPublicKey As String
    Public Sub Connect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblIp.Text = Dns.GetHostByName(Dns.GetHostName).AddressList(0).ToString() 'Outputs the local IP of the computer
        listener.Start() 'binds the listener to the port
        publicKey = rsaDecryptor.ToXmlString(False) 'saves the public key from the rsadecrypter to the variable publicKey
        tmrRecieve.Start() 'starts the tmrRecieve
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CloseForm() 'runs the sub closeForm
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Regex.Match(TextBox1.Text, "\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b").Success Then 'validates the input is a valid ip address (xxx.xxx.xxx.xxx) 
            Send(TextBox1.Text) 'runs the send sub and give it the text in the textbox
        Else
            MsgBox("Please enter a valid IP") 'outputs an error messege as a messagebox
        End If
    End Sub

    Sub CloseForm()
        Dim Messanger As New Messanger
        listener.Stop() 'stops the listener 
        Messanger.Show() 'shows form1
        tmrRecieve.Stop() 'stops the tmrRecieve
        Me.Close() 'closes this form
    End Sub

    Sub Send(ByVal ip As String)
        Try
            tcpClient = New TcpClient(ip, "6000") 'starts a new tcpClient with the entered destination ip
            Using writer As New StreamWriter(tcpClient.GetStream) 'starts a streamwriter that can write over the network
                writer.Write(userName & "¬" & publicKey) 'sends the Name and publicKey seperated with a "¬"
            End Using
        Catch ex As Exception
            MsgBox("Error: Client not found") 'outputs an error messege as a messagebox
        End Try
    End Sub
    Private Sub tmrRecieve_Tick(sender As Object, e As EventArgs) Handles tmrRecieve.Tick
        If listener.Pending Then 'checks if there is an incoming conection
            Dim recieved As String
            recieveClient = listener.AcceptTcpClient() 'listener accepts the connection and gives it to a tcpClient
            clientIP = (IPAddress.Parse(CType(recieveClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString())).ToString() 'Gets the ip of the computer sending the data and converts it to a string
            Using reader As New StreamReader(recieveClient.GetStream) 'starts a new streamreader that can read from the network 
                recieved = reader.ReadToEnd 'reads all of the incoming data
            End Using
            clientName = recieved.Split("¬")(0) 'splits the recieved data and gets the first half
            clientPublicKey = recieved.Split("¬")(1) 'splits the recieved data and gets the second half
            rsaEncrpytor.FromXmlString(clientPublicKey) 'imports the public key
            Send(clientIP) 'calls the send sub and gives it the recieved ip
            connected = True
            CloseForm()
        End If
    End Sub
End Class