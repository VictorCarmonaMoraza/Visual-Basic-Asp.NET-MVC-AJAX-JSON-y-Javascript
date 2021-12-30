


Imports System.Security.Cryptography
''' <summary>
''' Clase para el cifrado de las contraseñas
''' </summary>
Public Class Cifrado


    Public Function cifrar(cadena As String) As String
        Dim sha As SHA256Managed = New SHA256Managed()
        'Convertimso nuestra cadena a byte
        Dim dataNoCifrada As Byte() = Encoding.Default.GetBytes(cadena)
        'Cifrado de la cadema pasada
        Dim dataCifrada As Byte() = sha.ComputeHash(dataNoCifrada)
        'Eliminamos todos los guiones que tiene el cifrado
        'NOTA: BitConverter.ToString(dataCifrada) lo convierte a hexadecimal
        Dim data As String = BitConverter.ToString(dataCifrada).Replace("-", "")
        'Retornamos la data 
        Return data
    End Function

End Class
