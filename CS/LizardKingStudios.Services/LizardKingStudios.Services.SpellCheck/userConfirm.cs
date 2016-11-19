using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace LizardKingStudios.Services.SpellCheck
{
    public partial class userConfirm : Form
    {
        private SpellingSuggestions _suggestionsList = null;
        private string _selectedValue = String.Empty;

        public userConfirm()
        {
            InitializeComponent();
        }

        public SpellingSuggestions suggestionsList
        {
            set
            {
                _suggestionsList = value;

                loadSuggestionsList();
            }
        }

        public string selectedValue
        {
            get
            {
                return _selectedValue;
            }
        }

        private void loadSuggestionsList()
        {
            if (_suggestionsList!=null)
            {
                foreach (SpellingSuggestion theSuggestion in _suggestionsList)
                {
                    Suggestions.Items.Add(theSuggestion.Name);
                }

                Suggestions.SelectedIndex = 0;
            }
        }

        private void selectSuggestion_Click(object sender, EventArgs e)
        {
            if (Suggestions.SelectedIndex==-1)
            {
                MessageBox.Show("You must select a suggestion first!");
            }
            else
            {
                _selectedValue = Suggestions.SelectedItem.ToString();

                MessageBox.Show("selected: " + _selectedValue);
                this.Close();
            }
        }

        private void cancelForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}