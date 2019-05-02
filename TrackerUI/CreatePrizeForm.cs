using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        IPrizeRequester callingForm;

        string validationErrorMessage = "";

        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();

            callingForm = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(placeNameValue.Text, placeNumberValue.Text, prizeAmountValue.Text,
                    prizePercentageValue.Text);

                GlobalConfig.Connection.CreatePrize(model);

                callingForm.PrizeComplete(model);

                this.Close();
            }

            else
            {
                MessageBox.Show(validationErrorMessage);
            }
        }

        private bool ValidateForm()
        {
            bool output = true;
            validationErrorMessage = "";

            int placeNumber = 0;
            bool placeNumberValidNumber = int.TryParse(placeNumberValue.Text, out placeNumber);

            if (!placeNumberValidNumber)
            {
                output = false;
                validationErrorMessage = "Please enter 1 or 2 for place number";
                return output;
            }

            if (placeNumber < 1 || placeNumber > 2)
            {
                output = false;
                validationErrorMessage = "Please enter 1 or 2 for place number";
                return output;
            }

            if (placeNameValue.Text.Length == 0)
            {
                output = false;
                validationErrorMessage = "Please enter a valid place name";
                return output;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (!prizeAmountValid || prizeAmount < 0)
            {
                output = false;
                validationErrorMessage = "Please a valid prize amount";
                return output;
            }

            if (!prizePercentageValid || prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
                validationErrorMessage = "Please a valid percentage amount";
                return output;
            }

            if (prizeAmountValid && prizePercentageValid)
            {
                if (prizeAmount != 0 && prizePercentage != 0)
                {
                    output = false;
                    validationErrorMessage = "Please choose either the amount or percentage method, not both.";
                    return output;
                }
            }

            return output;
        }
    }
}
