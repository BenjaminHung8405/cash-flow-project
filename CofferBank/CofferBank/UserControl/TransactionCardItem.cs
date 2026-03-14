using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CofferBank
{
    public partial class TransactionCardItem : UserControl
    {
        public TransactionCardItem()
        {
            InitializeComponent();
        }

        public void SetTransactionData(string title, string type, string date, string currency, Color currencyColor, FontAwesome.Sharp.IconChar icon)
        {
            lbTitle.Text = title;
            lbType.Text = type;
            lbDate.Text = date;
            lbCurrency.Text = currency;
            lbCurrency.ForeColor = currencyColor;
            iconPictureBox.IconChar = icon;
        }
    }
}
