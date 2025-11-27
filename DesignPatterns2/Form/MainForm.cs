using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.Drawing;
using DesignPatterns2.Classes.MatrixClasses;
using DesignPatterns2.Classes.Visualization;
using DesignPatterns2.Interfaces;
using System.Drawing;
using System.Windows.Forms;

namespace DesignPatterns2.Forms
{
        public partial class MainForm : Form
    {
            private Button btnGenerateRegular;
            private Button btnGenerateSparse;
            private CheckBox chkShowBorder;
            private Panel graphicsPanel;
            private TextBox consoleTextBox;
            private Label lblGraphics;
            private Label lblConsole;

            private MatrixVisualization? currentVisualization;
            private IMatrix? currentMatrix;

            public MainForm()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Визуализация матриц - Паттерн Bridge";
                this.Size = new Size(1000, 700);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Кнопка генерации обычной матрицы
                btnGenerateRegular = new Button
                {
                    Text = "ГЕНЕРАЦИЯ ОБЫЧНОЙ МАТРИЦЫ",
                    Location = new Point(20, 20),
                    Size = new Size(250, 40),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                btnGenerateRegular.Click += BtnGenerateRegular_Click;
                this.Controls.Add(btnGenerateRegular);

                // Кнопка генерации разреженной матрицы
                btnGenerateSparse = new Button
                {
                    Text = "ГЕНЕРАЦИЯ РАЗРЕЖЕННОЙ МАТРИЦЫ",
                    Location = new Point(290, 20),
                    Size = new Size(300, 40),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                btnGenerateSparse.Click += BtnGenerateSparse_Click;
                this.Controls.Add(btnGenerateSparse);

                // Checkbox для отображения границы
                chkShowBorder = new CheckBox
                {
                    Text = "Отображать границу матрицы",
                    Location = new Point(610, 25),
                    Size = new Size(250, 30),
                    Checked = true,
                    Font = new Font("Arial", 10)
                };
                chkShowBorder.CheckedChanged += ChkShowBorder_CheckedChanged;
                this.Controls.Add(chkShowBorder);

                // Label для графической панели
                lblGraphics = new Label
                {
                    Text = "Графическая визуализация:",
                    Location = new Point(20, 80),
                    Size = new Size(300, 20),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                this.Controls.Add(lblGraphics);

                // Графическая панель
                graphicsPanel = new Panel
                {
                    Location = new Point(20, 110),
                    Size = new Size(450, 450),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White
                };
                graphicsPanel.Paint += GraphicsPanel_Paint;
                this.Controls.Add(graphicsPanel);

                // Label для консольной панели
                lblConsole = new Label
                {
                    Text = "Консольная визуализация:",
                    Location = new Point(490, 80),
                    Size = new Size(300, 20),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                this.Controls.Add(lblConsole);

                // Консольная панель (TextBox)
                consoleTextBox = new TextBox
                {
                    Location = new Point(490, 110),
                    Size = new Size(480, 450),
                    Multiline = true,
                    ReadOnly = true,
                    Font = new Font("Consolas", 10),
                    ScrollBars = ScrollBars.Both,
                    BackColor = Color.Black,
                    ForeColor = Color.LimeGreen
                };
                this.Controls.Add(consoleTextBox);
            }

            private void BtnGenerateRegular_Click(object? sender, EventArgs e)
            {
                GenerateMatrix(isRegular: true);
            }

            private void BtnGenerateSparse_Click(object? sender, EventArgs e)
            {
                GenerateMatrix(isRegular: false);
            }

            private void GenerateMatrix(bool isRegular)
            {
                int rows = 4;
                int columns = 4;
                int nonZeroElements = 6;
                int maxValue = 9;

                if (isRegular)
                {
                    currentMatrix = new RegularMatrix(rows, columns);
                    MatrixInitiator.FillMatrix((SomeMatrix)currentMatrix, nonZeroElements, maxValue);
                }
                else
                {
                    currentMatrix = new RAZMatrix(rows, columns);
                    MatrixInitiator.FillMatrix((SomeMatrix)currentMatrix, nonZeroElements, maxValue);
                }

                VisualizeMatrix(isRegular);
            }

            private void VisualizeMatrix(bool isRegular)
            {
                if (currentMatrix == null) return;

                graphicsPanel.Invalidate(); // Перерисовка панели

                // Консольная визуализация
                ConsoleDrawing consoleDrawing = new ConsoleDrawing
                {
                    ShowBorder = chkShowBorder.Checked
                };

                if (isRegular)
                {
                    currentVisualization = new RegularMatrixVisualization(currentMatrix, consoleDrawing);
                }
                else
                {
                    currentVisualization = new SparseMatrixVisualization(currentMatrix, consoleDrawing);
                }

                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);
                    currentVisualization.Visualize();
                    consoleTextBox.Text = sw.ToString();
                }

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }

            private void GraphicsPanel_Paint(object? sender, PaintEventArgs e)
            {
                if (currentMatrix == null) return;

                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                GraphicsDrawing graphicsDrawing = new GraphicsDrawing(g, 80)
                {
                    ShowBorder = chkShowBorder.Checked
                };

                bool isRegular = currentMatrix is RegularMatrix;

                MatrixVisualization visualization;
                if (isRegular)
                {
                    visualization = new RegularMatrixVisualization(currentMatrix, graphicsDrawing);
                }
                else
                {
                    visualization = new SparseMatrixVisualization(currentMatrix, graphicsDrawing);
                }

                visualization.Visualize();
            }

            private void ChkShowBorder_CheckedChanged(object? sender, EventArgs e)
            {
                if (currentMatrix != null)
                {
                    bool isRegular = currentMatrix is RegularMatrix;
                    VisualizeMatrix(isRegular);
                }
            }
        }
}
