﻿using AntShares.Core;
using AntShares.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    public partial class ClaimForm : Form
    {
        public ClaimForm()
        {
            InitializeComponent();
        }

        private void ClaimForm_Load(object sender, EventArgs e)
        {
            Fixed8 amount = Wallet.CalculateClaimAmount(Program.CurrentWallet.GetUnclaimedCoins().Select(p => p.Input));
            textBox1.Text = amount.ToString();
            if (amount == Fixed8.Zero) button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransactionInput[] claims = Program.CurrentWallet.GetUnclaimedCoins().Select(p => p.Input).ToArray();
            Helper.SignAndShowInformation(new ClaimTransaction
            {
                Claims = claims,
                Attributes = new TransactionAttribute[0],
                Inputs = new TransactionInput[0],
                Outputs = new[]
                {
                    new TransactionOutput
                    {
                        AssetId = Blockchain.AntCoin.Hash,
                        Value = Wallet.CalculateClaimAmount(claims),
                        ScriptHash = Program.CurrentWallet.GetChangeAddress()
                    }
                }
            });
        }
    }
}
