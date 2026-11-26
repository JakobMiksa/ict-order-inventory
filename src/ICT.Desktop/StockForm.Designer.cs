namespace ICT.Desktop;

partial class StockForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        productsGrid = new DataGridView();
        amountLabel = new Label();
        amountUpDown = new NumericUpDown();
        addStockButton = new Button();
        messageLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)productsGrid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)amountUpDown).BeginInit();
        SuspendLayout();
        productsGrid.AllowUserToAddRows = false;
        productsGrid.AllowUserToDeleteRows = false;
        productsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        productsGrid.Location = new Point(12, 12);
        productsGrid.MultiSelect = false;
        productsGrid.Name = "productsGrid";
        productsGrid.ReadOnly = true;
        productsGrid.RowHeadersVisible = false;
        productsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        productsGrid.Size = new Size(460, 250);
        productsGrid.TabIndex = 0;
        amountLabel.AutoSize = true;
        amountLabel.Location = new Point(12, 280);
        amountLabel.Name = "amountLabel";
        amountLabel.Size = new Size(110, 15);
        amountLabel.TabIndex = 1;
        amountLabel.Text = "Antal kommet ind:";
        amountUpDown.Location = new Point(130, 277);
        amountUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        amountUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        amountUpDown.Name = "amountUpDown";
        amountUpDown.Size = new Size(80, 23);
        amountUpDown.TabIndex = 2;
        amountUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
        addStockButton.Location = new Point(230, 274);
        addStockButton.Name = "addStockButton";
        addStockButton.Size = new Size(120, 29);
        addStockButton.TabIndex = 3;
        addStockButton.Text = "Læg på lager";
        addStockButton.UseVisualStyleBackColor = true;
        addStockButton.Click += addStockButton_Click;
        messageLabel.AutoSize = true;
        messageLabel.Location = new Point(12, 316);
        messageLabel.Name = "messageLabel";
        messageLabel.Size = new Size(0, 15);
        messageLabel.TabIndex = 4;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(484, 345);
        Controls.Add(messageLabel);
        Controls.Add(addStockButton);
        Controls.Add(amountUpDown);
        Controls.Add(amountLabel);
        Controls.Add(productsGrid);
        Name = "StockForm";
        Text = "Læg varer på lager";
        Load += StockForm_Load;
        ((System.ComponentModel.ISupportInitialize)productsGrid).EndInit();
        ((System.ComponentModel.ISupportInitialize)amountUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private DataGridView productsGrid;
    private Label amountLabel;
    private NumericUpDown amountUpDown;
    private Button addStockButton;
    private Label messageLabel;
}
