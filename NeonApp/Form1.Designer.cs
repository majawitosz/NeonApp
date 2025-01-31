
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
            convert = new Button();
            trackBarThreads = new TrackBar();
            threadLabel = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            restoreDefault = new Button();
            timeLabel = new Label();
            cSharp_radioBtn = new RadioButton();
            asm_radioBtn = new RadioButton();
            saveButton = new Button();
            pictureBoxNeon = new PictureBox();
            pictureBoxOriginal = new PictureBox();
            neonLabel = new Label();
            labelOriginal = new Label();
            radioButton1 = new RadioButton();
            colorPicker = new GroupBox();
            radioButton5 = new RadioButton();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chosenImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarThreads).BeginInit();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNeon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOriginal).BeginInit();
            colorPicker.SuspendLayout();
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
            flowLayoutPanel1.Location = new Point(31, 431);
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
            // convert
            // 
            convert.Location = new Point(661, 576);
            convert.Margin = new Padding(4, 3, 4, 3);
            convert.Name = "convert";
            convert.Size = new Size(200, 27);
            convert.TabIndex = 8;
            convert.Text = "Convert";
            convert.UseVisualStyleBackColor = true;
            convert.Click += Convert_Click;
            // 
            // trackBarThreads
            // 
            trackBarThreads.Location = new Point(4, 63);
            trackBarThreads.Margin = new Padding(4, 3, 4, 3);
            trackBarThreads.Maximum = 0;
            trackBarThreads.Name = "trackBarThreads";
            trackBarThreads.Size = new Size(331, 45);
            trackBarThreads.TabIndex = 1;
            trackBarThreads.Scroll += trackBarThreads_Scroll;
            // 
            // threadLabel
            // 
            threadLabel.Location = new Point(4, 45);
            threadLabel.Margin = new Padding(4, 0, 4, 0);
            threadLabel.Name = "threadLabel";
            threadLabel.Size = new Size(331, 15);
            threadLabel.TabIndex = 2;
            threadLabel.Text = "Number of Threads";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(restoreDefault);
            flowLayoutPanel2.Controls.Add(threadLabel);
            flowLayoutPanel2.Controls.Add(trackBarThreads);
            flowLayoutPanel2.Controls.Add(timeLabel);
            flowLayoutPanel2.Location = new Point(504, 438);
            flowLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(357, 136);
            flowLayoutPanel2.TabIndex = 3;
            // 
            // restoreDefault
            // 
            restoreDefault.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            restoreDefault.Location = new Point(4, 3);
            restoreDefault.Margin = new Padding(4, 3, 4, 15);
            restoreDefault.Name = "restoreDefault";
            restoreDefault.Size = new Size(144, 27);
            restoreDefault.TabIndex = 17;
            restoreDefault.Text = "Restore Default";
            restoreDefault.UseVisualStyleBackColor = true;
            restoreDefault.Click += restoreDefault_Click;
            // 
            // timeLabel
            // 
            timeLabel.AutoSize = true;
            timeLabel.Location = new Point(4, 111);
            timeLabel.Margin = new Padding(4, 0, 4, 0);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(36, 15);
            timeLabel.TabIndex = 3;
            timeLabel.Text = "Time:";
            // 
            // cSharp_radioBtn
            // 
            cSharp_radioBtn.AutoSize = true;
            cSharp_radioBtn.Location = new Point(508, 605);
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
            asm_radioBtn.Location = new Point(508, 580);
            asm_radioBtn.Margin = new Padding(4, 3, 4, 3);
            asm_radioBtn.Name = "asm_radioBtn";
            asm_radioBtn.Size = new Size(110, 19);
            asm_radioBtn.TabIndex = 20;
            asm_radioBtn.TabStop = true;
            asm_radioBtn.Text = "ASM x64 Library";
            asm_radioBtn.Checked = true;
            asm_radioBtn.UseVisualStyleBackColor = true;
            asm_radioBtn.CheckedChanged += asm_radioBtn_CheckedChanged;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(661, 609);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(200, 27);
            saveButton.TabIndex = 21;
            saveButton.Text = "Save Image";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += svaeButton_Click;
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
            pictureBoxOriginal.Size = new Size(411, 354);
            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOriginal.TabIndex = 5;
            pictureBoxOriginal.TabStop = false;
            // 
            // neonLabel
            // 
            neonLabel.AutoSize = true;
            neonLabel.Font = new Font("Impact", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            neonLabel.Location = new Point(601, 10);
            neonLabel.Margin = new Padding(4, 0, 4, 0);
            neonLabel.Name = "neonLabel";
            neonLabel.Size = new Size(157, 36);
            neonLabel.TabIndex = 6;
            neonLabel.Text = "NEON EFFECT";
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
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.BackColor = SystemColors.ButtonHighlight;
            radioButton1.Image = Properties.Resources.pink;
            radioButton1.ImageAlign = ContentAlignment.MiddleRight;
            radioButton1.Location = new Point(6, 22);
            radioButton1.Name = "radioButton1";
            radioButton1.Padding = new Padding(0, 0, 0, 1);
            radioButton1.Size = new Size(68, 21);
            radioButton1.TabIndex = 21;
            radioButton1.TabStop = true;
            radioButton1.Text = "Pink";
            radioButton1.Checked = true;
            radioButton1.TextImageRelation = TextImageRelation.ImageBeforeText;
            radioButton1.UseVisualStyleBackColor = false;
            radioButton1.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // colorPicker
            // 
            colorPicker.Controls.Add(radioButton5);
            colorPicker.Controls.Add(radioButton4);
            colorPicker.Controls.Add(radioButton3);
            colorPicker.Controls.Add(radioButton2);
            colorPicker.Controls.Add(radioButton1);
            colorPicker.Location = new Point(284, 431);
            colorPicker.Name = "colorPicker";
            colorPicker.Size = new Size(200, 148);
            colorPicker.TabIndex = 22;
            colorPicker.TabStop = false;
            colorPicker.Text = "Color Picker";
            // 
            // radioButton5
            // 
            radioButton5.AutoSize = true;
            radioButton5.Image = Properties.Resources.yellow;
            radioButton5.Location = new Point(6, 122);
            radioButton5.Name = "radioButton5";
            radioButton5.Padding = new Padding(0, 0, 0, 1);
            radioButton5.Size = new Size(79, 21);
            radioButton5.TabIndex = 25;
            radioButton5.TabStop = true;
            radioButton5.Text = "Yellow";
            radioButton5.TextImageRelation = TextImageRelation.ImageBeforeText;
            radioButton5.UseVisualStyleBackColor = true;
            radioButton5.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Image = Properties.Resources.orange;
            radioButton4.Location = new Point(6, 97);
            radioButton4.Name = "radioButton4";
            radioButton4.Padding = new Padding(0, 0, 0, 1);
            radioButton4.Size = new Size(84, 21);
            radioButton4.TabIndex = 24;
            radioButton4.TabStop = true;
            radioButton4.Text = "Orange";
            radioButton4.TextImageRelation = TextImageRelation.ImageBeforeText;
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Image = Properties.Resources.cyan;
            radioButton3.Location = new Point(6, 72);
            radioButton3.Name = "radioButton3";
            radioButton3.Padding = new Padding(0, 0, 0, 1);
            radioButton3.Size = new Size(72, 21);
            radioButton3.TabIndex = 23;
            radioButton3.TabStop = true;
            radioButton3.Text = "Cyan";
            radioButton3.TextImageRelation = TextImageRelation.ImageBeforeText;
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Image = Properties.Resources.green;
            radioButton2.Location = new Point(6, 47);
            radioButton2.Name = "radioButton2";
            radioButton2.Padding = new Padding(0, 0, 0, 1);
            radioButton2.Size = new Size(76, 21);
            radioButton2.TabIndex = 22;
            radioButton2.TabStop = true;
            radioButton2.Text = "Green";
            radioButton2.TextImageRelation = TextImageRelation.ImageBeforeText;
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(915, 706);
            Controls.Add(cSharp_radioBtn);
            Controls.Add(colorPicker);
            Controls.Add(asm_radioBtn);
            Controls.Add(saveButton);
            Controls.Add(labelOriginal);
            Controls.Add(neonLabel);
            Controls.Add(pictureBoxOriginal);
            Controls.Add(pictureBoxNeon);
            Controls.Add(convert);
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
            colorPicker.ResumeLayout(false);
            colorPicker.PerformLayout();
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
        private System.Windows.Forms.Label neonLabel;
        private System.Windows.Forms.Label labelOriginal;
        private Button convert;
        private Button restoreDefault;
        private RadioButton cSharp_radioBtn;
        private RadioButton asm_radioBtn;
        private Button saveButton;
        private RadioButton radioButton1;
        private GroupBox colorPicker;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton5;
    }
}
