using System;
using System.Reflection;
using Microsoft.Office.Interop.Word;

namespace LizardKingStudios.Services.SpellCheck
{
    public class SpellcheckEngine
    {
        private readonly Application _wordSpellChecker = new Application();
        private int _maxSuggestionsCount = 5;

        public SpellcheckEngine()
        {
            object optional = Missing.Value;

            _wordSpellChecker.Application.Documents.Add(ref optional, ref optional, ref optional, ref optional);
        }

        public int MaxSuggestionsCount
        {
            get
            {
                return _maxSuggestionsCount;
            }
            set
            {
                _maxSuggestionsCount = value;
                
            }
        }
        
        public string Execute(string checkValue)
        {
            string suggestedValue = String.Empty;

            if (checkValue.Length!=0)
            {
                object optional = Missing.Value;
                object ignoreUppercase = true;
                object suggestionMode = Missing.Value;

                SpellingSuggestions theSuggestions = _wordSpellChecker.GetSpellingSuggestions(checkValue, ref optional,
                    ref optional, ref optional, ref optional, 
                    ref optional, ref optional, ref optional, ref optional, 
                    ref optional, ref optional, ref optional, ref optional, 
                    ref optional);

                // if we get more than (parameter specified) suggestions, don't make
                // the change.
                if (theSuggestions.Count > 0 && theSuggestions.Count <= _maxSuggestionsCount)
                {
                    try
                    {
                        if (theSuggestions.Count > 1)
                        {
                            userConfirm theUserConfirmWindow = new userConfirm();
                            theUserConfirmWindow.suggestionsList = theSuggestions;
                            theUserConfirmWindow.ShowDialog();
                            suggestedValue = theUserConfirmWindow.selectedValue; 
                        }
                        else
                        {
                            SpellingSuggestion theSuggestion = theSuggestions[1];
                            suggestedValue = theSuggestion.Name;
                        }

                        
                    }
                    catch
                    {
                        // lets swallow it - implement error handling here!
                    }
                }
            }

            return suggestedValue;

        }
    }
}
