using CatiaHoleAutomation.Services;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace CatiaHoleAutomation
{
    public partial class MainForm : Form
    {
        private readonly CatiaConnector _catiaConnector;

        public string CATPartFilePath;

        public string CsvFilePath;

        public MainForm()
        {
            InitializeComponent();
            _catiaConnector = new CatiaConnector();
            _catiaConnector.Connect();
        }

        private void btnSelectPart_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a CATIA Part";
                dialog.Filter = "CATIA Part (*.CATPart)|*.CATPart";
                dialog.Multiselect = false;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                if (!string.IsNullOrWhiteSpace(CATPartFilePath))
                {
                    dialog.FileName = CATPartFilePath;
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    CATPartFilePath = dialog.FileName;
                    tb_BrowsePart.Text = CATPartFilePath;
                }
            }
        }

        private void btnBrowseCsv_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select Hole Data CSV";
                dialog.Filter = "CSV Files (*.csv)|*.csv";
                dialog.Multiselect = false;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                if (!string.IsNullOrWhiteSpace(CsvFilePath))
                {
                    dialog.FileName = CsvFilePath;
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    CsvFilePath = dialog.FileName;
                    tbrowseCSV.Text = CsvFilePath;

                    LoadHoleDataFromCsv(CsvFilePath);
                }
            }
        }

        private void LoadHoleDataFromCsv(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath) || !File.Exists(csvPath))
            {
                MessageBox.Show("CSV file not found.");
                return;
            }

            dgvHoleData.Rows.Clear();

            var lines = File.ReadAllLines(csvPath);
            var slNo = 1;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(',');

                // Expected CSV order: X, Y, Radius, Depth, HoleType
                if (parts.Length < 5)
                {
                    continue;
                }

                var xText = parts[0].Trim();
                var yText = parts[1].Trim();

                // If the first non-empty row looks like a header, skip it.
                if (slNo == 1 && !double.TryParse(xText, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                {
                    continue;
                }

                var radiusText = parts[2].Trim();
                var depthText = parts[3].Trim();
                var holeTypeText = parts[4].Trim();

                dgvHoleData.Rows.Add(slNo, xText, yText, radiusText, depthText, holeTypeText);
                slNo++;
            }
        }

        private sealed class Hole2D
        {
            public int SlNo { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Radius { get; set; }
        }

        private static bool TryParseDouble(string text, out double result)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                result = 0;
                return false;
            }

            // First try invariant (e.g. 12.34), then fall back to current culture.
            return double.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result)
                   || double.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out result);
        }

        private bool TryReadHolesFromCsv(string csvPath, out System.Collections.Generic.List<Hole2D> holes, out string error)
        {
            holes = new System.Collections.Generic.List<Hole2D>();
            error = null;

            if (string.IsNullOrWhiteSpace(csvPath) || !File.Exists(csvPath))
            {
                error = "CSV file path is invalid or file does not exist.";
                return false;
            }

            var lines = File.ReadAllLines(csvPath);
            var slNo = 1;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(',');

                // Expected CSV order: X, Y, Radius, Depth, HoleType
                if (parts.Length < 5)
                {
                    continue;
                }

                var xText = parts[0].Trim();
                var yText = parts[1].Trim();

                // If the first non-empty row looks like a header, skip it.
                if (slNo == 1 && !TryParseDouble(xText, out _))
                {
                    continue;
                }

                if (!TryParseDouble(xText, out var x))
                {
                    error = $"Invalid X coordinate at line {i + 1} (SlNo {slNo}).";
                    return false;
                }

                if (!TryParseDouble(yText, out var y))
                {
                    error = $"Invalid Y coordinate at line {i + 1} (SlNo {slNo}).";
                    return false;
                }

                var radiusText = parts[2].Trim();
                if (!TryParseDouble(radiusText, out var radius) || radius <= 0)
                {
                    error = $"Invalid radius at line {i + 1} (SlNo {slNo}). Radius must be > 0.";
                    return false;
                }

                holes.Add(new Hole2D { SlNo = slNo, X = x, Y = y, Radius = radius });
                slNo++;
            }

            if (holes.Count == 0)
            {
                error = "No valid hole data found in CSV.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates whether any holes intersect/overlap in 2D.
        /// Intersect rule: distance between centers <= r1 + r2 (+ optional clearance).
        /// If intersections found, highlights the conflicting rows in the DataGridView.
        /// </summary>
        private bool ValidateNoHoleIntersections(string csvPath, double clearance = 0)
        {
            // Clear any previous highlighting
            ClearGridHighlighting();

            if (!TryReadHolesFromCsv(csvPath, out var holes, out var error))
            {
                MessageBox.Show(error, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var intersectingSlNos = new System.Collections.Generic.HashSet<int>();

            for (var i = 0; i < holes.Count; i++)
            {
                for (var j = i + 1; j < holes.Count; j++)
                {
                    var a = holes[i];
                    var b = holes[j];

                    var dx = a.X - b.X;
                    var dy = a.Y - b.Y;
                    var distSq = (dx * dx) + (dy * dy);

                    var minDist = a.Radius + b.Radius + clearance;
                    var minDistSq = minDist * minDist;

                    // If centers are closer than the sum of radii => circles overlap (holes intersect)
                    if (distSq <= minDistSq)
                    {
                        intersectingSlNos.Add(a.SlNo);
                        intersectingSlNos.Add(b.SlNo);
                    }
                }
            }

            if (intersectingSlNos.Count > 0)
            {
                HighlightIntersectingRows(intersectingSlNos);

                MessageBox.Show(
                    $"Hole intersections detected!\n\n" +
                    $"Conflicting holes (SlNo): {string.Join(", ", intersectingSlNos)}\n\n" +
                    $"Intersecting rows are highlighted in RED in the grid.",
                    "Intersection Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void ClearGridHighlighting()
        {
            foreach (DataGridViewRow row in dgvHoleData.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void HighlightIntersectingRows(System.Collections.Generic.HashSet<int> slNos)
        {
            foreach (DataGridViewRow row in dgvHoleData.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                // Column 0 is SlNo
                var slNoValue = row.Cells[0].Value;
                if (slNoValue != null && int.TryParse(slNoValue.ToString(), out var slNo))
                {
                    if (slNos.Contains(slNo))
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        private void btnHoleAnalyzer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CsvFilePath))
            {
                MessageBox.Show("Please select a CSV file first.", "Missing CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Run intersection validation and highlight conflicts
            if (ValidateNoHoleIntersections(CsvFilePath, clearance: 0))
            {
                MessageBox.Show(
                    "✓ All holes validated successfully!\n\nNo intersections detected.",
                    "Validation Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CsvFilePath))
                {
                    MessageBox.Show("Please select a CSV file first.", "Missing CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Open the part if path is provided
                if (!string.IsNullOrWhiteSpace(CATPartFilePath))
                {
                    _catiaConnector.OpenPart(CATPartFilePath);
                }
                else
                {
                    // Use active part
                    _catiaConnector.GetActivePart();
                }

                // Prompt user to select a face in CATIA
                MessageBox.Show(
                    "Please select a planar face in CATIA where holes will be created.\n\n" +
                    "Note: X and Y coordinates from CSV must be in the part's absolute coordinate system.",
                    "Face Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                var selectedFace = _catiaConnector.PromptUserToSelectFace();

                // Read hole data from CSV
                var holes = CsvReader.ReadHoleData(CsvFilePath);

                // Create holes on the selected face
                var holeCreator = new HoleCreator(_catiaConnector);
                holeCreator.CreateHolesOnFace(selectedFace, holes);

                MessageBox.Show(
                    $"Successfully created {holes.Count} holes on the selected face!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error creating holes:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

   
    }
}
