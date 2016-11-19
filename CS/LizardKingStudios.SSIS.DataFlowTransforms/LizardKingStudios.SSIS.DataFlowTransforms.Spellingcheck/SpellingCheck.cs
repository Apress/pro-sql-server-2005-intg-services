using System;
using Microsoft.SqlServer.Dts;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using LizardKingStudios.Services.SpellCheck;

namespace LizardKingStudios.SSIS.DataFlowTransforms
{
    [DtsPipelineComponent(DisplayName = "LizardKing Spellchecker", Description = "Sample Data Flow Component from the Apress book Pro SSIS", IconResource = "LizardKingStudios.SSIS.DataFlowTranforms.Icon.ico", CurrentVersion = 1, ComponentType = ComponentType.Transform)]
    public class SpellingCheck : PipelineComponent
    {
        PipelineBuffer outputBuffer;
        private int[] inputColumnBufferIndexes;
        private int[] outputColumnBufferIndexes;

        private void PostError(string message)
        {
            bool cancel = false;
            this.ComponentMetaData.FireError(0, this.ComponentMetaData.Name, message, "", 0, out cancel);
        }

        public override DTSValidationStatus Validate()
        {
            DTSValidationStatus status = base.Validate();
            if (status == DTSValidationStatus.VS_ISCORRUPT)
            {
                return status;
            }

            IDTSComponentMetaData90 metadata = this.ComponentMetaData;
            IDTSCustomPropertyCollection90 componentCustomProperties = metadata.CustomPropertyCollection;
            try
            {
                IDTSCustomProperty90 customProperty = componentCustomProperties["InputColumnToCheck"];
                string s = (string)customProperty.Value;
                if (s.Length == 0)
                {
                    PostError("InputColumnToCheck must be populated");
                    return DTSValidationStatus.VS_ISCORRUPT;
                }
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                if (e.ErrorCode == HResults.DTS_E_ELEMENTNOTFOUND)
                {
                    PostError("Custom property 'InputColumnToCheck' not found in component custom property collection");
                    return DTSValidationStatus.VS_ISCORRUPT;
                }
                else
                {
                    throw e;
                }
            }
            if (metadata.InputCollection.Count != 1)
            {
                PostError("Component requires exactly one input.");
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            IDTSInput90 input = metadata.InputCollection[0];
            IDTSInputColumnCollection90 inputColumns = input.InputColumnCollection;
                     
            for (int j = 0; j < inputColumns.Count; j++)
            {
                IDTSInputColumn90 column = inputColumns[j];
                if (column.IsValid)
                {
                    // validation code can go in here for allowing only string types
                }
            }
            return status;
        }

        public override IDTSInput90 InsertInput(DTSInsertPlacement insertPlacement, int inputID)
        {
            PostError("Component requires exactly one input. New input is forbidden.");
            throw new PipelineComponentHResultException(HResults.DTS_E_CANTADDINPUT);
        }

        public override void DeleteInput(int inputID)
        {
            PostError("Component requires exactly one input. Deleted input is forbidden.");
            throw new PipelineComponentHResultException(HResults.DTS_E_CANTDELETEINPUT);
        }

        public override IDTSOutput90 InsertOutput(DTSInsertPlacement insertPlacement, int outputID)
        {

            PostError("Component requires exactly one output. New output is forbidden.");
            throw new PipelineComponentHResultException(HResults.DTS_E_CANTADDOUTPUT);
        }

        public override void DeleteOutput(int outputID)
        {
            PostError("Component requires exactly one output. Deleted output is forbidden.");
            throw new PipelineComponentHResultException(HResults.DTS_E_CANTDELETEOUTPUT);
        }

        public override IDTSOutputColumn90 InsertOutputColumnAt(int outputID, int outputColumnIndex, string name, string description)
        {
            PostError("Component does not allow addition of output columns.");
            throw new PipelineComponentHResultException(HResults.DTS_E_CANTADDCOLUMN);
        }

        public override IDTSInputColumn90 SetUsageType(int inputID, IDTSVirtualInput90 virtualInput, int lineageID, DTSUsageType usageType)
        {
            IDTSInputColumn90 inputColumn = null;
            switch (usageType)
            {
                case DTSUsageType.UT_READWRITE:

                case DTSUsageType.UT_READONLY:

                case DTSUsageType.UT_IGNORED:
                    inputColumn = base.SetUsageType(inputID, virtualInput, lineageID, usageType);
                    return inputColumn;
                default:
                    throw new PipelineComponentHResultException(HResults.DTS_E_CANTSETUSAGETYPE);
            }
        }

        public override void OnInputPathAttached(int inputID)
        {
            base.OnInputPathAttached(inputID);

            IDTSInput90 input = ComponentMetaData.InputCollection.GetObjectByID(inputID);
            IDTSOutput90 output = ComponentMetaData.OutputCollection[0];
            IDTSVirtualInput90 vInput = input.GetVirtualInput();

            foreach (IDTSVirtualInputColumn90 vCol in vInput.VirtualInputColumnCollection)
            {
                IDTSOutputColumn90 outCol = output.OutputColumnCollection.New();
                outCol.Name = vCol.Name;
                outCol.SetDataTypeProperties(vCol.DataType, vCol.Length, vCol.Precision, vCol.Scale, vCol.CodePage);
            }
            
            IDTSOutputColumn90 outputColumn = output.OutputColumnCollection.New();

            outputColumn.Name = "isSuggested";
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            outputColumn = this.ComponentMetaData.OutputCollection[0].OutputColumnCollection.New();
            outputColumn.Name = "suggestedValue";
            outputColumn.SetDataTypeProperties(DataType.DT_STR, 50, 0, 0, 1252);

        }

        public override void ProvideComponentProperties()
        {
            base.ProvideComponentProperties();

            ComponentMetaData.OutputCollection[0].SynchronousInputID = 0;

            IDTSCustomPropertyCollection90 customProperties = this.ComponentMetaData.CustomPropertyCollection;
            IDTSCustomProperty90 customProperty = customProperties.New();

            customProperty.Name = "InputColumnToCheck"; 
            customProperty.Value = "";
            customProperty = customProperties.New();
            customProperty.Name = "MaxSuggestionsCount";
            customProperty.Value = "5";

        }

        public override void ReinitializeMetaData()
        {
            base.ReinitializeMetaData();
            
        }

        public override IDTSCustomProperty90 SetComponentProperty(string propertyName, object propertyValue)
        {
            if (propertyName == "InputColumnToCheck")
            {
                string value = (string)propertyValue;
                if (value.Length == 0)
                {
                    PostError("InputColumnToCheck must be populated.");
                    throw new PipelineComponentHResultException(HResults.DTS_E_FAILEDTOSETPROPERTY);
                }
                else
                {
                    return base.SetComponentProperty(propertyName, propertyValue);
                }
            }
            else
            {
                PostError("Specified property name [" + propertyName + "] not expected.");
                throw new PipelineComponentHResultException(HResults.DTS_E_FAILEDTOSETPROPERTY);
            }
        }

        public override void PreExecute()
        {
             IDTSInput90 input = ComponentMetaData.InputCollection[0];
             IDTSOutput90 output = ComponentMetaData.OutputCollection[0];

             inputColumnBufferIndexes = new int[input.InputColumnCollection.Count];
             outputColumnBufferIndexes = new int[output.OutputColumnCollection.Count];

             for (int col = 0; col < input.InputColumnCollection.Count; col++)
                inputColumnBufferIndexes[col] = BufferManager.FindColumnByLineageID(input.Buffer, input.InputColumnCollection[col].LineageID);

             for (int col = 0; col < output.OutputColumnCollection.Count; col++)
                outputColumnBufferIndexes[col] = BufferManager.FindColumnByLineageID(output.Buffer, output.OutputColumnCollection[col].LineageID);
        }

        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            if (buffers.Length != 0)
                outputBuffer = buffers[0];
        }

        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            string suggestedValue = String.Empty;
            string checkValue;
            SpellcheckEngine spellEngine = new SpellcheckEngine();
            int colId = -1;

            spellEngine.MaxSuggestionsCount = Convert.ToInt32( ComponentMetaData.CustomPropertyCollection["MaxSuggestionsCount"].Value.ToString() );
            IDTSInput90 input = ComponentMetaData.InputCollection.GetObjectByID(inputID);
          
            for (int colIndex = 0; colIndex < input.InputColumnCollection.Count; colIndex++ )
            {
                if (input.InputColumnCollection[colIndex].Name.ToUpper() == ComponentMetaData.CustomPropertyCollection["InputColumnToCheck"].Value.ToString().ToUpper())
                {
                    
                    colId = input.InputColumnCollection[colIndex].LineageID;
                    break;
                }
            }

            int theColIndex = BufferManager.FindColumnByLineageID(input.Buffer, colId);

            if (!buffer.EndOfRowset)
            {
                while (buffer.NextRow())
                {
                    // perform the spellcheck for the specified column
                    checkValue = buffer[theColIndex].ToString();
                    suggestedValue = spellEngine.Execute(checkValue);

                    outputBuffer.AddRow();

                    for (int x = 0; x < inputColumnBufferIndexes.Length; x++)
                    {
                        outputBuffer[outputColumnBufferIndexes[x]] = buffer[inputColumnBufferIndexes[x]];
                    }
                    
                    if (suggestedValue.Length == 0)
                        outputBuffer[outputColumnBufferIndexes[outputColumnBufferIndexes.Length - 2]] = "false";
                    else
                        outputBuffer[outputColumnBufferIndexes[outputColumnBufferIndexes.Length - 2]] = "true";
                    
                    outputBuffer[outputColumnBufferIndexes[outputColumnBufferIndexes.Length-1]] = suggestedValue;

                }
            }
            else
            {
                outputBuffer.SetEndOfRowset();
            }
        }
    }               
}
