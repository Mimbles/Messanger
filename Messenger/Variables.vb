Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Net
Imports System.IO
Module Variables 'A list of golbal variables that can be accessed by all forms
    Public rsaDecryptor, rsaEncrpytor As New RSACryptoServiceProvider(2048) 'generates the public and private keys with a key size of 2048 bits
    Public clientName As String
    Public userName As String
    Public FileName As String = "Name.txt"
    Public connected As Boolean = False
    Public clientIP As String
End Module
