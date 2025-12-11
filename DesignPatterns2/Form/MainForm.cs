using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.Drawing;
using DesignPatterns2.Classes.Matrix;
using DesignPatterns2.Classes.Visualization;
using DesignPatterns2.Classes.Decorators;
using DesignPatterns2.Interfaces;
//using DesignPatterns2.Command;
using DesignPatterns2.Classes.Comand;

namespace DesignPatterns2.Forms
{
    public partial class MainForm : Form
    {
        // UI Controls
        private Button btnGenerateRegular;
        private Button btnGenerateSparse;
        private Button btnGenerateHorizontalComposite;
        private Button btnGenerateVerticalComposite;
        private Button btnGenerateComplexComposite;
        private Button btnChange;           // ✨ NEW: Кнопка "Изменить"
        private Button btnUndo;             // ✨ NEW: Кнопка "Отменить"
        private Button btnClearHistory;     // ✨ NEW: Очистить историю
        private Button btnRenumber;
        private Button btnRestore;
        private CheckBox chkShowBorder;
        private Panel graphicsPanel;
        private TextBox consoleTextBox;
        private TextBox commandHistoryTextBox;  // ✨ NEW: История команд
        private Label lblGraphics;
        private Label lblConsole;
        private Label lblInfo;
        private Label lblCommandHistory;    // ✨ NEW

        // Matrix state
        private MatrixVisualization? currentVisualization;
        private IMatrix? currentMatrix;
        private IMatrix? originalMatrix;
        private RenumberingDecorator? decorator;
        private bool isDecorated = false;

        // Command Manager
        private readonly CommandManager _commandManager;

        public MainForm()
        {
            // Получаем Singleton CommandManager
            _commandManager = CommandManager.Instance;

            InitializeComponent();

            // Инициализация приложения через команду
            InitializeApplication();
        }

        /// <summary>
        /// Инициализация приложения через Command Pattern
        /// </summary>
        private void InitializeApplication()
        {
            var initCommand = new InitializeApplicationCommand("Matrix Command App", "2.0");
            _commandManager.RegisterCommand(initCommand);

            UpdateCommandHistory();
            UpdateUndoButton();
        }

        private void InitializeComponent()
        {
            this.Text = "Матрицы - Паттерн Command (Отмена операций)";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Информационная метка
            lblInfo = new Label
            {
                Text = "COMMAND PATTERN: Отмена операций",
                Location = new Point(20, 5),
                Size = new Size(1350, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(lblInfo);

            #region Matrix Generation Buttons (First Row)

            btnGenerateRegular = new Button
            {
                Text = "ОБЫЧНАЯ МАТРИЦА",
                Location = new Point(20, 30),
                Size = new Size(160, 30),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            btnGenerateRegular.Click += BtnGenerateRegular_Click;
            this.Controls.Add(btnGenerateRegular);

            btnGenerateSparse = new Button
            {
                Text = "РАЗРЕЖЕННАЯ",
                Location = new Point(190, 30),
                Size = new Size(150, 30),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            btnGenerateSparse.Click += BtnGenerateSparse_Click;
            this.Controls.Add(btnGenerateSparse);

            btnGenerateHorizontalComposite = new Button
            {
                Text = "ГОРИЗОНТ. ГРУППА",
                Location = new Point(350, 30),
                Size = new Size(160, 30),
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.LightGreen
            };
            btnGenerateHorizontalComposite.Click += BtnGenerateHorizontalComposite_Click;
            this.Controls.Add(btnGenerateHorizontalComposite);

            btnGenerateVerticalComposite = new Button
            {
                Text = "ВЕРТИК. ГРУППА",
                Location = new Point(520, 30),
                Size = new Size(150, 30),
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.LightBlue
            };
            btnGenerateVerticalComposite.Click += BtnGenerateVerticalComposite_Click;
            this.Controls.Add(btnGenerateVerticalComposite);

            btnGenerateComplexComposite = new Button
            {
                Text = "КОМПОЗИЦИЯ",
                Location = new Point(680, 30),
                Size = new Size(140, 30),
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.LightCoral
            };
            btnGenerateComplexComposite.Click += BtnGenerateComplexComposite_Click;
            this.Controls.Add(btnGenerateComplexComposite);

            #endregion

            #region Command Buttons (Second Row) ✨ NEW

            btnChange = new Button
            {
                Text = "ИЗМЕНИТЬ",
                Location = new Point(20, 70),
                Size = new Size(160, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Enabled = false
            };
            btnChange.Click += BtnChange_Click;
            this.Controls.Add(btnChange);

            btnUndo = new Button
            {
                Text = "ОТМЕНИТЬ",
                Location = new Point(190, 70),
                Size = new Size(160, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Enabled = false
            };
            btnUndo.Click += BtnUndo_Click;
            this.Controls.Add(btnUndo);

            btnClearHistory = new Button
            {
                Text = "ОЧИСТИТЬ ИСТОРИЮ",
                Location = new Point(360, 70),
                Size = new Size(180, 40),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnClearHistory.Click += BtnClearHistory_Click;
            this.Controls.Add(btnClearHistory);

            #endregion

            #region Decorator Buttons (Third Row)

            btnRenumber = new Button
            {
                Text = "ПЕРЕНУМЕРОВАТЬ",
                Location = new Point(550, 70),
                Size = new Size(160, 40),
                Font = new Font("Arial", 9, FontStyle.Bold),
                Enabled = false
            };
            btnRenumber.Click += BtnRenumber_Click;
            this.Controls.Add(btnRenumber);

            btnRestore = new Button
            {
                Text = "ВОССТАНОВИТЬ",
                Location = new Point(720, 70),
                Size = new Size(150, 40),
                Font = new Font("Arial", 9, FontStyle.Bold),
                Enabled = false
            };
            btnRestore.Click += BtnRestore_Click;
            this.Controls.Add(btnRestore);

            chkShowBorder = new CheckBox
            {
                Text = "Границы",
                Location = new Point(880, 80),
                Size = new Size(100, 30),
                Checked = true,
                Font = new Font("Arial", 9)
            };
            chkShowBorder.CheckedChanged += ChkShowBorder_CheckedChanged;
            this.Controls.Add(chkShowBorder);

            #endregion

            #region Graphics Panel

            lblGraphics = new Label
            {
                Text = "Графическая визуализация:",
                Location = new Point(20, 120),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblGraphics);

            graphicsPanel = new Panel
            {
                Location = new Point(20, 150),
                Size = new Size(550, 680),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                AutoScroll = true
            };
            graphicsPanel.Paint += GraphicsPanel_Paint;
            this.Controls.Add(graphicsPanel);

            #endregion

            #region Console Panel

            lblConsole = new Label
            {
                Text = "Консольная визуализация:",
                Location = new Point(590, 120),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblConsole);

            consoleTextBox = new TextBox
            {
                Location = new Point(590, 150),
                Size = new Size(450, 680),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 9),
                ScrollBars = ScrollBars.Both,
                BackColor = Color.Black,
                ForeColor = Color.LimeGreen
            };
            this.Controls.Add(consoleTextBox);

            #endregion

            #region Command History Panel ✨ NEW

            lblCommandHistory = new Label
            {
                Text = "История команд:",
                Location = new Point(1060, 120),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };
            this.Controls.Add(lblCommandHistory);

            commandHistoryTextBox = new TextBox
            {
                Location = new Point(1060, 150),
                Size = new Size(310, 680),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 8),
                ScrollBars = ScrollBars.Both,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Yellow
            };
            this.Controls.Add(commandHistoryTextBox);

            #endregion
        }

        #region Matrix Generation Methods

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
            int rows = 5;
            int columns = 5;
            int nonZeroElements = 8;
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

            EnableCommandButtons();
            VisualizeMatrix(isRegular);
        }

        private void BtnGenerateHorizontalComposite_Click(object? sender, EventArgs e)
        {
            var matrix1 = new RegularMatrix(2, 2);
            FillMatrixWithValue(matrix1, 1);

            var matrix2 = new RegularMatrix(3, 3);
            FillMatrixWithValue(matrix2, 2);

            var matrix3 = new RegularMatrix(5, 1);
            FillMatrixWithValue(matrix3, 3);

            var matrix4 = new RegularMatrix(1, 1);
            FillMatrixWithValue(matrix4, 4);

            var horizontalGroup = new HorizontalMatrixGroup();
            horizontalGroup.AddMatrix(matrix1);
            horizontalGroup.AddMatrix(matrix2);
            horizontalGroup.AddMatrix(matrix3);
            horizontalGroup.AddMatrix(matrix4);

            originalMatrix = horizontalGroup;
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            EnableCommandButtons();
            VisualizeMatrix(true);
        }

        private void BtnGenerateVerticalComposite_Click(object? sender, EventArgs e)
        {
            var matrix1 = new RegularMatrix(2, 5);
            FillMatrixWithValue(matrix1, 1);

            var matrix2 = new RegularMatrix(3, 5);
            FillMatrixWithValue(matrix2, 2);

            var matrix3 = new RegularMatrix(2, 5);
            FillMatrixWithValue(matrix3, 3);

            var verticalGroup = VerticalMatrixGroupHelper.CreateVerticalGroup(matrix1, matrix2, matrix3);

            originalMatrix = verticalGroup;
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            EnableCommandButtons();
            VisualizeMatrix(true);
        }

        private void BtnGenerateComplexComposite_Click(object? sender, EventArgs e)
        {
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

            var h2m1 = new RegularMatrix(2, 3);
            FillMatrixWithValue(h2m1, 4);
            var h2m2 = new RegularMatrix(2, 3);
            FillMatrixWithValue(h2m2, 5);

            var horizontalGroup2 = new HorizontalMatrixGroup();
            horizontalGroup2.AddMatrix(h2m1);
            horizontalGroup2.AddMatrix(h2m2);

            var matrix3 = new RegularMatrix(1, 6);
            FillMatrixWithValue(matrix3, 6);

            var complexComposite = VerticalMatrixGroupHelper.CreateVerticalGroup(
                horizontalGroup1,
                horizontalGroup2,
                matrix3
            );

            originalMatrix = complexComposite;
            currentMatrix = originalMatrix;
            decorator = null;
            isDecorated = false;

            EnableCommandButtons();
            VisualizeMatrix(true);
        }

        #endregion

        #region Command Methods ✨ NEW

        /// <summary>
        /// Кнопка "ИЗМЕНИТЬ" - случайное изменение матрицы через Command
        /// </summary>
        private void BtnChange_Click(object? sender, EventArgs e)
        {
            if (currentMatrix == null) return;

            try
            {
                // Создаем команду случайного изменения
                var changeCommand = new RandomChangeMatrixCommand(
                    currentMatrix,
                    changesCount: 5,
                    maxValue: 9
                );

                // Регистрируем и выполняем через CommandManager
                _commandManager.RegisterCommand(changeCommand);

                // Обновляем UI
                UpdateCommandHistory();
                UpdateUndoButton();
                VisualizeMatrix(!(originalMatrix is RAZMatrix));

                // Показываем уведомление
                ShowNotification("Матрица изменена", Color.Orange);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при изменении матрицы:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Кнопка "ОТМЕНИТЬ" - отмена последней команды
        /// </summary>
        private void BtnUndo_Click(object? sender, EventArgs e)
        {
            try
            {
                bool success = _commandManager.UndoLastCommand();

                if (success)
                {
                    // Обновляем UI
                    UpdateCommandHistory();
                    UpdateUndoButton();
                    VisualizeMatrix(!(originalMatrix is RAZMatrix));

                    // Показываем уведомление
                    ShowNotification("Команда отменена", Color.IndianRed);
                }
                else
                {
                    MessageBox.Show(
                        "Нет команд для отмены",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при отмене команды:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Очистить историю команд
        /// </summary>
        private void BtnClearHistory_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите очистить историю команд?\n" +
                "После этого отмена будет невозможна!",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                _commandManager.ClearHistory();
                UpdateCommandHistory();
                UpdateUndoButton();
                ShowNotification("История очищена", Color.Gray);
            }
        }

        #endregion

        #region Decorator Methods (existing)

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

        #endregion

        #region Visualization

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

            int cellSize = 40;
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

        #endregion

        #region Helper Methods

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

        private void EnableCommandButtons()
        {
            btnChange.Enabled = true;
            btnRenumber.Enabled = true;
            btnRestore.Enabled = false;
            UpdateUndoButton();
        }

        /// <summary>
        /// Обновить состояние кнопки "Отменить"
        /// </summary>
        private void UpdateUndoButton()
        {
            btnUndo.Enabled = _commandManager.CanUndo;

            // Обновляем текст кнопки с количеством команд
            int undoableCount = _commandManager.CommandHistory.Count(c => c.CanUndo);
            btnUndo.Text = undoableCount > 0
                ? $"ОТМЕНИТЬ ({undoableCount})"
                : "ОТМЕНИТЬ";
        }

        /// <summary>
        /// Обновить панель истории команд
        /// </summary>
        private void UpdateCommandHistory()
        {
            if (_commandManager.HistoryCount == 0)
            {
                commandHistoryTextBox.Text = "История команд пуста\r\n\r\nНажмите 'ИЗМЕНИТЬ' для создания команды";
                return;
            }

            var history = new System.Text.StringBuilder();
            history.AppendLine("=== ИСТОРИЯ КОМАНД ===\r\n");

            var commands = _commandManager.CommandHistory.Reverse().ToList();
            for (int i = 0; i < commands.Count; i++)
            {
                var cmd = commands[i];
                string undoMark = cmd.CanUndo ? "✓" : "✗";
                history.AppendLine($"[{commands.Count - i}] {undoMark} {cmd.Description}");
                history.AppendLine($"    Время: {cmd.ExecutedAt:HH:mm:ss}");
                history.AppendLine($"    Отмена: {(cmd.CanUndo ? "Возможна" : "Невозможна")}\r\n");
            }

            history.AppendLine($"\r\nВсего команд: {_commandManager.HistoryCount}");
            history.AppendLine($"Доступно для отмены: {_commandManager.CommandHistory.Count(c => c.CanUndo)}");

            commandHistoryTextBox.Text = history.ToString();

            // Прокручиваем вверх
            commandHistoryTextBox.SelectionStart = 0;
            commandHistoryTextBox.ScrollToCaret();
        }

        /// <summary>
        /// Показать временное уведомление
        /// </summary>
        private void ShowNotification(string message, Color color)
        {
            var originalColor = lblInfo.BackColor;
            var originalText = lblInfo.Text;

            lblInfo.Text = message;
            lblInfo.BackColor = color;
            lblInfo.ForeColor = Color.White;

            // Таймер для восстановления через 2 секунды
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += (s, e) =>
            {
                lblInfo.Text = originalText;
                lblInfo.BackColor = originalColor;
                lblInfo.ForeColor = Color.DarkBlue;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        #endregion
    }
}