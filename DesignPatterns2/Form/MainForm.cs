using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.Drawing;
using DesignPatterns2.Classes.Matrix;
using DesignPatterns2.Classes.Visualization;
using DesignPatterns2.Classes.Decorators;
using DesignPatterns2.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using DesignPatterns2.Classes.Matrix;

namespace DesignPatterns2.Forms
{
    public partial class MainForm : Form
    {
        private Button btnGenerateRegular;
        private Button btnGenerateSparse;
        private Button btnGenerateHorizontalComposite;
        private Button btnGenerateVerticalComposite;
        private Button btnGenerateComplexComposite;
        private Button btnRenumber;
        private Button btnRestore;
        private CheckBox chkShowBorder;
        private Panel graphicsPanel;
        private TextBox consoleTextBox;
        private Label lblGraphics;
        private Label lblConsole;
        private Label lblInfo;

        private MatrixVisualization? currentVisualization;
        private IMatrix? currentMatrix;
        private IMatrix? originalMatrix;
        private RenumberingDecorator? decorator;
        private bool isDecorated = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Визуализация матриц - Паттерны Composite, Bridge и Decorator";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Информационная метка
            lblInfo = new Label
            {
                Text = "COMPOSITE PATTERN: Составные матрицы",
                Location = new Point(20, 5),
                Size = new Size(1150, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(lblInfo);

            // Первая строка кнопок - простые матрицы
            btnGenerateRegular = new Button
            {
                Text = "ОБЫЧНАЯ МАТРИЦА",
                Location = new Point(20, 30),
                Size = new Size(180, 35),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnGenerateRegular.Click += BtnGenerateRegular_Click;
            this.Controls.Add(btnGenerateRegular);

            btnGenerateSparse = new Button
            {
                Text = "РАЗРЕЖЕННАЯ МАТРИЦА",
                Location = new Point(210, 30),
                Size = new Size(200, 35),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnGenerateSparse.Click += BtnGenerateSparse_Click;
            this.Controls.Add(btnGenerateSparse);

            // Вторая строка кнопок - составные матрицы
            btnGenerateHorizontalComposite = new Button
            {
                Text = "ГОРИЗОНТАЛЬНАЯ ГРУППА",
                Location = new Point(20, 75),
                Size = new Size(200, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.LightGreen
            };
            btnGenerateHorizontalComposite.Click += BtnGenerateHorizontalComposite_Click;
            this.Controls.Add(btnGenerateHorizontalComposite);

            btnGenerateVerticalComposite = new Button
            {
                Text = "ВЕРТИКАЛЬНАЯ ГРУППА",
                Location = new Point(230, 75),
                Size = new Size(200, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.LightBlue
            };
            btnGenerateVerticalComposite.Click += BtnGenerateVerticalComposite_Click;
            this.Controls.Add(btnGenerateVerticalComposite);

            btnGenerateComplexComposite = new Button
            {
                Text = "СЛОЖНАЯ КОМПОЗИЦИЯ",
                Location = new Point(440, 75),
                Size = new Size(200, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.LightCoral
            };
            btnGenerateComplexComposite.Click += BtnGenerateComplexComposite_Click;
            this.Controls.Add(btnGenerateComplexComposite);

            // Кнопки управления
            btnRenumber = new Button
            {
                Text = "ПЕРЕНУМЕРОВАТЬ",
                Location = new Point(660, 30),
                Size = new Size(180, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                Enabled = false
            };
            btnRenumber.Click += BtnRenumber_Click;
            this.Controls.Add(btnRenumber);

            btnRestore = new Button
            {
                Text = "ВОССТАНОВИТЬ",
                Location = new Point(850, 30),
                Size = new Size(160, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                Enabled = false
            };
            btnRestore.Click += BtnRestore_Click;
            this.Controls.Add(btnRestore);

            // Checkbox для границы
            chkShowBorder = new CheckBox
            {
                Text = "Отображать границу",
                Location = new Point(660, 80),
                Size = new Size(200, 30),
                Checked = true,
                Font = new Font("Arial", 10)
            };
            chkShowBorder.CheckedChanged += ChkShowBorder_CheckedChanged;
            this.Controls.Add(chkShowBorder);

            // Label для графики
            lblGraphics = new Label
            {
                Text = "Графическая визуализация:",
                Location = new Point(20, 120),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblGraphics);

            // Графическая панель
            graphicsPanel = new Panel
            {
                Location = new Point(20, 150),
                Size = new Size(550, 600),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                AutoScroll = true
            };
            graphicsPanel.Paint += GraphicsPanel_Paint;
            this.Controls.Add(graphicsPanel);

            // Label для консоли
            lblConsole = new Label
            {
                Text = "Консольная визуализация:",
                Location = new Point(590, 120),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblConsole);

            // Консольная панель
            consoleTextBox = new TextBox
            {
                Location = new Point(590, 150),
                Size = new Size(580, 600),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 9),
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
                originalMatrix = new RegularMatrix(rows, columns);
                MatrixInitiator.FillMatrix((SomeMatrix)originalMatrix, nonZeroElements, maxValue);
            }
            else
            {
                originalMatrix = new RAZMatrix(rows, columns);
                MatrixInitiator.FillMatrix((SomeMatrix)originalMatrix, nonZeroElements, maxValue);
            }

            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            btnRenumber.Enabled = true;
            btnRestore.Enabled = false;

            VisualizeMatrix(isRegular);
        }

        /// <summary>
        /// Демонстрация горизонтальной группы матриц
        /// Создаем группу из 4 матриц: [2x2 | 3x3 | 5x1 | 1x1]
        /// </summary>
        private void BtnGenerateHorizontalComposite_Click(object? sender, EventArgs e)
        {
            // Создаем 4 матрицы разных размеров
            var matrix1 = new RegularMatrix(2, 2);
            FillMatrixWithValue(matrix1, 1);

            var matrix2 = new RegularMatrix(3, 3);
            FillMatrixWithValue(matrix2, 2);

            var matrix3 = new RegularMatrix(5, 1);
            FillMatrixWithValue(matrix3, 3);

            var matrix4 = new RegularMatrix(1, 1);
            FillMatrixWithValue(matrix4, 4);

            // Создаем горизонтальную группу
            var horizontalGroup = new HorizontalMatrixGroup();
            horizontalGroup.AddMatrix(matrix1);
            horizontalGroup.AddMatrix(matrix2);
            horizontalGroup.AddMatrix(matrix3);
            horizontalGroup.AddMatrix(matrix4);

            originalMatrix = horizontalGroup;
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            btnRenumber.Enabled = true;
            btnRestore.Enabled = false;

            MessageBox.Show(
                $"Создана горизонтальная группа [2x2 | 3x3 | 5x1 | 1x1]\n\n" +
                $"RowNum (max): {horizontalGroup.RowNum}\n" +
                $"ColumnNum (sum): {horizontalGroup.ColumnNum}",
                "Горизонтальная группа",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            VisualizeMatrix(true);
        }

        /// <summary>
        /// Демонстрация вертикальной группы через транспонирование
        /// </summary>
        private void BtnGenerateVerticalComposite_Click(object? sender, EventArgs e)
        {
            // Создаем 3 матрицы
            var matrix1 = new RegularMatrix(2, 5);
            FillMatrixWithValue(matrix1, 1);

            var matrix2 = new RegularMatrix(3, 5);
            FillMatrixWithValue(matrix2, 2);

            var matrix3 = new RegularMatrix(2, 5);
            FillMatrixWithValue(matrix3, 3);

            // Создаем вертикальную группу через транспонирование
            var verticalGroup = VerticalMatrixGroupHelper.CreateVerticalGroup(
                matrix1,
                matrix2,
                matrix3
            );

            originalMatrix = verticalGroup;
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            btnRenumber.Enabled = true;
            btnRestore.Enabled = false;

            MessageBox.Show(
                $"Создана вертикальная группа из 3 матриц:\n" +
                $"- Матрица 1: 2x5\n" +
                $"- Матрица 2: 3x5\n" +
                $"- Матрица 3: 2x5\n\n" +
                $"Результат: {verticalGroup.RowNum}x{verticalGroup.ColumnNum}\n" +
                $"RowNum (sum): {verticalGroup.RowNum}\n" +
                $"ColumnNum (max): {verticalGroup.ColumnNum}",
                "Вертикальная группа",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            VisualizeMatrix(true);
        }

        /// <summary>
        /// Демонстрация сложной композиции:
        /// Вертикальная группа из:
        /// 1. Горизонтальная группа [3x2 | 3x2 | 3x2]
        /// 2. Горизонтальная группа [2x3 | 2x3]
        /// 3. Обычная матрица 1x6
        /// </summary>
        private void BtnGenerateComplexComposite_Click(object? sender, EventArgs e)
        {
            // Первая горизонтальная группа
            var h1m1 = new RegularMatrix(3, 2);
            FillMatrixWithValue(h1m1, 1);
            var h1m2 = new RegularMatrix(3, 2);
            FillMatrixWithValue(h1m2, 2);
            var h1m3 = new RegularMatrix(3, 2);
            FillMatrixWithValue(h1m3, 3);

            var horizontalGroup1 = new HorizontalMatrixGroup();
            horizontalGroup1.AddMatrix(h1m1);
            horizontalGroup1.AddMatrix(h1m2);
            horizontalGroup1.AddMatrix(h1m3);

            // Вторая горизонтальная группа
            var h2m1 = new RegularMatrix(2, 3);
            FillMatrixWithValue(h2m1, 4);
            var h2m2 = new RegularMatrix(2, 3);
            FillMatrixWithValue(h2m2, 5);

            var horizontalGroup2 = new HorizontalMatrixGroup();
            horizontalGroup2.AddMatrix(h2m1);
            horizontalGroup2.AddMatrix(h2m2);

            // Третья - обычная матрица
            var matrix3 = new RegularMatrix(1, 6);
            FillMatrixWithValue(matrix3, 6);

            // Объединяем вертикально
            var complexComposite = VerticalMatrixGroupHelper.CreateVerticalGroup(
                horizontalGroup1,
                horizontalGroup2,
                matrix3
            );

            originalMatrix = complexComposite;
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            btnRenumber.Enabled = true;
            btnRestore.Enabled = false;

            MessageBox.Show(
                $"Создана сложная композиция:\n\n" +
                $"1. Горизонтальная группа [3x2 | 3x2 | 3x2] = 3x6\n" +
                $"2. Горизонтальная группа [2x3 | 2x3] = 2x6\n" +
                $"3. Обычная матрица 1x6\n\n" +
                $"Итого (вертикальная группа): {complexComposite.RowNum}x{complexComposite.ColumnNum}",
                "Сложная композиция",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            VisualizeMatrix(true);
        }

        private void BtnRenumber_Click(object? sender, EventArgs e)
        {
            if (originalMatrix == null) return;

            if (!isDecorated)
            {
                decorator = new RenumberingDecorator(originalMatrix);
                currentMatrix = decorator;
                isDecorated = true;
            }

            decorator?.Renumber();
            btnRestore.Enabled = true;

            bool isRegular = originalMatrix is RegularMatrix ||
                           originalMatrix is HorizontalMatrixGroup ||
                           originalMatrix is TransposeDecorator;
            VisualizeMatrix(isRegular);
        }

        private void BtnRestore_Click(object? sender, EventArgs e)
        {
            if (decorator == null || originalMatrix == null) return;

            decorator.Restore();
            btnRestore.Enabled = false;

            bool isRegular = originalMatrix is RegularMatrix ||
                           originalMatrix is HorizontalMatrixGroup ||
                           originalMatrix is TransposeDecorator;
            VisualizeMatrix(isRegular);
        }

        private void VisualizeMatrix(bool isRegular)
        {
            if (currentMatrix == null) return;

            graphicsPanel.Invalidate();

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

            int cellSize = 40; // Уменьшенный размер для больших матриц
            if (currentMatrix.RowNum > 6 || currentMatrix.ColumnNum > 6)
                cellSize = 30;

            GraphicsDrawing graphicsDrawing = new GraphicsDrawing(g, cellSize)
            {
                ShowBorder = chkShowBorder.Checked
            };

            bool isRegular = !(originalMatrix is RAZMatrix);

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
                bool isRegular = !(originalMatrix is RAZMatrix);
                VisualizeMatrix(isRegular);
            }
        }

        /// <summary>
        /// Вспомогательный метод для заполнения матрицы одинаковым значением
        /// </summary>
        private void FillMatrixWithValue(IMatrix matrix, float value)
        {
            for (int i = 0; i < matrix.RowNum; i++)
            {
                for (int j = 0; j < matrix.ColumnNum; j++)
                {
                    matrix.SetElement(i, j, value);
                }
            }
        }
    }
}