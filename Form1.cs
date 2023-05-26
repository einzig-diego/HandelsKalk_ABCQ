using System;
using System.Windows.Forms;

namespace AngebotVergleichApp
{
    public partial class Form1 : Form
    {
        private Label lblGrossPrice;
        private TextBox txtGrossPrice;
        private Label lblNetPrice;
        private TextBox txtNetPrice;
        private Label lblVatRate;
        private TextBox txtVatRate;
        private Label lblDiscount;
        private TextBox txtDiscount;
        private Label lblCashDiscount;
        private TextBox txtCashDiscount;
        private Label lblPurchaseCost;
        private TextBox txtPurchaseCost;
        private Button btnCalculate;
        private Label lblResult;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateFormElements();
        }

        private void CreateFormElements()
        {
            // Label "Bruttolisteneinkaufspreis"
            lblGrossPrice = new Label();
            lblGrossPrice.Text = "Bruttolisteneinkaufspreis:";
            lblGrossPrice.AutoSize = true;
            lblGrossPrice.Location = new System.Drawing.Point(20, 20);
            this.Controls.Add(lblGrossPrice);

            // TextBox "Bruttolisteneinkaufspreis"
            txtGrossPrice = new TextBox();
            txtGrossPrice.Location = new System.Drawing.Point(200, 20);
            this.Controls.Add(txtGrossPrice);

            // Label "Nettolisteneinkaufspreis"
            lblNetPrice = new Label();
            lblNetPrice.Text = "Nettolisteneinkaufspreis:";
            lblNetPrice.AutoSize = true;
            lblNetPrice.Location = new System.Drawing.Point(20, 60);
            this.Controls.Add(lblNetPrice);

            // TextBox "Nettolisteneinkaufspreis"
            txtNetPrice = new TextBox();
            txtNetPrice.Location = new System.Drawing.Point(200, 60);
            this.Controls.Add(txtNetPrice);

            // Label "Mehrwertsteuer"
            lblVatRate = new Label();
            lblVatRate.Text = "Mehrwertsteuer (%):";
            lblVatRate.AutoSize = true;
            lblVatRate.Location = new System.Drawing.Point(20, 100);
            this.Controls.Add(lblVatRate);

            // TextBox "Mehrwertsteuer"
            txtVatRate = new TextBox();
            txtVatRate.Location = new System.Drawing.Point(200, 100);
            this.Controls.Add(txtVatRate);

            // Label "Rabatt"
            lblDiscount = new Label();
            lblDiscount.Text = "Rabatt (%):";
            lblDiscount.AutoSize = true;
            lblDiscount.Location = new System.Drawing.Point(20, 140);
            this.Controls.Add(lblDiscount);

            // TextBox "Rabatt"
            txtDiscount = new TextBox();
            txtDiscount.Location = new System.Drawing.Point(200, 140);
            this.Controls.Add(txtDiscount);

            // Label "Skonto"
            lblCashDiscount = new Label();
            lblCashDiscount.Text = "Skonto (%):";
            lblCashDiscount.AutoSize = true;
            lblCashDiscount.Location = new System.Drawing.Point(20, 180);
            this.Controls.Add(lblCashDiscount);

            // TextBox "Skonto"
            txtCashDiscount = new TextBox();
            txtCashDiscount.Location = new System.Drawing.Point(200, 180);
            this.Controls.Add(txtCashDiscount);

            // Label "Bezugskosten"
            lblPurchaseCost = new Label();
            lblPurchaseCost.Text = "Bezugskosten:";
            lblPurchaseCost.AutoSize = true;
            lblPurchaseCost.Location = new System.Drawing.Point(20, 220);
            this.Controls.Add(lblPurchaseCost);

            // TextBox "Bezugskosten"
            txtPurchaseCost = new TextBox();
            txtPurchaseCost.Location = new System.Drawing.Point(200, 220);
            this.Controls.Add(txtPurchaseCost);

            // Button "Berechnen"
            btnCalculate = new Button();
            btnCalculate.Text = "Berechnen";
            btnCalculate.Location = new System.Drawing.Point(20, 260);
            btnCalculate.Click += BtnCalculate_Click;
            this.Controls.Add(btnCalculate);

            // Label "Ergebnis"
            lblResult = new Label();
            lblResult.AutoSize = true;
            lblResult.Location = new System.Drawing.Point(20, 300);
            this.Controls.Add(lblResult);
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            double grossPrice, netPrice, vatRate, discount, cashDiscountRate, purchaseCost;
            double targetPrice, cashDiscount, purchasePrice;

            if (double.TryParse(txtGrossPrice.Text, out grossPrice) && double.TryParse(txtVatRate.Text, out vatRate))
            {
                if (double.TryParse(txtNetPrice.Text, out netPrice))
                {
                    // Falls das Feld für den Nettolisteneinkaufspreis nicht leer ist,
                    // wird der Bruttolisteneinkaufspreis ignoriert
                    netPrice = netPrice != 0 ? netPrice : CalculateNetPrice(grossPrice, vatRate);
                }
                else
                {
                    if (double.TryParse(txtVatRate.Text, out vatRate))
                    {
                        netPrice = CalculateNetPrice(grossPrice, vatRate);
                    }
                    else
                    {
                        lblResult.Text = "Ungültige Eingabe für den Mehrwertsteuersatz";
                        return;
                    }
                }

                if (double.TryParse(txtVatRate.Text, out vatRate) &&
                    double.TryParse(txtDiscount.Text, out discount) &&
                    double.TryParse(txtCashDiscount.Text, out cashDiscountRate) &&
                    double.TryParse(txtPurchaseCost.Text, out purchaseCost))
                {
                    targetPrice = CalculateTargetPrice(netPrice, discount);
                    cashDiscount = CalculateCashDiscount(targetPrice, cashDiscountRate);
                    purchasePrice = CalculatePurchasePrice(targetPrice, cashDiscount, purchaseCost);

                    lblResult.Text = "Rechenweg:\n";
                    lblResult.Text += $"Bruttolisteneinkaufspreis: {grossPrice.ToString("C2")}\n";
                    lblResult.Text += $"Nettolisteneinkaufspreis: {netPrice.ToString("C2")}\n";
                    lblResult.Text += $"Zieleinkaufspreis: {targetPrice.ToString("C2")}\n";
                    lblResult.Text += $"Bareinkaufspreis: {cashDiscount.ToString("C2")}\n";
                    lblResult.Text += $"Einstandspreis / Bezugspreis: {purchasePrice.ToString("C2")}";
                    lblResult.Text += "\n\nBerechnungsschritte:\n";
                    lblResult.Text += $"Nettolisteneinkaufspreis = Bruttolisteneinkaufspreis / (1 + (Mehrwertsteuer / 100))\n";
                    lblResult.Text += $"Zieleinkaufspreis = Nettolisteneinkaufspreis * (1 - (Rabatt / 100))\n";
                    lblResult.Text += $"Bareinkaufspreis = Zieleinkaufspreis * (Skonto / 100)\n";
                    lblResult.Text += $"Einstandspreis / Bezugspreis = Zieleinkaufspreis - Bareinkaufspreis + Bezugskosten";
                }
                else
                {
                    lblResult.Text = "Ungültige Eingabe";
                }
            }
            else
            {
                lblResult.Text = "Ungültige Eingabe für den Bruttolisteneinkaufspreis";
            }
        }


        private double CalculateNetPrice(double grossPrice, double vatRate)
        {
            return grossPrice / (1 + (vatRate / 100));
        }

        private double CalculateTargetPrice(double netPrice, double discount)
        {
            return netPrice * (1 - (discount / 100));
        }

        private double CalculateCashDiscount(double targetPrice, double cashDiscountRate)
        {
            return targetPrice * (cashDiscountRate / 100);
        }

        private double CalculatePurchasePrice(double targetPrice, double cashDiscount, double purchaseCost)
        {
            return targetPrice - cashDiscount + purchaseCost;
        }
    }
}
