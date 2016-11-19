
Imports System
Imports Microsoft.SqlServer.Dts
Imports Microsoft.SqlServer.Dts.Pipeline
Imports Microsoft.SqlServer.Dts.Pipeline.Wrapper
Imports Microsoft.SqlServer.Dts.Runtime.Wrapper
Imports LizardKingStudios.Services.SpellcheckEngine

Namespace LizardKingStudios.SSIS.DataflowTransformsVB

    <DtsPipelineComponent(ComponentType:=ComponentType.Transform, Description:="Sample Data Flow Component from the Apress book Pro SSIS", DisplayName:="Lizard King Spellchecker", IconResource:="LizardKingStudios.SSIS.DataFlowTranforms.Icon.ico")> _
    Public Class SpellingCheck
        Inherits PipelineComponent

        Dim outputBuffer As PipelineBuffer
        Private inputColumnBufferIndexes() As Integer
        Private outputColumnBufferIndexes() As Integer

        Private Sub PostError(ByVal message As String)

            Dim cancel As Boolean = False
            Me.ComponentMetaData.FireError(0, Me.ComponentMetaData.Name, message, "", 0, cancel)

        End Sub

        Private Function promoteStatus(ByVal currentStatus As DTSValidationStatus, ByVal NewStatus As DTSValidationStatus) As DTSValidationStatus
            Select Case currentStatus
                Case DTSValidationStatus.VS_ISVALID

                    Select Case NewStatus
                        Case DTSValidationStatus.VS_ISBROKEN

                        Case DTSValidationStatus.VS_ISCORRUPT

                        Case DTSValidationStatus.VS_NEEDSNEWMETADATA
                            currentStatus = NewStatus

                        Case DTSValidationStatus.VS_ISVALID

                        Case Else

                            Throw New System.ApplicationException("Internal Error: A value outside the scope of the status enumeration was found.")
                    End Select

                Case DTSValidationStatus.VS_ISBROKEN

                    Select Case NewStatus
                        Case DTSValidationStatus.VS_ISCORRUPT

                        Case DTSValidationStatus.VS_NEEDSNEWMETADATA

                            currentStatus = NewStatus
                        Case DTSValidationStatus.VS_ISVALID

                        Case DTSValidationStatus.VS_ISBROKEN

                        Case Else

                            Throw New System.ApplicationException("Internal Error: A value outside the scope of the status enumeration was found.")
                    End Select
                Case DTSValidationStatus.VS_NEEDSNEWMETADATA

                    Select Case NewStatus
                        Case DTSValidationStatus.VS_ISCORRUPT

                            currentStatus = NewStatus
                        Case DTSValidationStatus.VS_ISVALID

                        Case DTSValidationStatus.VS_ISBROKEN

                        Case DTSValidationStatus.VS_NEEDSNEWMETADATA

                        Case Else

                            Throw New System.ApplicationException("Internal Error: A value outside the scope of the status enumeration was found.")
                    End Select

                Case DTSValidationStatus.VS_ISCORRUPT

                    Select Case NewStatus
                        Case DTSValidationStatus.VS_ISCORRUPT

                        Case DTSValidationStatus.VS_ISVALID

                        Case DTSValidationStatus.VS_ISBROKEN

                        Case DTSValidationStatus.VS_NEEDSNEWMETADATA

                        Case Else

                            Throw New System.ApplicationException("Internal Error: A value outside the scope of the status enumeration was found.")
                    End Select
                Case Else

                    Throw New System.ApplicationException("Internal Error: A value outside the scope of the status enumeration was found.")
            End Select
            Return currentStatus

        End Function

        Public Overrides Function Validate() As DTSValidationStatus

            Dim status As DTSValidationStatus = MyBase.Validate()
            If (status = DTSValidationStatus.VS_ISCORRUPT) Then
                Return status
            End If

            Dim metadata As IDTSComponentMetaData90 = Me.ComponentMetaData
            Dim componentCustomProperties As IDTSCustomPropertyCollection90 = metadata.CustomPropertyCollection
            Try
                Dim customProperty As IDTSCustomProperty90 = componentCustomProperties("InputColumnToCheck")
                Dim s As String = CType(customProperty.Value, String)
                If (s.Length = 0) Then
                    PostError("InputColumnToCheck must be populated")
                    Return DTSValidationStatus.VS_ISCORRUPT
                End If
            Catch e As System.Runtime.InteropServices.COMException
                If (e.ErrorCode = HResults.DTS_E_ELEMENTNOTFOUND) Then
                    PostError("Custom property 'InputColumnToCheck' not found in component custom property collection")
                    Return DTSValidationStatus.VS_ISCORRUPT
                Else

                    Throw e
                End If
            End Try
            If (metadata.InputCollection.Count <> 1) Then

                PostError("Component requires exactly one input.")
                Return DTSValidationStatus.VS_ISCORRUPT
            End If

            Dim input As IDTSInput90 = metadata.InputCollection(0)
            Dim inputColumns As IDTSInputColumnCollection90 = input.InputColumnCollection

            Dim j As Integer
            For j = 0 To inputColumns.Count - 1 Step j + 1
                Dim column As IDTSInputColumn90 = inputColumns(j)
                If (column.IsValid) Then

                    ' validation code can go in here for allowing only string types
                End If
            Next
            Return status
        End Function

        Public Overrides Function InsertInput(ByVal insertPlacement As DTSInsertPlacement, ByVal inputID As Integer) As IDTSInput90

            PostError("Component requires exactly one input. New input is forbidden.")
            Throw New PipelineComponentHResultException(HResults.DTS_E_CANTADDINPUT)

        End Function

        Public Overrides Sub DeleteInput(ByVal inputID As Integer)

            PostError("Component requires exactly one input. Deleted input is forbidden.")
            Throw New PipelineComponentHResultException(HResults.DTS_E_CANTDELETEINPUT)

        End Sub

        Public Overrides Function InsertOutput(ByVal insertPlacement As DTSInsertPlacement, ByVal outputID As Integer) As IDTSOutput90

            PostError("Component requires exactly one output. New output is forbidden.")
            Throw New PipelineComponentHResultException(HResults.DTS_E_CANTADDOUTPUT)

        End Function

        Public Overrides Sub DeleteOutput(ByVal outputID As Integer)

            PostError("Component requires exactly one output. Deleted output is forbidden.")
            Throw New PipelineComponentHResultException(HResults.DTS_E_CANTDELETEOUTPUT)

        End Sub

        Public Overrides Function InsertOutputColumnAt(ByVal outputID As Integer, ByVal outputColumnIndex As Integer, ByVal name As String, ByVal description As String) As IDTSOutputColumn90

            PostError("Component does not allow addition of output columns.")
            Throw New PipelineComponentHResultException(HResults.DTS_E_CANTADDCOLUMN)

        End Function

        Public Overrides Function SetUsageType(ByVal inputID As Integer, ByVal virtualInput As IDTSVirtualInput90, ByVal lineageID As Integer, ByVal usageType As DTSUsageType) As IDTSInputColumn90

            Dim inputColumn As IDTSInputColumn90 = Nothing
            Select Case usageType
                Case DTSUsageType.UT_READWRITE


                Case DTSUsageType.UT_READONLY


                Case DTSUsageType.UT_IGNORED

                    inputColumn = MyBase.SetUsageType(inputID, virtualInput, lineageID, usageType)
                    Return inputColumn
                Case Else

                    Throw New PipelineComponentHResultException(HResults.DTS_E_CANTSETUSAGETYPE)
            End Select

        End Function

        Public Overrides Sub OnInputPathAttached(ByVal inputID As Integer)

            MyBase.OnInputPathAttached(inputID)

            Dim input As IDTSInput90 = ComponentMetaData.InputCollection.GetObjectByID(inputID)
            Dim output As IDTSOutput90 = ComponentMetaData.OutputCollection(0)
            Dim vInput As IDTSVirtualInput90 = input.GetVirtualInput()

            Dim vCol As IDTSVirtualInputColumn90
            For Each vCol In vInput.VirtualInputColumnCollection
                Dim outCol As IDTSOutputColumn90 = output.OutputColumnCollection.New()
                outCol.Name = vCol.Name
                outCol.SetDataTypeProperties(vCol.DataType, vCol.Length, vCol.Precision, vCol.Scale, vCol.CodePage)
            Next

            Dim outputColumn As IDTSOutputColumn90 = output.OutputColumnCollection.New()

            outputColumn.Name = "isSuggested"
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0)
            outputColumn = Me.ComponentMetaData.OutputCollection(0).OutputColumnCollection.New()
            outputColumn.Name = "suggestedValue"
            outputColumn.SetDataTypeProperties(DataType.DT_STR, 50, 0, 0, 1252)

        End Sub

        Public Overrides Sub ProvideComponentProperties()

            MyBase.ProvideComponentProperties()

            ComponentMetaData.OutputCollection(0).SynchronousInputID = 0

            Dim customProperties As IDTSCustomPropertyCollection90 = Me.ComponentMetaData.CustomPropertyCollection
            Dim customProperty As IDTSCustomProperty90 = customProperties.New()

            customProperty.Name = "InputColumnToCheck"
            customProperty.Value = ""
            customProperty = customProperties.New()
            customProperty.Name = "MaxSuggestionsCount"
            customProperty.Value = "5"

        End Sub

        Public Overrides Function SetComponentProperty(ByVal propertyName As String, ByVal propertyValue As Object) As IDTSCustomProperty90

            If (propertyName = "InputColumnToCheck") Then

                Dim value As String = CType(propertyValue, String)
                If (value.Length = 0) Then

                    PostError("InputColumnToCheck must be populated.")
                    Throw New PipelineComponentHResultException(HResults.DTS_E_FAILEDTOSETPROPERTY)

                Else

                    Return MyBase.SetComponentProperty(propertyName, propertyValue)
                End If

            Else

                PostError("Specified property name [" + propertyName + "] not expected.")
                Throw New PipelineComponentHResultException(HResults.DTS_E_FAILEDTOSETPROPERTY)
            End If
        End Function

        Public Overrides Sub PreExecute()

            Dim input As IDTSInput90 = ComponentMetaData.InputCollection(0)
            Dim output As IDTSOutput90 = ComponentMetaData.OutputCollection(0)

            inputColumnBufferIndexes = New Integer(input.InputColumnCollection.Count) {}
            outputColumnBufferIndexes = New Integer(output.OutputColumnCollection.Count) {}

            Dim col As Integer
            For col = 0 To input.InputColumnCollection.Count - 1 Step col + 1
                inputColumnBufferIndexes(col) = BufferManager.FindColumnByLineageID(input.Buffer, input.InputColumnCollection(col).LineageID)
            Next

            For col = 0 To output.OutputColumnCollection.Count - 1 Step col + 1
                outputColumnBufferIndexes(col) = BufferManager.FindColumnByLineageID(output.Buffer, output.OutputColumnCollection(col).LineageID)
            Next
        End Sub

        Public Overrides Sub PrimeOutput(ByVal outputs As Integer, ByVal outputIDs As Integer(), ByVal buffers As PipelineBuffer())

            If (buffers.Length <> 0) Then
                outputBuffer = buffers(0)
            End If

        End Sub

        Public Overrides Sub ProcessInput(ByVal inputID As Integer, ByVal buffer As PipelineBuffer)

            Dim suggestedValue As String = String.Empty
            Dim checkValue As String
            Dim spellEngine As SpellcheckEngine = New SpellcheckEngine()
            Dim colId As Integer = -1

            spellEngine.MaxSuggestionsCount = Convert.ToInt32(ComponentMetaData.CustomPropertyCollection("MaxSuggestionsCount").Value.ToString())
            Dim input As IDTSInput90 = ComponentMetaData.InputCollection.GetObjectByID(inputID)

            Dim colIndex As Integer
            For colIndex = 0 To input.InputColumnCollection.Count - 1 Step colIndex + 1
                If (input.InputColumnCollection(colIndex).Name.ToUpper() = ComponentMetaData.CustomPropertyCollection("InputColumnToCheck").Value.ToString().ToUpper()) Then

                    colId = input.InputColumnCollection(colIndex).LineageID
                    Exit For
                End If
            Next

            Dim theColIndex As Integer = BufferManager.FindColumnByLineageID(input.Buffer, colId)

            If (Not buffer.EndOfRowset) Then
                While buffer.NextRow()
                    ' perform the spellcheck for the specified column
                    checkValue = buffer(theColIndex).ToString()
                    suggestedValue = spellEngine.Execute(checkValue)

                    outputBuffer.AddRow()

                    Dim x As Integer
                    For x = 0 To inputColumnBufferIndexes.Length - 1 Step x + 1
                        outputBuffer(outputColumnBufferIndexes(x)) = buffer(inputColumnBufferIndexes(x))
                    Next

                    If (suggestedValue.Length = 0) Then
                        outputBuffer(outputColumnBufferIndexes(outputColumnBufferIndexes.Length - 2)) = "false"
                    Else
                        outputBuffer(outputColumnBufferIndexes(outputColumnBufferIndexes.Length - 2)) = "true"

                        outputBuffer(outputColumnBufferIndexes(outputColumnBufferIndexes.Length - 1)) = suggestedValue
                    End If
                End While
            Else
                outputBuffer.SetEndOfRowset()
            End If
        End Sub
    End Class
End Namespace
