using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.Drawing;
using DesignPatterns2.Classes.MatrixClasses;
using DesignPatterns2.Classes.Visualization;
using DesignPatterns2.Classes.Decorators;
using DesignPatterns2.Interfaces;
using System.Drawing;
using System.Windows.Forms;

namespace DesignPatterns2.Forms
{
    public partial class MainForm : Form
    {
        private Button btnGenerateRegular;
        private Button btnGenerateSparse;
        private Button btnRenumber;
        private Button btnRestore;
        private CheckBox chkShowBorder;
        private Panel graphicsPanel;
        private TextBox consoleTextBox;
        private Label lblGraphics;
        private Label lblConsole;

        private MatrixVisualization? currentVisualization;
        private IMatrix? currentMatrix;          // Текущая матрица (может быть декорированной)
        private IMatrix? originalMatrix;          // Оригинальная матрица (без декоратора)
        private RenumberingDecorator? decorator;  // Декоратор 
        private bool isDecorated = false;        

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Визуализация матриц - Паттерны Bridge и Decorator";
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

            // Кнопка перенумерации
            btnRenumber = new Button
            {
                Text = "ПЕРЕНУМЕРОВАТЬ",
                Location = new Point(610, 20),
                Size = new Size(180, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Enabled = false
            };
            btnRenumber.Click += BtnRenumber_Click;
            this.Controls.Add(btnRenumber);

            // Кнопка восстановления
            btnRestore = new Button
            {
                Text = "ВОССТАНОВИТЬ",
                Location = new Point(810, 20),
                Size = new Size(160, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Enabled = false
            };
            btnRestore.Click += BtnRestore_Click;
            this.Controls.Add(btnRestore);

            // Checkbox для отображения границы
            chkShowBorder = new CheckBox
            {
                Text = "Отображать границу",
                Location = new Point(20, 70),
                Size = new Size(200, 30),
                Checked = true,
                Font = new Font("Arial", 10)
            };
            chkShowBorder.CheckedChanged += ChkShowBorder_CheckedChanged;
            this.Controls.Add(chkShowBorder);

            // Label для графической панели
            lblGraphics = new Label
            {
                Text = "Графическая визуализация:",
                Location = new Point(20, 110),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblGraphics);

            // Графическая панель
            graphicsPanel = new Panel
            {
                Location = new Point(20, 140),
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
                Location = new Point(490, 110),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblConsole);

            // Консольная панель (TextBox)
            consoleTextBox = new TextBox
            {
                Location = new Point(490, 140),
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

            // Создаем новую матрицу
            if (isRegular)
            {
                originalMatrix = new RegularMatrix(rows, columns);
                MatrixInitiator.FillMatrix((SomeMatrix)originalMatrix, nonZeroElements, maxValue);
            }
            else
            {
                originalMatrix = new RAZMatrix(rows, columns);
                MatrixInitiator.FillMatrix((SomeMatrix)originalMatrix, nonZeroElements, maxValue);
            }

            // Сбрасываем декорирование
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            // Активируем кнопку перенумерации, деактивируем восстановление
            btnRenumber.Enabled = true;
            btnRestore.Enabled = false;

            VisualizeMatrix(isRegular);
        }

        private void BtnRenumber_Click(object? sender, EventArgs e)
        {
            if (originalMatrix == null) return;

            if (!isDecorated)
            {
                // Создаем декоратор при первом нажатии
                decorator = new RenumberingDecorator(originalMatrix);
                currentMatrix = decorator;
                isDecorated = true;
            }

            // Выполняем перенумерацию
            decorator?.Renumber();

            // Обновляем состояние кнопок
            btnRestore.Enabled = true;

            // Перерисовываем матрицу
            bool isRegular = originalMatrix is RegularMatrix;
            VisualizeMatrix(isRegular);
        }

        private void BtnRestore_Click(object? sender, EventArgs e)
        {
            if (decorator == null || originalMatrix == null) return;

            // Восстанавливаем исходную нумерацию
            decorator.Restore();

            // Деактивируем кнопку восстановления
            btnRestore.Enabled = false;

            // Перерисовываем матрицу
            bool isRegular = originalMatrix is RegularMatrix;
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

            bool isRegular = originalMatrix is RegularMatrix;

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
            if (currentMatrix != null && originalMatrix != null)
            {
                bool isRegular = originalMatrix is RegularMatrix;
                VisualizeMatrix(isRegular);
            }
        }
    }
}