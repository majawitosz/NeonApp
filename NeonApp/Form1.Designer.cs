
namespace NeonApp
{
    partial class Form1
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            flowLayoutPanel1 = new FlowLayoutPanel();
            chooseImage = new Button();
            imagePathTextBox = new TextBox();
            chosenImage = new PictureBox();
            trackBarThreads = new TrackBar();
            threadLabel = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            timeLabel = new Label();
            pictureBoxNeon = new PictureBox();
            pictureBoxOriginal = new PictureBox();
            label2 = new Label();
            labelOriginal = new Label();
            convert = new Button();
            t1Asm = new Label();
            t2Asm = new Label();
            t4Asm = new Label();
            t8Asm = new Label();
            t16Asm = new Label();
            t32Asm = new Label();
            t64Asm = new Label();
            TestsButton = new Button();
            restoreDefault = new Button();
            cSharp_radioBtn = new RadioButton();
            asm_radioBtn = new RadioButton();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chosenImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarThreads).BeginInit();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNeon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOriginal).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(chooseImage);
            flowLayoutPanel1.Controls.Add(imagePathTextBox);
            flowLayoutPanel1.Controls.Add(chosenImage);
            flowLayoutPanel1.Location = new Point(14, 481);
            flowLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(233, 256);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // chooseImage
            // 
            chooseImage.Location = new Point(4, 3);
            chooseImage.Margin = new Padding(4, 3, 4, 3);
            chooseImage.Name = "chooseImage";
            chooseImage.Size = new Size(230, 27);
            chooseImage.TabIndex = 3;
            chooseImage.Text = "Choose Image";
            chooseImage.UseVisualStyleBackColor = true;
            chooseImage.Click += chooseImage_Click;
            // 
            // imagePathTextBox
            // 
            imagePathTextBox.Location = new Point(4, 36);
            imagePathTextBox.Margin = new Padding(4, 3, 4, 3);
            imagePathTextBox.Name = "imagePathTextBox";
            imagePathTextBox.Size = new Size(229, 23);
            imagePathTextBox.TabIndex = 2;
            // 
            // chosenImage
            // 
            chosenImage.Location = new Point(4, 65);
            chosenImage.Margin = new Padding(4, 3, 4, 3);
            chosenImage.Name = "chosenImage";
            chosenImage.Size = new Size(230, 179);
            chosenImage.SizeMode = PictureBoxSizeMode.Zoom;
            chosenImage.TabIndex = 1;
            chosenImage.TabStop = false;
            // 
            // trackBarThreads
            // 
            trackBarThreads.Location = new Point(4, 18);
            trackBarThreads.Margin = new Padding(4, 3, 4, 3);
            trackBarThreads.Maximum = 0;
            trackBarThreads.Name = "trackBarThreads";
            trackBarThreads.Size = new Size(331, 45);
            trackBarThreads.TabIndex = 1;
            trackBarThreads.Scroll += trackBarThreads_Scroll;
            // 
            // threadLabel
            // 
            threadLabel.Location = new Point(4, 0);
            threadLabel.Margin = new Padding(4, 0, 4, 0);
            threadLabel.Name = "threadLabel";
            threadLabel.Size = new Size(331, 15);
            threadLabel.TabIndex = 2;
            threadLabel.Text = "Number of Threads";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(threadLabel);
            flowLayoutPanel2.Controls.Add(trackBarThreads);
            flowLayoutPanel2.Controls.Add(timeLabel);
            flowLayoutPanel2.Location = new Point(550, 471);
            flowLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(335, 141);
            flowLayoutPanel2.TabIndex = 3;
            // 
            // timeLabel
            // 
            timeLabel.AutoSize = true;
            timeLabel.Location = new Point(4, 66);
            timeLabel.Margin = new Padding(4, 0, 4, 0);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(36, 15);
            timeLabel.TabIndex = 3;
            timeLabel.Text = "Time:";
            // 
            // pictureBoxNeon
            // 
            pictureBoxNeon.Location = new Point(474, 55);
            pictureBoxNeon.Margin = new Padding(4, 3, 4, 3);
            pictureBoxNeon.Name = "pictureBoxNeon";
            pictureBoxNeon.Size = new Size(411, 354);
            pictureBoxNeon.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxNeon.TabIndex = 4;
            pictureBoxNeon.TabStop = false;
            // 
            // pictureBoxOriginal
            // 
            pictureBoxOriginal.Location = new Point(31, 55);
            pictureBoxOriginal.Margin = new Padding(4, 3, 4, 3);
            pictureBoxOriginal.Name = "pictureBoxOriginal";
            pictureBoxOriginal.Size = new Size(402, 354);
            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOriginal.TabIndex = 5;
            pictureBoxOriginal.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Impact", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(601, 10);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(157, 36);
            label2.TabIndex = 6;
            label2.Text = "NEON EFFECT";
            // 
            // labelOriginal
            // 
            labelOriginal.AutoSize = true;
            labelOriginal.BackColor = SystemColors.ButtonHighlight;
            labelOriginal.Font = new Font("Impact", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelOriginal.Location = new Point(152, 10);
            labelOriginal.Margin = new Padding(4, 0, 4, 0);
            labelOriginal.Name = "labelOriginal";
            labelOriginal.Size = new Size(120, 36);
            labelOriginal.TabIndex = 7;
            labelOriginal.Text = "ORIGINAL";
            // 
            // convert
            // 
            convert.Location = new Point(394, 500);
            convert.Margin = new Padding(4, 3, 4, 3);
            convert.Name = "convert";
            convert.Size = new Size(88, 27);
            convert.TabIndex = 8;
            convert.Text = "Convert";
            convert.UseVisualStyleBackColor = true;
            convert.Click += Convert_Click;
            // 
            // t1Asm
            // 
            t1Asm.AutoSize = true;
            t1Asm.Location = new Point(391, 584);
            t1Asm.Margin = new Padding(4, 0, 4, 0);
            t1Asm.Name = "t1Asm";
            t1Asm.Size = new Size(38, 15);
            t1Asm.TabIndex = 9;
            t1Asm.Text = "label1";
            // 
            // t2Asm
            // 
            t2Asm.AutoSize = true;
            t2Asm.Location = new Point(393, 599);
            t2Asm.Margin = new Padding(4, 0, 4, 0);
            t2Asm.Name = "t2Asm";
            t2Asm.Size = new Size(38, 15);
            t2Asm.TabIndex = 10;
            t2Asm.Text = "label1";
            // 
            // t4Asm
            // 
            t4Asm.AutoSize = true;
            t4Asm.Location = new Point(393, 614);
            t4Asm.Margin = new Padding(4, 0, 4, 0);
            t4Asm.Name = "t4Asm";
            t4Asm.Size = new Size(38, 15);
            t4Asm.TabIndex = 11;
            t4Asm.Text = "label1";
            // 
            // t8Asm
            // 
            t8Asm.AutoSize = true;
            t8Asm.Location = new Point(393, 629);
            t8Asm.Margin = new Padding(4, 0, 4, 0);
            t8Asm.Name = "t8Asm";
            t8Asm.Size = new Size(38, 15);
            t8Asm.TabIndex = 12;
            t8Asm.Text = "label1";
            // 
            // t16Asm
            // 
            t16Asm.AutoSize = true;
            t16Asm.Location = new Point(391, 644);
            t16Asm.Margin = new Padding(4, 0, 4, 0);
            t16Asm.Name = "t16Asm";
            t16Asm.Size = new Size(38, 15);
            t16Asm.TabIndex = 13;
            t16Asm.Text = "label1";
            // 
            // t32Asm
            // 
            t32Asm.AutoSize = true;
            t32Asm.Location = new Point(393, 659);
            t32Asm.Margin = new Padding(4, 0, 4, 0);
            t32Asm.Name = "t32Asm";
            t32Asm.Size = new Size(38, 15);
            t32Asm.TabIndex = 14;
            t32Asm.Text = "label1";
            // 
            // t64Asm
            // 
            t64Asm.AutoSize = true;
            t64Asm.Location = new Point(393, 674);
            t64Asm.Margin = new Padding(4, 0, 4, 0);
            t64Asm.Name = "t64Asm";
            t64Asm.Size = new Size(38, 15);
            t64Asm.TabIndex = 15;
            t64Asm.Text = "label1";
            // 
            // TestsButton
            // 
            TestsButton.Location = new Point(397, 554);
            TestsButton.Margin = new Padding(4, 3, 4, 3);
            TestsButton.Name = "TestsButton";
            TestsButton.Size = new Size(88, 27);
            TestsButton.TabIndex = 16;
            TestsButton.Text = "Run tests";
            TestsButton.UseVisualStyleBackColor = true;
            TestsButton.Click += TestsButton_Click;
            // 
            // restoreDefault
            // 
            restoreDefault.Location = new Point(550, 437);
            restoreDefault.Margin = new Padding(4, 3, 4, 3);
            restoreDefault.Name = "restoreDefault";
            restoreDefault.Size = new Size(145, 27);
            restoreDefault.TabIndex = 17;
            restoreDefault.Text = "Restore Default";
            restoreDefault.UseVisualStyleBackColor = true;
            restoreDefault.Click += restoreDefault_Click;
            // 
            // cSharp_radioBtn
            // 
            cSharp_radioBtn.AutoSize = true;
            cSharp_radioBtn.Location = new Point(622, 642);
            cSharp_radioBtn.Margin = new Padding(4, 3, 4, 3);
            cSharp_radioBtn.Name = "cSharp_radioBtn";
            cSharp_radioBtn.Size = new Size(79, 19);
            cSharp_radioBtn.TabIndex = 19;
            cSharp_radioBtn.TabStop = true;
            cSharp_radioBtn.Text = "C# Library";
            cSharp_radioBtn.UseVisualStyleBackColor = true;
            cSharp_radioBtn.CheckedChanged += cSharp_radioBtn_CheckedChanged;
            // 
            // asm_radioBtn
            // 
            asm_radioBtn.AutoSize = true;
            asm_radioBtn.Location = new Point(622, 674);
            asm_radioBtn.Margin = new Padding(4, 3, 4, 3);
            asm_radioBtn.Name = "asm_radioBtn";
            asm_radioBtn.Size = new Size(110, 19);
            asm_radioBtn.TabIndex = 20;
            asm_radioBtn.TabStop = true;
            asm_radioBtn.Text = "ASM x64 Library";
            asm_radioBtn.UseVisualStyleBackColor = true;
            asm_radioBtn.CheckedChanged += asm_radioBtn_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(915, 763);
            Controls.Add(asm_radioBtn);
            Controls.Add(cSharp_radioBtn);
            Controls.Add(restoreDefault);
            Controls.Add(TestsButton);
            Controls.Add(t64Asm);
            Controls.Add(t32Asm);
            Controls.Add(t16Asm);
            Controls.Add(t8Asm);
            Controls.Add(t4Asm);
            Controls.Add(t2Asm);
            Controls.Add(t1Asm);
            Controls.Add(convert);
            Controls.Add(labelOriginal);
            Controls.Add(label2);
            Controls.Add(pictureBoxOriginal);
            Controls.Add(pictureBoxNeon);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(flowLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "NeonApp";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chosenImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarThreads).EndInit();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNeon).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOriginal).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox imagePathTextBox;
        private System.Windows.Forms.PictureBox chosenImage;
        private System.Windows.Forms.Button chooseImage;
        private System.Windows.Forms.TrackBar trackBarThreads;
        private System.Windows.Forms.Label threadLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.PictureBox pictureBoxNeon;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelOriginal;
        private Button convert;
        private Label t1Asm;
        private Label t2Asm;
        private Label t4Asm;
        private Label t8Asm;
        private Label t16Asm;
        private Label t32Asm;
        private Label t64Asm;
        private Button TestsButton;
        private Button restoreDefault;
        private RadioButton cSharp_radioBtn;
        private RadioButton asm_radioBtn;
    }
}
