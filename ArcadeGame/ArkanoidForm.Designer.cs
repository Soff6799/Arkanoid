namespace ArcadeGame;

partial class ArkanoidForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ButtonStartAgain = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // ButtonStartAgain
        // 
        ButtonStartAgain.BackColor = System.Drawing.SystemColors.ButtonShadow;
        ButtonStartAgain.Location = new System.Drawing.Point(833, 552);
        ButtonStartAgain.Name = "ButtonStartAgain";
        ButtonStartAgain.Size = new System.Drawing.Size(192, 69);
        ButtonStartAgain.TabIndex = 0;
        ButtonStartAgain.Text = "Start again";
        ButtonStartAgain.UseVisualStyleBackColor = false;
        ButtonStartAgain.Click += ButtonStartAgain_Click;
        // 
        // Form
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.ButtonFace;
        ClientSize = new System.Drawing.Size(1049, 859);
        Controls.Add(ButtonStartAgain);
        Text = "ArcadeGame";
        Load += Form_Load;
        MouseClick += Form_MouseClick;
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button ButtonStartAgain;

    #endregion
}