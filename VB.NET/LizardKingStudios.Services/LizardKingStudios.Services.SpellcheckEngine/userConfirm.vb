Imports System
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word

Partial Friend Class userConfirm
    Inherits Form

    Private _suggestionsList As SpellingSuggestions = Nothing
    Private _selectedValue As String = String.Empty

    Public Sub New()

        InitializeComponent()

    End Sub

    Public WriteOnly Property suggestionsList() As SpellingSuggestions
        Set(ByVal Value As SpellingSuggestions)

            _suggestionsList = Value

            loadSuggestionsList()
        End Set
    End Property

    Public ReadOnly Property selectedValue() As String
        Get
            Return _selectedValue
        End Get
    End Property

    Private Sub loadSuggestionsList()

        If (Not _suggestionsList Is Nothing) Then

            Dim theSuggestion As SpellingSuggestion
            For Each theSuggestion In _suggestionsList
                Suggestions.Items.Add(theSuggestion.Name)
            Next

            Suggestions.SelectedIndex = 0

        End If
    End Sub

    Friend WithEvents Suggestions As System.Windows.Forms.ListBox
    Friend WithEvents selectSuggestion As System.Windows.Forms.Button
    Friend WithEvents cancelForm As System.Windows.Forms.Button

    
    Private Sub selectSuggestion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles selectSuggestion.Click

        If (Suggestions.SelectedIndex = -1) Then

            MessageBox.Show("You must select a suggestion first!")

        Else

            _selectedValue = Suggestions.SelectedItem.ToString()

            MessageBox.Show("selected: " & _selectedValue)
            Me.Close()
        End If

    End Sub

    Private Sub cancelForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelForm.Click
        Me.Close()
    End Sub
End Class

