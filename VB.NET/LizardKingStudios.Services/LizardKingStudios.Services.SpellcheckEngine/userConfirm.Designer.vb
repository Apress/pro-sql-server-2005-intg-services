<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class userConfirm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Suggestions = New System.Windows.Forms.ListBox
        Me.selectSuggestion = New System.Windows.Forms.Button
        Me.cancelForm = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Suggestions
        '
        Me.Suggestions.FormattingEnabled = True
        Me.Suggestions.Location = New System.Drawing.Point(12, 12)
        Me.Suggestions.Name = "Suggestions"
        Me.Suggestions.Size = New System.Drawing.Size(312, 199)
        Me.Suggestions.TabIndex = 0
        '
        'selectSuggestion
        '
        Me.selectSuggestion.Location = New System.Drawing.Point(330, 154)
        Me.selectSuggestion.Name = "selectSuggestion"
        Me.selectSuggestion.Size = New System.Drawing.Size(75, 23)
        Me.selectSuggestion.TabIndex = 1
        Me.selectSuggestion.Text = "&Select"
        Me.selectSuggestion.UseVisualStyleBackColor = True
        '
        'cancelForm
        '
        Me.cancelForm.Location = New System.Drawing.Point(331, 184)
        Me.cancelForm.Name = "cancelForm"
        Me.cancelForm.Size = New System.Drawing.Size(75, 23)
        Me.cancelForm.TabIndex = 2
        Me.cancelForm.Text = "&Cancel"
        Me.cancelForm.UseVisualStyleBackColor = True
        '
        'userConfirm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(411, 230)
        Me.Controls.Add(Me.cancelForm)
        Me.Controls.Add(Me.selectSuggestion)
        Me.Controls.Add(Me.Suggestions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "userConfirm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Suggestion"
        Me.ResumeLayout(False)

    End Sub
End Class
