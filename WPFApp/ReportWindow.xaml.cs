using BusinessObjects;
using OfficeOpenXml;
using Services.Implement;
using Services.Interface;
using System.IO;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private INewsArticleSvc _newsArticleSvc;
        public ReportWindow()
        {
            InitializeComponent();
            _newsArticleSvc = new NewsArticleSvc();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                // Get the StartDate and EndDate from your UI or any other source
                DateTime startDate = dpStartDate.SelectedDate.Value; // Set your start date here
                DateTime endDate = dpEndDate.SelectedDate.Value;   // Set your end date here

                // Retrieve news articles within the specified period
                List<NewsArticle> newsArticles = _newsArticleSvc.GetNewsArticlesByPeriod(startDate, endDate);

                // Sort the news articles in descending order based on their creation date
                newsArticles = newsArticles.OrderByDescending(n => n.CreatedDate).ToList();

                // Generate Excel report
                GenerateExcelReport(newsArticles, startDate, endDate);
            }
            else
            {
                // Display a message indicating that both start and end dates must be selected
                MessageBox.Show("Please select both start and end dates.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (dpStartDate.SelectedDate == null && dpEndDate.SelectedDate == null)
            {
                return false;
            }
            else
            {
                // Validation passed
                return true;
            }
        }


        private void GenerateExcelReport(List<NewsArticle> newsArticles, DateTime startDate, DateTime endDate)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            btnExport.IsEnabled = false;
            // Create a new Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Add a new worksheet to the Excel package
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("News Articles");

                // Define headers
                string[] headers = { "News Article ID", "Title", "Category", "Tags", "Created By", "Created Date", "Modified Date", "Status" };

                // Write headers to the worksheet
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Write news articles data to the worksheet
                for (int i = 0; i < newsArticles.Count; i++)
                {
                    NewsArticle article = newsArticles[i];
                    worksheet.Cells[i + 2, 1].Value = article.NewsArticleId;
                    worksheet.Cells[i + 2, 2].Value = article.NewsTitle;
                    worksheet.Cells[i + 2, 3].Value = article.Category?.CategoryName;

                    string tags = string.Join(", ", article.Tags.Select(t => t.TagName));
                    worksheet.Cells[i + 2, 4].Value = tags;

                    worksheet.Cells[i + 2, 5].Value = article.CreatedBy?.AccountName;
                    worksheet.Cells[i + 2, 6].Value = article.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[i + 2, 7].Value = article.ModifiedDate?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[i + 2, 8].Value = (bool)article.NewsStatus ? "Active" : "Inactive";
                }

                // Save the Excel package to a file
                string folderPath = Path.Combine(Environment.CurrentDirectory, "Reports");
                Directory.CreateDirectory(folderPath); // Create the Reports folder if it doesn't exist
                string filePath = Path.Combine(folderPath, "NewsReport_From_" + startDate.ToString("yyyy-MM-dd") + "_To_" + endDate.ToString("yyyy-MM-dd") + "_CreateAt_" + DateTime.Now.ToString("HH-mm-ss") + ".xlsx");

                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);
                MessageBox.Show("Export to " + filePath + " success!", "Report", MessageBoxButton.OK, MessageBoxImage.Information);
                btnExport.IsEnabled = true;

            }
        }
    }
}
