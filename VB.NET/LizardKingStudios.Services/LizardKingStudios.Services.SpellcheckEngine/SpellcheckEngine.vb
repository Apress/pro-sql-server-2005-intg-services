Imports System
Imports System.Reflection
Imports Microsoft.Office.Interop.Word

Public Class SpellcheckEngine

    Private _wordSpellChecker As Application = New Application()
    Private _maxSuggestionsCount As Integer = 5

    Public Sub New()

        Dim optionalValue As Object = Missing.Value

        _wordSpellChecker.Application.Documents.Add(optionalValue, optionalValue, optionalValue, optionalValue)

    End Sub

    Public Property MaxSuggestionsCount() As Integer
        Get
            Return _maxSuggestionsCount
        End Get
        Set(ByVal value As Integer)
            _maxSuggestionsCount = value
        End Set
    End Property

    Public Function Execute(ByVal checkValue As String) As String

        Dim suggestedValue As String = String.Empty

        If (checkValue.Length <> 0) Then

            Dim optionalValue As Object = Missing.Value
            Dim ignoreUppercase As Object = True
            Dim suggestionMode As Object = Missing.Value

            Dim theSuggestions As SpellingSuggestions = _wordSpellChecker.GetSpellingSuggestions(checkValue, optionalValue, _
                optionalValue, optionalValue, optionalValue, _
                optionalValue, optionalValue, optionalValue, optionalValue, _
                optionalValue, optionalValue, optionalValue, optionalValue, _
                optionalValue)

            ' if we get more than (parameter specified) suggestions, don't make
            ' the change.
            If (theSuggestions.Count > 0 And theSuggestions.Count <= _maxSuggestionsCount) Then

                Try
                    If (theSuggestions.Count > 1) Then
                        Dim theUserConfirmWindow As userConfirm = New userConfirm()
                        theUserConfirmWindow.suggestionsList = theSuggestions
                        theUserConfirmWindow.ShowDialog()
                        suggestedValue = theUserConfirmWindow.selectedValue
                    Else

                        Dim theSuggestion As SpellingSuggestion = theSuggestions(1)
                        suggestedValue = theSuggestion.Name

                    End If
                Catch
                    ' lets swallow it - implement error handling here!
                End Try

            End If

        End If

        Return suggestedValue

    End Function

End Class